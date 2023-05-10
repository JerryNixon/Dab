namespace Dacpac2Poco.Tests
{
    public class TablesReaderTests
    {
        [Fact]
        public void GetTables_WithOneTable_ReturnsOne()
        {
            using var tablesReader = new Dac2Poco.Tables.Reader("sample.dacpac");
            var tables = tablesReader.GetTables().ToArray();
            Assert.Single(tables);
        }
    }
}