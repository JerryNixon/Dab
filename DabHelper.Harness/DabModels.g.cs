// more info at https://aka.ms/dab

using System;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public abstract class Poco { /* TODO */ }
}

namespace Models.dbo
{
    [DebuggerDisplay("dbo.PageCustomers (PageSize = {Parameters.PageSize}, StartIndex = {Parameters.StartIndex}) [procedure]")]
    public class @PageCustomers : Poco 
    {
        [ReadOnly(true)]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("city")]
        public string @City { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("state")]
        public string @State { get; set; } = default!;

        public ParameterInfo Parameters { get; set; } = new();
        public class ParameterInfo
        {
            public int? @PageSize { get; set; } = default!;
            public int? @StartIndex { get; set; } = default!;
        }
    }

    [DebuggerDisplay("dbo.Top10Customers ( = {Parameters.}) [procedure]")]
    public class @Top10Customers : Poco 
    {
        [ReadOnly(true)]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("city")]
        public string @City { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("state")]
        public string @State { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("location")]
        public string @Location { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("special-rank")]
        public decimal @SpecialRank { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Customers (Id = {Id},  = {Parameters.}) [table]")]
    public class @Customers : Poco 
    {
        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [JsonPropertyName("city")]
        public string @City { get; set; } = default!;

        [JsonPropertyName("state")]
        public string @State { get; set; } = default!;

        [JsonPropertyName("special-rank")]
        public decimal @SpecialRank { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("location")]
        public string @Location { get; set; } = default!;

        [Key]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Lines (Id = {Id},  = {Parameters.}) [table]")]
    public class @Lines : Poco 
    {
        [JsonPropertyName("quantity")]
        public int @Quantity { get; set; } = default!;

        [JsonPropertyName("product-id")]
        public int @ProductId { get; set; } = default!;

        [JsonPropertyName("order-id")]
        public int @OrderId { get; set; } = default!;

        [JsonPropertyName("price-each")]
        public double @PriceEach { get; set; } = default!;

        [Key]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Orders (Id = {Id},  = {Parameters.}) [table]")]
    public class @Orders : Poco 
    {
        [JsonPropertyName("date")]
        public DateTime? @Date { get; set; } = default!;

        [JsonPropertyName("customer-id")]
        public int @CustomerId { get; set; } = default!;

        [Key]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Products (Id = {Id},  = {Parameters.}) [table]")]
    public class @Products : Poco 
    {
        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [JsonPropertyName("price")]
        public double @Price { get; set; } = default!;

        [Key]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Sample (Id1 = {Id1}, Id2 = {Id2}, Id3 = {Id3},  = {Parameters.}) [table]")]
    public class @Sample : Poco 
    {
        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [JsonPropertyName("weird-name")]
        public string @WeirdName { get; set; } = default!;

        [JsonPropertyName("super-weird-name-here")]
        public string @SuperWeirdNameHere { get; set; } = default!;

        [JsonPropertyName("very-strange-like-name")]
        public string @VeryStrangeLikeName { get; set; } = default!;

        [JsonPropertyName("lowercase")]
        public int? @Lowercase { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("upper-name")]
        public string @UpperName { get; set; } = default!;

        [Key]
        [JsonPropertyName("id1")]
        public int @Id1 { get; set; } = default!;

        [Key]
        [JsonPropertyName("id2")]
        public int @Id2 { get; set; } = default!;

        [Key]
        [JsonPropertyName("id3")]
        public int @Id3 { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.SampleEdgeTwo ( = {Parameters.}) [table]")]
    public class @SampleEdgeTwo : Poco 
    {
        [JsonPropertyName("id")]
        public string @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.SampleNode (Id = {Id},  = {Parameters.}) [table]")]
    public class @SampleNode : Poco 
    {
        [Key]
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.vCustomers ( = {Parameters.}) [view]")]
    public class @VCustomers : Poco 
    {
        [JsonPropertyName("id")]
        public int @Id { get; set; } = default!;

        [JsonPropertyName("name")]
        public string @Name { get; set; } = default!;

        [JsonPropertyName("city")]
        public string @City { get; set; } = default!;

        [JsonPropertyName("state")]
        public string @State { get; set; } = default!;

        [JsonPropertyName("location")]
        public string @Location { get; set; } = default!;

        [JsonPropertyName("special-rank")]
        public decimal @SpecialRank { get; set; } = default!;

        [JsonPropertyName("upper-name")]
        public string @UpperName { get; set; } = default!;

    }

}