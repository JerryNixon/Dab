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

namespace DabHelpers;

public partial class RestHelper<T> where T : new()
{
    private readonly string baseUri;
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

        var url = AssembleUrl(model);
        Trace.WriteLine($"{nameof(GetAsync)} URL:{url}");

        var response = await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();
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
        EnsureValidModel(model);
        Trace.WriteLine($"{nameof(InsertAsync)} URL:{baseUri}");

        var json = Clone(model, removeKeys: true, removeReadOnlyProperties: true, otherPropertiesToRemove: propertiesToRemove);
        var response = await httpClient.PostAsync(baseUri, json);
        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();
        return result!.Values.Single();
    }

    /// <summary>
    /// For models without keys, manually supply the key/value pair(s)
    /// </summary>
    /// <param name="model">The model with key and readonly properties removed.</param>
    /// <param name="keys">The key/value pair of key(s)</param>
    /// <returns></returns>
    public async Task<T> UpdateAsync(T model, params string[] keys)
    {
        var keyProps = typeof(T)
           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
           .Where(p => keys.Contains(p.Name))
           .Select(x => (x.Name, Value: x.GetValue(model)?.ToString() ?? string.Empty));

        if (keyProps.Count() != keys.Length)
        {
            throw new ArgumentException("Not all keys properties found in model.", nameof(keys));
        }

        var parts = keyProps.Select(x => $"/{x.Name}/{x.Value}");
        var url = baseUri + string.Join(string.Empty, parts);

        return await InternalUpdateAsync(model, url);
    }

    /// <summary>
    /// Updated the provided model
    /// </summary>
    /// <param name="model">The model to update, including keys</param>
    /// <returns>The updated model</returns>
    public async Task<T> UpdateAsync(T model)
    {
        return await InternalUpdateAsync(model, AssembleUrl(model));
    }

    private async Task<T> InternalUpdateAsync(T model, string url)
    {
        ArgumentNullException.ThrowIfNull(model);

        Trace.WriteLine($"{nameof(UpdateAsync)} URL:{url}");

        var json = Clone(model);
        var response = await httpClient.PatchAsync(url, json);
        if (!response.IsSuccessStatusCode)
        {
            Debugger.Break();
        }

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<RestRoot<T>>();
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
        var keyProps = typeof(T)
           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
           .Where(p => keys.Contains(p.Name))
           .Select(x => (x.Name, Value: x.GetValue(model)?.ToString() ?? string.Empty));

        if (keyProps.Count() != keys.Length)
        {
            throw new ArgumentException("Not all keys properties found in model.", nameof(keys));
        }

        var parts = keyProps.Select(x => $"/{x.Name}/{x.Value}");
        var url = baseUri + string.Join(string.Empty, parts);

        await InternalDeleteAsync(model, url);
    }

    /// <summary>
    /// For models with proper data attribution of properties with [Key] and [ComputerGenerated]
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task DeleteAsync(T model)
    {
        await InternalDeleteAsync(model, AssembleUrl(model));
    }

    private async Task InternalDeleteAsync(T model, string url)
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

    private IEnumerable<(string Name, string Value)> GetKeyPropertiesWithValues(T model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)))
            .Select(x => (x.Name, x.GetValue(model)?.ToString() ?? string.Empty));
    }

    private StringContent Clone(T model, bool removeKeys = true, bool removeReadOnlyProperties = true, string[]? otherPropertiesToRemove = null)
    {
        ArgumentNullException.ThrowIfNull(model);

        var jsonString = JsonSerializer.Serialize(model);
        var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)!;

        if (removeKeys)
        {
            var keys = KeyProperties();
            foreach (var key in keys)
            {
                var jsonAttribute = key.GetCustomAttribute<JsonPropertyNameAttribute>();
                jsonDictionary.Remove(jsonAttribute?.Name ?? key.Name);
            }
        }

        if (removeReadOnlyProperties)
        {
            var roProps = ReadOnlyProperties();
            foreach (var prop in roProps)
            {
                var jsonAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                jsonDictionary.Remove(jsonAttribute?.Name ?? prop.Name);
            }
        }

        if (otherPropertiesToRemove is not null && otherPropertiesToRemove.Any())
        {
            foreach (var other in otherPropertiesToRemove)
            {
                var prop = model.GetType().GetProperty(other);
                if (prop is not null)
                {
                    var jsonAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
                    jsonDictionary.Remove(jsonAttribute?.Name ?? prop.Name);
                }
            }
        }

        var json = JsonSerializer.Serialize(jsonDictionary);
        return new StringContent(json, Encoding.UTF8, "application/json");

        IEnumerable<PropertyInfo> ReadOnlyProperties()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => Attribute.IsDefined(p, typeof(ReadOnlyAttribute)));
        }

        IEnumerable<PropertyInfo> KeyProperties()
        {
            return typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)));
        }
    }

    private IEnumerable<(string Name, string Value)> EnsureValidModel(T model)
    {
        ArgumentNullException.ThrowIfNull(nameof(model));

        var keys = GetKeyPropertiesWithValues(model);

        if (!keys.Any())
        {
            throw new ArgumentException("At least one key is required.");
        }

        if (keys.Any(x => string.IsNullOrEmpty(x.Value)))
        {
            throw new ArgumentException($"All keys must have values in: {typeof(T)}", nameof(model));
        }

        return keys;
    }

    private string AssembleUrl(T model)
    {
        var keys = EnsureValidModel(model);

        var builder = new StringBuilder(baseUri);

        foreach (var key in keys)
        {
            var name = HttpUtility.UrlEncode(key.Name);
            var value = HttpUtility.UrlEncode(key.Value.ToString());
            builder.Append($"/{name}/{value}");
        }

        return builder.ToString();
    }
}