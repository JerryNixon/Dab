using Dac2Poco.Tables;
using Dac2Poco.Views;

using System.Data.Common;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;

namespace Dac2Poco;

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

                Tables.Generator.Generate(table, code, attributes, methods, baseName);
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

                Views.Generator.Generate(view, code, attributes, methods, baseName);
            }
        }
        code.AppendLine("}");

        return code.ToString().Trim();
    }
}