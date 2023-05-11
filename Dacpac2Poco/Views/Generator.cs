using Dac2Poco.Views;

using System.Text;

namespace Dac2Poco.Views;

public static class Generator
{
    public static void GeneratePoco(ViewInfo view, StringBuilder code, bool attributes, bool methods, string? baseName)
    {
        if (attributes)
        {
            // code.AppendLine($"    [Keyless] // View");
            code.AppendLine($"    [DebuggerDisplay(\"{view.Schema}.{view.Name}\")]");
        }

        var baseText = (baseName is not null) ? $" : {baseName}" : string.Empty;
        code.AppendLine($"    public partial class {Utilities.ToPascalCase(view.Name)}{baseText}");
        code.AppendLine("    {");

        foreach (var column in view.Columns)
        {
            if (attributes && column != view.Columns.First())
            {
                code.AppendLine();
            }

            var propName = Utilities.ToPascalCase(column.Name);
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
}