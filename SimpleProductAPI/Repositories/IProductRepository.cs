using SimpleProductAPI.Models;

namespace SimpleProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
