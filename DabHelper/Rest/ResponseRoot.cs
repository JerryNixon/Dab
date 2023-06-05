// learn more at https://aka.ms/dab

using System;
using System.Text.Json.Serialization;

namespace DabHelper.Rest;

public class ResponseRoot<T>
{
    [JsonPropertyName("value")]
    public IEnumerable<T> Values { get; set; } = default!;

    [JsonPropertyName("nextLink")]
    public string? ContinuationUrl { get; set; }
}
