using BCrypt.Net;
using rede_pescador_api.Dto;
using rede_pescador_api.Models;
using rede_pescador_api.Repository;
using rede_pescador_api.Services;
using System.Security.Claims;
using System.Text.RegularExpressions;

public class ProductServiceImpl : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;

    public ProductServiceImpl(IProductRepository productRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
    }

    public async Task<ProductDTO> GetByIdAsync(long id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;

        return new ProductDTO
        {
            Id = product.Id,
            Tipo = product.Tipo,
            Nome = product.Nome,
            Descricao = product.Descricao,
            Localizacao = product.Localizacao,
            PesoKg = product.PesoKg,
            PrecoQuilo = product.PrecoQuilo,
            ImagemURL = product.ImagemURL,
            Avaliável = product.Avaliável,
            IdPescador = product.IdPescador
        };
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products.Select(p => new ProductDTO
        {
            Id = p.Id,
            Tipo = p.Tipo,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Localizacao= p.Localizacao,
            PesoKg = p.PesoKg,
            PrecoQuilo = p.PrecoQuilo,
            ImagemURL = p.ImagemURL,
            Avaliável = p.Avaliável,
            IdPescador = p.IdPescador
        });
    }

    public async Task AddProductAsync(ProductDTO productDTO)
    {
        var pescador = await _userRepository.GetByIdAsync(productDTO.IdPescador);
        if (pescador == null)
            throw new Exception("IdPescador inválido. Nenhum usuário encontrado com esse ID.");

        if (productDTO.Tipo is < 0 or > (rede_pescador_api.Models.Enums.FishTypes)3)
            throw new Exception("Tipo inválido. Só são permitidos valores entre 0 e 3.");

        if (string.IsNullOrEmpty(productDTO.Localizacao) || !IsValidLocationFormat(productDTO.Localizacao))
        {
            throw new Exception("A localização deve estar no formato 'Cidade - Estado'. Exemplo: 'Jundiaí - SP'");
        }

        var product = new Product
        {
            Tipo = productDTO.Tipo,
            Nome = productDTO.Nome,
            Descricao = productDTO.Descricao,
            Localizacao = productDTO.Localizacao,
            PesoKg = productDTO.PesoKg,
            PrecoQuilo = productDTO.PrecoQuilo,
            ImagemURL = productDTO.ImagemURL,
            Avaliável = productDTO.Avaliável,
            IdPescador = productDTO.IdPescador
        };

        await _productRepository.AddAsync(product);
    }

    public async Task UpdateProductAsync(long id, ProductDTO productDTO)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            throw new Exception("Produto não encontrado.");

        var pescador = await _userRepository.GetByIdAsync(productDTO.IdPescador);
        if (pescador == null)
            throw new Exception("IdPescador inválido. Nenhum usuário encontrado com esse ID.");

        if (productDTO.Tipo is < 0 or > (rede_pescador_api.Models.Enums.FishTypes)3)
            throw new Exception("Tipo inválido. Só são permitidos valores entre 0 e 3.");

        if (string.IsNullOrEmpty(productDTO.Localizacao) || !IsValidLocationFormat(productDTO.Localizacao))
        {
            throw new Exception("A localização deve estar no formato 'Cidade - Estado'. Exemplo: 'Jundiaí - SP'");
        }

        product.Tipo = productDTO.Tipo;
        product.Nome = productDTO.Nome;
        product.Descricao = productDTO.Descricao;
        product.Localizacao = productDTO.Localizacao;
        product.PesoKg = productDTO.PesoKg;
        product.PrecoQuilo = productDTO.PrecoQuilo;
        product.ImagemURL = productDTO.ImagemURL;
        product.Avaliável = productDTO.Avaliável;
        product.IdPescador = productDTO.IdPescador;

        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(long id)
    {
        await _productRepository.DeleteAsync(id);
    }

    private bool IsValidLocationFormat(string location)
    {
        var regex = new Regex(@"^[A-Za-zÀ-ÿ\s]+ - [A-Za-zÀ-ÿ]{2}$");
        return regex.IsMatch(location);
    }
}
