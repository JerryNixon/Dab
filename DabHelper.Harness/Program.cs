using System.Diagnostics;

using Models.dbo;

using Xunit;

internal class Program
{
    private static async Task Main(string[] args)
    {
        /*
         dotnet tool install --global Microsoft.DataApiBuilder 
         dotnet tool update --global Microsoft.DataApiBuilder 
         dab --version
         dab init --database-type "mssql" --connection-string "Server=(localdb)\MSSQLLocalDB;Database=SampleDb;User ID=DabLogin;Password=P@ssw0rd!;TrustServerCertificate=True;" --host-mode "Development"
         dab add Customers --source dbo.Customers --permissions "anonymous:*"
         dab start
        */

        var h = new DabHelpers.RestHelper<Customers>("http://localhost:5000/api/customers");

        // first, create new

        var insert = new Customers
        {
            Name = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(),
            State = Guid.NewGuid().ToString()
        };
        var inserted = await h.InsertAsync(insert);

        Assert.Equal(insert.Name, inserted.Name);
        Assert.Equal(insert.City, inserted.City);
        Assert.Equal(insert.State, inserted.State);
        Assert.NotEqual(inserted.Id, default);

        // second get that new one

        Customers? getOne = await h.GetAsync(inserted);

        Assert.NotNull(getOne);
        Assert.Equivalent(inserted, getOne);

        // third, get the new one from list

        var getMany1 = await h.GetAsync();
        Customers? test = getMany1.Items.Single(x => x.Id == inserted.Id);
        Assert.Equivalent(inserted, test);

        // third, get the new one from special list

        var getMany2 = await h.GetAsync(select: "Id,City", filter: $"Id eq {inserted.Id}");
        test = getMany2.Items.Single(x => x.Id == inserted.Id);
        Assert.Equal(inserted.Id, test.Id);
        Assert.NotNull(test.City);
        Assert.Null(test.Name);

        // fourth, update same record

        var update = new Customers
        {
            Id = inserted.Id,
            Name = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(),
            State = Guid.NewGuid().ToString(),
        };
        var updated = await h.UpdateAsync(update);
        getOne = await h.GetAsync(updated);

        Assert.NotNull(updated);
        Assert.NotNull(getOne);
        Assert.Equivalent(updated, getOne);

        // fifth, delete that record

        await h.DeleteAsync(updated);
        getOne = await h.GetAsync(updated);

        Assert.Null(getOne);

        // over

        Debugger.Break();
    }
}