// learn more at https://aka.ms/dab

using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public abstract class Poco { /* TODO */ }
}

namespace Models.dbo
{
    [DebuggerDisplay("dbo.Customers (Id = {Id})")]
    public partial class Customers : Poco
    {
        [Key]
        [ReadOnly(true)]
        public Int32 @Id { get; set; } = default;

        public String? @Name { get; set; } = default!;

        public String @City { get; set; } = string.Empty;

        public String @State { get; set; } = string.Empty;

        [ReadOnly(true)]
        public string @Location { get; set; } = default!;

        public Decimal @SpecialRank { get; set; } = default;

        [JsonPropertyName("weirdName")]
        public String? @WeirdName { get; set; } = default!;

        [JsonPropertyName("Very Weird Name")]
        public String? @VeryWeirdName { get; set; } = default!;
    }

    [DebuggerDisplay("dbo.vCustomers")]
    public partial class VCustomers : Poco
    {
        public string @Id { get; set; } = default!;

        public string @Name { get; set; } = default!;

        public string @City { get; set; } = default!;

        public string @State { get; set; } = default!;

        public string @Location { get; set; } = default!;

        public string @SpecialRank { get; set; } = default!;

        [JsonPropertyName("weirdName")]
        public string @WeirdName { get; set; } = default!;

        [JsonPropertyName("Very Weird Name")]
        public string @VeryWeirdName { get; set; } = default!;
    }
}