using rede_pescador_api.Dto;

namespace rede_pescador_api.Services
{
    public interface IProductService
    {
        Task<ProductDTO> GetByIdAsync(long id);
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task AddProductAsync(ProductDTO productDTO);
        Task UpdateProductAsync(long id, ProductDTO productDTO);
        Task DeleteProductAsync(long id);
    }
}
