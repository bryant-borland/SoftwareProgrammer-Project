using Microsoft.AspNetCore.Mvc;
using SimpleProductAPI.Models;
using SimpleProductAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace SimpleProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository repo, ILogger<ProductsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Returns all products.
        /// </summary>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>JSON array of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Product>>> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                // (Repo doesn't accept a token yet; we'll wire that next.)
                var products = await _repo.GetAllAsync(cancellationToken);
                return Ok(products); // 200 OK, [] if none
            }
            catch (OperationCanceledException)
            {
                // Client canceled the request; let the framework turn this into a 499/appropriate end.
                _logger.LogInformation("GET /api/product was canceled by the client.");
                return new StatusCodeResult(StatusCodes.Status499ClientClosedRequest); // optional; omit to let pipelin handle
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GET /api/product");
                // Generic 500 without leaking internals
                return Problem(
                    title: "An unexpected error occurred.",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
