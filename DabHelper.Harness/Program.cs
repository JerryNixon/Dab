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

        var h = new DabHelpers.RestHelper<Customers>("https://localhost:5001/api/Customers");

        // first, create new

        var insert = new Customers
        {
            @Name = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(),
            State = Guid.NewGuid().ToString()
        };
        var insertResult = await h.InsertAsync(new[] { insert }).ToArrayAsync();
        var inserted = insertResult.First().Result!;


        Assert.Equal(insert.Name, inserted.Name);
        Assert.Equal(insert.City, inserted.City);
        Assert.Equal(insert.State, inserted.State);
        Assert.NotEqual(inserted.Id, default);

        // second get that new one

        Customers? getOne = await h.GetOneAsync(inserted);

        Assert.NotNull(getOne);
        Assert.Equivalent(inserted, getOne);

        // third, get the new one from list

        var getMany1 = await h.GetManyAsync();
        Customers? test = getMany1.Items.First(x => x.Id == inserted.Id);
        Assert.Equivalent(inserted, test);

        // third, get the new one from special list

        var getMany2 = await h.GetManyAsync(select: "Id", filter: $"Id eq {inserted.Id}");
        test = getMany2.Items.First(x => x.Id == inserted.Id);
        Assert.Equal(inserted.Id, test.Id);
        Assert.Null(test.Name);

        // fourth, update same record

        var update = new Customers
        {
            Id = inserted.Id,
            Name = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(),
            State = Guid.NewGuid().ToString(),
        };
        var updateResult = await h.UpdateAsync(new[] { update }).ToArrayAsync();
        var updated = updateResult.First().Result!;
        getOne = await h.GetOneAsync(updated);

        Assert.Equal(update.Name, updated.Name);
        Assert.Equal(update.City, updated.City);
        Assert.Equal(update.State, updated.State);
        Assert.Equal(update.Id, updated.Id);

        Assert.NotNull(getOne);
        Assert.Equivalent(updated, getOne);

        // fifth, delete that record

        await h.DeleteAsync(new[] { updated }).ToArrayAsync();
        getOne = await h.GetOneAsync(updated);

        Assert.Null(getOne);

        // over

        Debugger.Break();
    }
}