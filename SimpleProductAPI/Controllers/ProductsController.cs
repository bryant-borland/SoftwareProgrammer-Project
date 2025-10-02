using Microsoft.AspNetCore.Mvc;
using SimpleProductAPI.Models;
using SimpleProductAPI.Repositories;

namespace SimpleProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // Get /api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var products = await _repo.GetAllAsync();
            return Ok(products); // 200 ok with [] if empty
        }
    }
}
