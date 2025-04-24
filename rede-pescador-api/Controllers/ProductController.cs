using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rede_pescador_api.Data;
using rede_pescador_api.Dto;
using rede_pescador_api.Models.Enums;
using rede_pescador_api.Services;

namespace rede_pescador_api.Controllers
{
    [ApiController]
    [Route("rede-pescador/produtos")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _context;
        public ProductsController(IProductService productService, AppDbContext context)
        {
            _productService = productService;
            _context = context;
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

      
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductDTO>> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Produto não encontrado." });

            return Ok(product);
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpPost("novo")]
        public async Task<ActionResult> Create([FromBody] ProductDTO productDTO)
        {
            await _productService.AddProductAsync(productDTO);
            return Ok("Produto criado com sucesso!");
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, [FromBody] ProductDTO productDTO)
        {
            try
            {
                await _productService.UpdateProductAsync(id, productDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "PESCADOR")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [Authorize]
        [HttpGet("filtros")]
        public async Task<IActionResult> GetProducts(
        string tipo = null,
        bool? available = null,
        string localization = null) 
        {
            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
            {
                if (Enum.TryParse<FishTypes>(tipo, true, out var tipoEnum))
                {
                    productsQuery = productsQuery.Where(p => p.Tipo == tipoEnum);
                }
                else
                {
                    return BadRequest("Tipo inválido.");
                }
            }            
            if (available.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Avaliável == available.Value);
            }
            if (!string.IsNullOrEmpty(localization))
            {
                var locationParts = localization.Split(" - ");

                if (locationParts.Length == 2)
                {
                    string city = locationParts[0].Trim();
                    string state = locationParts[1].Trim();

                    productsQuery = productsQuery
                        .Where(p => p.Localizacao.Contains(localization));
                }
            }

            var products = await productsQuery.ToListAsync();
            return Ok(products);
        }

    }
}
