using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rede_pescador_api.Dto;
using rede_pescador_api.Services;

namespace rede_pescador_api.Controllers
{
    [ApiController]
    [Route("rede-pescador/produtos")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [Authorize(Roles = "CONSUMIDOR")]
        [Authorize(Roles = "ESTABELECIMENTO")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

      
        [HttpGet("{id}")]
        [Authorize(Roles = "CONSUMIDOR")]
        [Authorize(Roles = "ESTABELECIMENTO")]
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
    }
}
