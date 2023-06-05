using DabHelper;

using DabHelper.Models;

using DabHelper.Models.dbo;

using Xunit.Abstractions;
using DabHelper.Rest;

namespace DabHelper.Tests
{
    public class CustomerTests : Xunit.IAsyncLifetime
    {
        private readonly string name = Guid.NewGuid().ToString();
        private readonly ApiWrapper<Customers> helper = default!;
        private readonly ITestOutputHelper output;

        public CustomerTests(ITestOutputHelper output)
        {
            helper = new ApiWrapper<Customers>(TestSettings.URL);
            this.output = output;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            var result = await helper.GetAsync(filter: $"Name eq '{name}'");
            foreach (var item in result.Items)
            {
                await helper.DeleteAsync(item);
                output.WriteLine($"Cleaned up item id: {item.Id}");
            }
        }

        Customers Model => new Customers
        {
            Name = name,
            City = name,
            State = name,
            SpecialRank = 123,
        };

        [Fact]
        public async Task Insert_Simple()
        {
            var model = Model;
            var inserted = await helper.InsertAsync(model);
            output.WriteLine($"Inserted item id: {inserted.Id}");

            Assert.Equal(model.Name, inserted.Name);
            Assert.Equal(model.City, inserted.City);
            Assert.Equal(model.State, inserted.State);
            Assert.Equal(model.SpecialRank, inserted.SpecialRank);
            Assert.NotEqual(default(int), inserted.Id);
        }

        [Fact]
        public async Task Insert_RequireNotNullProperties()
        {
            var model = Model;
            var result = await Assert.ThrowsAsync<HttpRequestException>(async () => await helper.InsertAsync(model, "City"));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Insert_IgnoreSomeProperties()
        {
            var model = Model;
            var inserted = await helper.InsertAsync(model, "State", "SpecialRank");
            output.WriteLine($"Inserted item id: {inserted.Id}");

            Assert.Equal("CO", inserted.State);
            Assert.Equal(default(decimal), inserted.SpecialRank);
            Assert.NotEqual(default(int), inserted.Id);
        }

        [Fact]
        public async Task Update()
        {
            var model = Model;
            var inserted = await helper.InsertAsync(model);
            output.WriteLine($"Inserted item id: {inserted.Id}");

            inserted.City = Guid.NewGuid().ToString();
            inserted.State = Guid.NewGuid().ToString();

            var updated = await helper.UpdateAsync(inserted);
            output.WriteLine($"Updated item id: {inserted.Id}");

            Assert.Equal(inserted.City, updated.City);
            Assert.Equal(inserted.State, updated.State);
        }

        [Fact]
        public async Task Update_SomeProperties()
        {
            var model = Model;
            var inserted = await helper.InsertAsync(model);
            output.WriteLine($"Inserted item id: {inserted.Id}");

            inserted.Name = "NameToIgnore";
            inserted.City = Guid.NewGuid().ToString();
            inserted.State = Guid.NewGuid().ToString();
            inserted.SpecialRank = 8675309;

            var updated = await helper.UpdateAsync(inserted, "City", "State");
            output.WriteLine($"Updated item id: {inserted.Id}");

            Assert.Equal(inserted.City, updated.City);
            Assert.Equal(inserted.State, updated.State);

            Assert.NotEqual(inserted.Name, updated.Name);
            Assert.NotEqual(inserted.SpecialRank, updated.SpecialRank);
        }

        [Fact]
        public async Task Get_OneModel()
        {
            var model = Model;
            var inserted = await helper.InsertAsync(model);
            output.WriteLine($"Inserted item id: {inserted.Id}");

            var result = await helper.GetAsync(inserted);

            Assert.Equivalent(inserted, result);
        }

        [Fact]
        public async Task Get_InvalidModel()
        {
            var result = await helper.GetAsync(Model);
            Assert.Null(result);
        }

        [Fact]
        public async Task Get_QueryMany()
        {
            var model = Model;
            await helper.InsertAsync(model);
            await helper.InsertAsync(model);

            var results = await helper.GetAsync(filter: $"Name eq '{model.Name}'");

            Assert.Equal(2, results.Items.Count());
        }
    }
}