using SimpleProductAPI.Models;

namespace SimpleProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        // NEW: returns items plus the total number of rows in the table
        Task<(List<Product> Items, int Total)> GetPagedAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
    }
}
