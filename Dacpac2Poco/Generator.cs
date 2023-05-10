using Dac2Poco;
using Dac2Poco.Tables;
using Dac2Poco.Views;

using System.Data.Common;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;

public class Generator
{
    private readonly TableInfo[] tables;
    private readonly ViewInfo[] views;

    public Generator(TableInfo[] tables, ViewInfo[] views)
    {
        this.tables = tables;
        this.views = views;
    }

    public string Generate(string? baseName = null, bool attributes = true, bool methods = true)
    {
        var schemas = tables.Select(x => x.Schema).Union(views.Select(x => x.Schema)).Distinct();
        var code = new StringBuilder();

        code.AppendLine("""
            // learn more at https://aka.ms/dab

            using System.Diagnostics;
            using System.Text.Json.Serialization;
            using System.Text.Json;
            """);

        if (!string.IsNullOrEmpty(baseName))
        {
            code.AppendLine($$"""
                using Models;

                namespace Models
                {
                    public abstract class {{baseName}} { /* TODO */ }
                }

                """);
        }

        foreach (var schema in schemas)
        {
            if (schema != schemas.First())
            {
                code.AppendLine();
            }

            code.AppendLine($"namespace Models.{schema}");
            code.AppendLine("{");

            var schemaTables = tables.Where(x => x.Schema == schema).Where(x => !x.IsGraph);
            var schemaViews = views.Where(x => x.Schema == schema);

            foreach (var table in schemaTables)
            {
                if (table != schemaTables.First())
                {
                    code.AppendLine();
                }

                GenerateTable(table, code, attributes, methods, baseName);
            }

            if (schemaTables.Any() && schemaViews.Any())
            {
                code.AppendLine();
            }

            foreach (var view in schemaViews)
            {
                if (view != schemaViews.First())
                {
                    code.AppendLine();
                }

                GenerateView(view, code, attributes, methods, baseName);
            }
        }
        code.AppendLine("}");

        return code.ToString().Trim();
    }

    private void GenerateTable(TableInfo table, StringBuilder code, bool attributes, bool methods, string? baseName)
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
        code.AppendLine($"    public partial class {ToPascalCase(table.Name)}{baseText}");
        code.AppendLine("    {");

        foreach (var column in table.Columns.Where(x => !x.IsGraph))
        {
            var netTypeText = SqlUtilities.GetDotnetType(column.SqlType, column.IsNullable);
            if (string.IsNullOrEmpty(netTypeText) && !column.IsComputed)
            {
                continue;
            }

            if (attributes && column != table.Columns.First())
            {
                code.AppendLine();
            }

            var propName = ToPascalCase(column.Name);
            if (attributes && propName != column.Name)
            {
                code.AppendLine($"        [JsonPropertyName(\"{column.Name}\")]");
            }

            var inlineType = column.IsComputed ? "string" : netTypeText;
            var netType = Type.GetType("System." + netTypeText.Trim('?'));
            var def = column.IsNullable ? "default!" : netType == typeof(System.String) ? "string.Empty" : "default";
            code.AppendLine($"        public {inlineType} @{propName} {{ get; set; }} = {def};");
        }

        var keys = string.Join("", table.Columns.Where(x => x.IsPrimaryKey).Select(x => $"/{x.Name}/{{@{ToPascalCase(x.Name)}}}"));
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

    private void GenerateView(ViewInfo view, StringBuilder code, bool attributes, bool methods, string? baseName)
    {
        if (attributes)
        {
            // code.AppendLine($"    [Keyless] // View");
            code.AppendLine($"    [DebuggerDisplay(\"{view.Schema}.{view.Name}\")]");
        }

        var baseText = (baseName is not null) ? $" : {baseName}" : string.Empty;
        code.AppendLine($"    public partial class {ToPascalCase(view.Name)}{baseText}");
        code.AppendLine("    {");

        foreach (var column in view.Columns)
        {
            if (attributes && column != view.Columns.First())
            {
                code.AppendLine();
            }

            var propName = ToPascalCase(column.Name);
            if (attributes && propName != column.Name)
            {
                code.AppendLine($"        [JsonPropertyName(\"{column.Name}\")]");
            }

            // the dacpac does not know the data inlineType of view columns
            var netType = column.SqlType;
            code.AppendLine($"        public {netType} @{propName} {{ get; set; }} = default!;");
        }
        code.AppendLine("    }");
    }

    public static string ToPascalCase(string input)
    {
        string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
        {
            string word = words[i];
            if (word.Length > 0)
            {
                words[i] = char.ToUpper(word[0]) + word.Substring(1);
            }
        }

        return string.Join("", words);
    }
}