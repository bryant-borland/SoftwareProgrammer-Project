using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SimpleProductAPI.Data;
using SimpleProductAPI.Models;
using SimpleProductAPI.Repositories;
using Xunit;

namespace SimpleProductAPI.Tests.Repositories
{
    public class ProductRepositoryTests
    {
        private static ProductDbContext BuildContext(string dbName, IEnumerable<Product>? seed = null)
        {
            var opts = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            var ctx = new ProductDbContext(opts);
            if (seed != null)
            {
                ctx.Products.AddRange(seed);
                ctx.SaveChanges();
            }
            return ctx;
        }

        [Fact]
        public async Task GetAllAsync_returns_all_products()
        {
            // Arrange
            var seed = new[]
            {
                new Product { Id = 1, Name = "Laptop",     Price = 999.99m },
                new Product { Id = 2, Name = "Headphones", Price = 199.50m },
                new Product { Id = 3, Name = "Mouse",      Price = 49.99m }
            };
            using var ctx = BuildContext(nameof(GetAllAsync_returns_all_products), seed);
            var repo = new ProductRepository(ctx);

            // Act
            var result = await repo.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().HaveCount(3);
            result.Should().ContainSingle(p => p.Id == 1 && p.Name == "Laptop");
        }

        [Fact]
        public async Task GetPagedAsync_returns_slice_and_total()
        {
            // Arrange
            var seed = new List<Product>();
            for (int i = 1; i <= 7; i++)
                seed.Add(new Product { Id = i, Name = $"P{i}", Price = i });

            using var ctx = BuildContext(nameof(GetPagedAsync_returns_slice_and_total), seed);
            var repo = new ProductRepository(ctx);

            // Act
            var (items, total) = await repo.GetPagedAsync(page: 2, pageSize: 3, CancellationToken.None);

            // Assert
            total.Should().Be(7);
            items.Should().HaveCount(3);
            items[0].Id.Should().Be(4);
            items[1].Id.Should().Be(5);
            items[2].Id.Should().Be(6);
        }

        [Fact]
        public async Task GetPagedAsync_beyond_last_page_returns_empty_items_with_total()
        {
            // Arrange
            var seed = new[]
            {
                new Product { Id = 1, Name = "A", Price = 1 },
                new Product { Id = 2, Name = "B", Price = 2 }
            };
            using var ctx = BuildContext(nameof(GetPagedAsync_beyond_last_page_returns_empty_items_with_total), seed);
            var repo = new ProductRepository(ctx);

            // Act
            var (items, total) = await repo.GetPagedAsync(page: 5, pageSize: 2, CancellationToken.None);

            // Assert
            total.Should().Be(2);
            items.Should().BeEmpty();
        }
    }
}