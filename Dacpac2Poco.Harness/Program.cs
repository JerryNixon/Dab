using Dac2Poco;

internal partial class Program
{
    private static void Main(string[] args)
    {
        using var tablesReader = new Dac2Poco.Tables.Reader("sample.dacpac");
        var tables = tablesReader.GetTables().ToArray();

        using var viewsReader = new Dac2Poco.Views.Reader("sample.dacpac");
        var views = viewsReader.GetViews().ToArray();

        // using var procsReader = new Dac2Poco.Procedures.Reader("sample.dacpac");
        // var procs = procsReader.GetProcedures().ToArray();

        var generator = new Generator(tables, views);
        var code = generator.GeneratePoco("Poco", true);
        var config = generator.GenerateConfig();

        var pocoPath = "output.cs";
        File.WriteAllText(pocoPath, code);

        var configPath = "dab-config.json";
        File.WriteAllText(configPath, config);

        try { "sample.dacpac.xml".OpenInVsCode(); } catch { "sample.dacpac.xml".OpenInNotepad(); }
        try { pocoPath.OpenInVsCode(); } catch { pocoPath.OpenInNotepad(); }
        try { configPath.OpenInVsCode(); } catch { configPath.OpenInNotepad(); }
    }
}