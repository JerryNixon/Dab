// more info at https://aka.ms/dab

using System;
using System.Diagnostics;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DabHelper.Models
{
    public abstract class Poco { /* TODO */ }
}

namespace DabHelper.Models.dbo
{
    [DebuggerDisplay("dbo.Customers (Id = {Id}) [Table]")]
    public class Customers : Poco 
    {
        [Key]
        [ReadOnly(true)]
        public int Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string City { get; set; } = default!;

        public string State { get; set; } = default!;

        [JsonPropertyName("Specialrank")]
        public decimal SpecialRank { get; set; } = default!;

        [ReadOnly(true)]
        public string Location { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Lines (Id = {Id}) [Table]")]
    public class Lines : Poco 
    {
        [Key]
        public int Id { get; set; } = default!;

        public int Quantity { get; set; } = default!;

        [JsonPropertyName("Productid")]
        public int ProductId { get; set; } = default!;

        [JsonPropertyName("Orderid")]
        public int OrderId { get; set; } = default!;

        [JsonPropertyName("Priceeach")]
        public double PriceEach { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Orders (Id = {Id}) [Table]")]
    public class Orders : Poco 
    {
        [Key]
        public int Id { get; set; } = default!;

        public DateTime? Date { get; set; } = default!;

        [JsonPropertyName("Customerid")]
        public int CustomerId { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.PageCustomers (Id = {Id}) [Procedure]")]
    public class PageCustomers : Poco 
    {
        [Key]
        [ReadOnly(true)]
        public int Id { get; set; } = default!;

        [ReadOnly(true)]
        public string Name { get; set; } = default!;

        [ReadOnly(true)]
        public string City { get; set; } = default!;

        [ReadOnly(true)]
        public string State { get; set; } = default!;

        [JsonIgnore]
        public ParameterInfo Parameters { get; set; } = new();
        public class ParameterInfo
        {
            public int? StartIndex { get; set; } = default!;
            public int? PageSize { get; set; } = default!;
        }
    }

    [DebuggerDisplay("dbo.Products (Id = {Id}) [Table]")]
    public class Products : Poco 
    {
        [Key]
        public int Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public double Price { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Sample (Id1 = {Id1}, Id2 = {Id2}, Id3 = {Id3}) [Table]")]
    public class Sample : Poco 
    {
        [Key]
        public int Id1 { get; set; } = default!;

        [Key]
        public int Id2 { get; set; } = default!;

        [Key]
        public int Id3 { get; set; } = default!;

        public string Name { get; set; } = default!;

        [JsonPropertyName("Weirdname")]
        public string WeirdName { get; set; } = default!;

        [JsonPropertyName("Superweirdnamehere")]
        public string SuperWeirdNameHere { get; set; } = default!;

        [JsonPropertyName("Verystrangelikename")]
        public string VeryStrangeLikeName { get; set; } = default!;

        public int? Lowercase { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("Uppername")]
        public string UpperName { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.SampleEdgeTwo (Id = {Id}) [Table]")]
    public class SampleEdgeTwo : Poco 
    {
        [Key]
        public string Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.SampleNode (Id = {Id}) [Table]")]
    public class SampleNode : Poco 
    {
        [Key]
        public int Id { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.Top10Customers (Id = {Id}) [Procedure]")]
    public class Top10Customers : Poco 
    {
        [Key]
        [ReadOnly(true)]
        public int Id { get; set; } = default!;

        [ReadOnly(true)]
        public string Name { get; set; } = default!;

        [ReadOnly(true)]
        public string City { get; set; } = default!;

        [ReadOnly(true)]
        public string State { get; set; } = default!;

        [ReadOnly(true)]
        public string Location { get; set; } = default!;

        [ReadOnly(true)]
        [JsonPropertyName("Specialrank")]
        public decimal SpecialRank { get; set; } = default!;

    }

    [DebuggerDisplay("dbo.vCustomers (Id = {Id}) [View]")]
    public class VCustomers : Poco 
    {
        [Key]
        public int Id { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string City { get; set; } = default!;

        public string State { get; set; } = default!;

        public string Location { get; set; } = default!;

        [JsonPropertyName("Specialrank")]
        public decimal SpecialRank { get; set; } = default!;

        [JsonPropertyName("Uppername")]
        public string UpperName { get; set; } = default!;

    }

}