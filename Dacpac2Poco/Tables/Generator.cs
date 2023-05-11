using Dac2Poco;
using Dac2Poco.Tables;

using System.Text;
using System.Web;
using System.Xml;

namespace Dac2Poco.Tables;

public static class Generator
{
    public static void GenerateConfig(TableInfo table, StringBuilder code)
    {
        /*
          "Customers": {
              "source": {
                "object": "[dbo].[Customers]",
                "type": "table",
                "key-fields": [ "Id" ],
                "mappings": {
                  "Id": "Id",
                  "Name": "Name"
                 },  
              },
              "rest": true,
              "graphql": false,
              "permissions": [
                {
                  "role": "anonymous",
                  "actions": [ "create", "read" ,"update", "delete" ]
                }
              ]
            },
         */
        /*
         
        dab add Customers 
        --source dbo.Customers 
        --source.type "table" 
        --source.key-fields "Id" 
        --rest customers 
        --graphql false 
        --permissions "anonymous:create,read,update,delete" 
        --fields.include "Id,Name" < Not going to add; too noisy
        --fields.exclude "TimeStamp"  < Not going to add; too noisy
        --map "Id:id,Name:name"
          
         */

        code.Append($"dab add {table.Name}");
        code.Append($" --source [{table.Schema}].[{table.Name}]");
        code.Append($" --source.type \"table\"");

        var keys = string.Join(",", table.Columns.Where(x => x.IsPrimaryKey).Select(x => $"[{x.Name}]"));
        code.Append($" --source.key-fields \"{keys}\"");

        var restName = GetRestName(table.Name);
        code.Append($" --rest \"{restName}\"");

        code.Append($" --graphql false");
        code.Append($" --permissions \"anonymous:create,read,update,delete\"");

        code.AppendLine();

        code.Append($"dab update {table.Name}");
        code.Append($" --source [{table.Schema}].[{table.Name}]");

        var map = string.Join(",", table.Columns.Select(x => $"{x.Name}:{GetJsonName(x.Name)}"));
        code.Append($" --map \"{map}\"");

        code.AppendLine();
    }

    private static string GetJsonName(string name)
    {
        return name.Trim('[', ']', '.').ToLower().Replace(" ", string.Empty);
    }

    private static string GetRestName(string name)
    {
        // TODO: should we include schema in case 2 tables have the same name?
        return HttpUtility.UrlEncode(name.Trim('[', ']', '.').ToLower().Replace(" ", string.Empty));
    }

    public static void GeneratePoco(TableInfo table, StringBuilder code, bool attributes, bool methods, string? baseName)
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

            if (attributes && column.IsPrimaryKey)
            {
                code.AppendLine($"        [Key]");
            }

            if (attributes && (column.IsComputed || column.IsIdentity))
            {
                code.AppendLine($"        [ReadOnly(true)]");
            }

            var propName = Utilities.ToPascalCase(column.Name);
            if (attributes && propName != column.Name)
            {
                code.AppendLine($"        [JsonPropertyName(\"{GetJsonName(column.Name)}\")]");
            }

            var inlineType = column.IsComputed ? "string" : netTypeText;
            var netType = Type.GetType("System." + netTypeText.Trim('?'));
            var def = column.IsNullable ? "default!" : netType == typeof(System.String) ? "string.Empty" : "default";
            code.AppendLine($"        public {inlineType} @{propName} {{ get; set; }} = {def};");
        }
        code.AppendLine("    }");
    }
}
