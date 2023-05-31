// learn more at https://aka.ms/dab

using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using DabHelper.Library;

namespace DabHelpers;

public partial class RestHelper<T> where T : new()
{
    private readonly string baseUri;
    private readonly string? restName;
    private readonly HttpClient httpClient;

    public RestHelper(string baseUri, HttpClient? httpClient = null)
    {
        this.baseUri = Uri.TryCreate(baseUri, UriKind.Absolute, out var uri) ? uri.ToString() : throw new ArgumentException("Uri invalid.", nameof(baseUri));
        this.httpClient = httpClient ?? new();
    }

    /// <summary>
    /// Get a list of records
    /// </summary>
    /// <param name="select" cref="https://learn.microsoft.com/en-us/azure/data-api-builder/rest#select">The query parameter $select allow to specify which fields must be returned. </param>
    /// <param name="filter" cref="https://learn.microsoft.com/en-us/azure/data-api-builder/rest#filter">The value of the $filter option is predicate expression (an expression that returns a boolean value) using entity's fields. </param>
    /// <param name="orderby" cref="https://learn.microsoft.com/en-us/azure/data-api-builder/rest#orderby">The value of the orderby parameter is a comma-separated list of expressions used to sort the items.</param>
    /// <param name="first" cref="https://learn.microsoft.com/en-us/azure/data-api-builder/rest#first-and-after">The query parameter $first allows to limit the number of items returned.</param>
    /// <param name="after" cref="https://learn.microsoft.com/en-us/azure/data-api-builder/rest#first-and-after">The query parameter $first allows to limit the number of items returned.</param>
    /// <returns>List of items and NextLink can be used to get the next set of items via the $after query parameter</returns>
    public async Task<(IEnumerable<T> Items, string NextLink)> GetAsync(
        string? select = null, string? filter = null, string? orderby = null, int? first = null, int? after = null)
    {
        var url = CombineQuerystring();

        Trace.WriteLine($"{nameof(GetAsync)} URL:{url}");

        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();
        return (result?.Values!, result?.ContinuationUrl!);

        string CombineQuerystring()
        {
            var builder = new UriBuilder(baseUri);
            var query = HttpUtility.ParseQueryString(builder.Query);
            if (!string.IsNullOrEmpty(select))
            {
                query["$select"] = select;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                query["$filter"] = filter;
            }

            if (!string.IsNullOrEmpty(orderby))
            {
                query["$orderby"] = orderby;
            }

            if (first.HasValue)
            {
                query["$first"] = first.Value.ToString();
            }

            if (after.HasValue)
            {
                query["$after"] = after.Value.ToString();
            }

            builder.Query = query.ToString();
            return builder.Uri.ToString();
        }
    }

    /// <summary>
    /// Get a single record
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<T?> GetAsync(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var url = model.ToUriWithKeys(baseUri);
        Trace.WriteLine($"{nameof(GetAsync)} URL:{url}");

        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();

        if (result is null)
        {
            throw new Exception("Response content is invalid.");
        }

        return result!.Values.SingleOrDefault();
    }

    /// <summary>
    /// Inserts the provided record
    /// </summary>
    /// <param name="model"></param>
    /// <param name="propertiesToRemove">(optional) For models without keys, manually supply the keys to be removed from the model</param>
    /// <returns>Returns the inserted record</returns>
    public async Task<T> InsertAsync(T model, params string[] propertiesToRemove)
    {
        ArgumentNullException.ThrowIfNull(model);

        Trace.WriteLine($"{nameof(InsertAsync)} URL:{baseUri}");

        var clone = Clone(model!, removeKeys: true, removeReadOnly: true, removeOthers: propertiesToRemove);
        var content = clone.ToStringContent();
        var response = await httpClient.PostAsync(baseUri, content);

        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();

        if (result is null)
        {
            throw new Exception("Response content is invalid.");
        }

        if (!result.Values.Any())
        {
            throw new Exception("Response returned no record.");
        }

        if (result.Values.Count() > 1)
        {
            throw new Exception("Response returned more than one record.");
        }

        return result!.Values.Single();
    }

    /// <summary>
    /// For models without keys, manually supply the key/value pair(s)
    /// </summary>
    /// <param name="model">The model with key and readonly properties removed.</param>
    /// <param name="keys">The key/value pair of key(s)</param>
    /// <returns></returns>
    public async Task<T> UpdateAsync(T model, string[] keys, params string[] updateOnlyThese)
    {
        ArgumentNullException.ThrowIfNull(model);

        var props = model.GetProperties().Where(p => keys.Contains(p.Name));
        var url = model.ToUriWithKeys(baseUri, props);
        return await InternalUpdateAsync(model, url, updateOnlyThese);
    }

    /// <summary>
    /// Updated the provided model
    /// </summary>
    /// <param name="model">The model to update, including keys</param>
    /// <returns>The updated model</returns>
    public async Task<T> UpdateAsync(T model, params string[] updateOnlyThese)
    {
        ArgumentNullException.ThrowIfNull(model);

        var url = model.ToUriWithKeys(baseUri);
        return await InternalUpdateAsync(model, url, updateOnlyThese);
    }

    private async Task<T> InternalUpdateAsync(T? model, Uri url, params string[] updateOnlyThese)
    {
        ArgumentNullException.ThrowIfNull(model);

        Trace.WriteLine($"{nameof(UpdateAsync)} URL:{url}");

        List<PropertyInfo> remove = new();
        if (updateOnlyThese.Any())
        {
            var keys = model.GetKeyProperties();
            var onlyThese = model.GetProperties(updateOnlyThese);
            var custom = model.GetProperties().Where(p => !onlyThese.Contains(p) && !keys.Contains(p));
            remove.AddRange(custom);
        }

        var clone = Clone(model, removeKeys: true, removeReadOnly: true, removeOthers: remove.Select(x => x.Name).ToArray());
        var content = clone.ToStringContent();
        var response = await httpClient.PatchAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();

        if (result is null)
        {
            throw new Exception("Response content is invalid.");
        }

        if (!result.Values.Any())
        {
            throw new Exception("Response returned no record.");
        }

        if (result.Values.Count() > 1)
        {
            throw new Exception("Response returned more than one record.");
        }

        return result!.Values.Single();
    }

    /// <summary>
    /// For models without keys, manually supply the key/value pair(s)
    /// </summary>
    /// <param name="model">The model with key and readonly properties removed.</param>
    /// <param name="keys">The key/value pair of key(s)</param>
    /// <returns></returns>
    public async Task DeleteAsync(T model, params string[] keys)
    {
        ArgumentNullException.ThrowIfNull(model);

        var keyProps = model.GetProperties(keys);
        var url = model.ToUriWithKeys(baseUri, keyProps);
        await InternalDeleteAsync(model, url);
    }

    /// <summary>
    /// For models with proper data attribution of properties with [Key] and [ComputerGenerated]
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task DeleteAsync(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        await InternalDeleteAsync(model, model.ToUriWithKeys(baseUri));
    }

    private async Task InternalDeleteAsync(T model, Uri url)
    {
        ArgumentNullException.ThrowIfNull(model);

        Trace.WriteLine($"{nameof(DeleteAsync)} URL:{url}");

        var response = await httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();
    }

    private Dictionary<string, JsonElement> Clone(object model, bool removeKeys, bool removeReadOnly, params string[] removeOthers)
    {
        var remove = new List<PropertyInfo>();

        if (removeReadOnly)
        {
            remove.AddRange(model.GetReadOnlyProperties());
        }

        if (removeOthers.Any())
        {
            remove.AddRange(model.GetProperties(removeOthers));
        }

        if (removeKeys)
        {
            remove.AddRange(model.GetKeyProperties());
        }
        else
        {
            var keys = model.GetKeyProperties();
            remove.RemoveAll(x => keys.Contains(x));
        }

        return model.CloneWithout(remove);
    }
}