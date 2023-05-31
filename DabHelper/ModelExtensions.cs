using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DabHelper.Library
{
    public static class ModelExtensions
    {
        public static Uri ToUriWithKeys(this object model, string baseUri, IEnumerable<PropertyInfo>? keys = null)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNullOrEmpty(baseUri);

            if (keys is null)
            {
                keys = model.GetKeyProperties(throwWhenEmpty: true);
            }

            if (!keys.Any())
            {
                throw new ArgumentException("At least one key is required.", nameof(keys));
            }

            if (model.GetProperties().Where(x => keys.ToArray().Contains(x)).Count() != keys.Count())
            {
                throw new ArgumentException("Not all keys properties found in model.", nameof(keys));
            }

            var parts = keys.Select(x => $"/{x.Name}/{x.GetValue(model)?.ToString()}");
            var url = baseUri + string.Join(string.Empty, parts);
            return new Uri(url);
        }

        public static IEnumerable<PropertyInfo> GetKeyProperties(this object model, bool throwWhenEmpty = false)
        {
            ArgumentNullException.ThrowIfNull(model);

            var props = model.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(KeyAttribute)));

            if (!props.Any())
            {
                throw new ArgumentException("Model has no defined keys.", nameof(model));
            }

            if (props.Any(x => string.IsNullOrEmpty(x.GetValue(model)?.ToString())))
            {
                throw new ArgumentException($"All keys must have values in: {model.GetType()}", nameof(model));
            }

            return props;
        }

        public static IEnumerable<PropertyInfo> GetReadOnlyProperties(this object model)
        {
            ArgumentNullException.ThrowIfNull(model);

            return model.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(ReadOnlyAttribute)));
        }

        public static IEnumerable<PropertyInfo> GetProperties(this object model, params string[] propNames)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(propNames);

            return model.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !propNames.Any() || propNames.Contains(p.Name));
        }

        public static StringContent ToStringContent(this object model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var jsonDictionary = model.Clone();

            var json = JsonSerializer.Serialize(jsonDictionary);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public static Dictionary<string, JsonElement> CloneWithout(this object model, IEnumerable<PropertyInfo> removeThese)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(removeThese);

            var jsonDictionary = Clone(model);

            foreach (var key in removeThese)
            {
                var jsonAttribute = key.GetCustomAttribute<JsonPropertyNameAttribute>();
                jsonDictionary.Remove(jsonAttribute?.Name ?? key.Name);
            }

            return jsonDictionary;
        }

        public static Dictionary<string, JsonElement> CloneWithout(this object model, IEnumerable<string> propNames)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(propNames);

            var jsonDictionary = Clone(model);

            var removeThese = model.GetType().GetProperties().Where(x => !propNames.Contains(x.Name));
            if (!removeThese.Any())
            {
                throw new Exception("Cannot CloneModel() when no matching properties are found.");
            }

            return CloneWithout(model, removeThese);
        }

        public static Dictionary<string, JsonElement> CloneWith(this object model, IEnumerable<string> propNames)
        {
            ArgumentNullException.ThrowIfNull(model);
            ArgumentNullException.ThrowIfNull(propNames);

            var jsonDictionary = Clone(model);

            var removeThese = model.GetType().GetProperties().Where(x => !propNames.Contains(x.Name));
            if (!removeThese.Any())
            {
                throw new Exception("Cannot CloneModel() when no matching properties are found.");
            }

            return CloneWithout(model, removeThese);
        }

        private static Dictionary<string, JsonElement> Clone(this object model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var jsonString = JsonSerializer.Serialize(model);
            var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)!;

            return jsonDictionary;
        }
    }
}
