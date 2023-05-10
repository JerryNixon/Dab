using Dac2Poco;
using Dac2Poco.Tables;

using System.Text;

namespace Dac2Poco.Tables;

public static class Generator
{
    public static void Generate(TableInfo table, StringBuilder code, bool attributes, bool methods, string? baseName)
    {
        if (attributes)
        {
            if (table.Columns.Any(x => x.IsPrimaryKey))
            {
                var keysNameValue = string.Join(", ", table.Columns.Where(x => x.IsPrimaryKey).Select(x => $"{x.Name} = {{{x.Name}}}"));
                code.AppendLine($"    [DebuggerDisplay(\"{table.Schema}.{table.Name} ({keysNameValue})\")]");
            }
            else
            {
                code.AppendLine($"    [Keyless]");
                code.AppendLine($"    [DebuggerDisplay(\"{table.Schema}.{table.Name}\")]");
            }
        }

        var baseText = (baseName is not null) ? $" : {baseName}" : string.Empty;
        code.AppendLine($"    public partial class {Utilities.ToPascalCase(table.Name)}{baseText}");
        code.AppendLine("    {");

        foreach (var column in table.Columns.Where(x => !x.IsGraph))
        {
            var netTypeText = Utilities.GetDotnetType(column.SqlType, column.IsNullable);
            if (string.IsNullOrEmpty(netTypeText) && !column.IsComputed)
            {
                continue;
            }

            if (attributes && column != table.Columns.First())
            {
                code.AppendLine();
            }

            var propName = Utilities.ToPascalCase(column.Name);
            if (attributes && propName != column.Name)
            {
                code.AppendLine($"        [JsonPropertyName(\"{column.Name}\")]");
            }

            var inlineType = column.IsComputed ? "string" : netTypeText;
            var netType = Type.GetType("System." + netTypeText.Trim('?'));
            var def = column.IsNullable ? "default!" : netType == typeof(System.String) ? "string.Empty" : "default";
            code.AppendLine($"        public {inlineType} @{propName} {{ get; set; }} = {def};");
        }

        var keys = string.Join("", table.Columns.Where(x => x.IsPrimaryKey).Select(x => $"/{x.Name}/{{@{Utilities.ToPascalCase(x.Name)}}}"));
        var remove = table.Columns.Where(x => x.IsPrimaryKey || x.IsComputed).Select(x => $"jsonDictionary.Remove(\"{x.Name}\");");
        code.AppendLine($$"""

                    public (string Url, string Json) ToJson()
                    {
                        var jsonString = JsonSerializer.Serialize(this);
                        var jsonDictionary = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonString)!;
                        {{string.Join("\r\n            ", remove)}}
                        return ($"{{keys}}", JsonSerializer.Serialize(jsonDictionary));
                    }
            """);
        code.AppendLine("    }");
    }
}
