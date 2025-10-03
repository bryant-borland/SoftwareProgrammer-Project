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
        private const int MaxPageSize = 100;

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
        /// <param name="page">Optional page number (1-based).</param>
        /// <param name="pageSize">Optional page size (max 100)</param>
        /// <param name="cancellationToken">Request cancellation token.</param>
        /// <returns>
        /// When paged: { items, total, page, pageSize, totalPages }.
        /// Otherwise: JSON array of products.
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            try
            {
                // If no pagination params, preserve original behavior: return all items as an array.
                if (page is null && pageSize is null)
                {
                    var all = await _repo.GetAllAsync(cancellationToken);
                    return Ok(all);
                }

                // Validate inputs when any pagination param is present
                var effectivePage = page ?? 1;
                var effectivePageSize = pageSize ?? 10;

                if (effectivePage <= 0 || effectivePageSize <= 0)
                {
                    return BadRequest("page and pageSize must be positive integers.");
                }

                if (effectivePageSize > MaxPageSize)
                {
                    effectivePageSize = MaxPageSize; // coerce to cap
                }

                var (items, total) = await _repo.GetPagedAsync(effectivePage, effectivePageSize, cancellationToken);

                var totalPages = total == 0
                    ? 0
                    : (int)Math.Ceiling(total / (double)effectivePageSize);

                var envelope = new
                {
                    items,
                    total,
                    page = effectivePage,
                    pageSize = effectivePageSize,
                    totalPages
                };

                return Ok(envelope);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("GET /api/products was canceled by the client.");
                return new StatusCodeResult(StatusCodes.Status499ClientClosedRequest); // optional
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in GET /api/products");
                return Problem(title: "An unexpected error occurred.", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
