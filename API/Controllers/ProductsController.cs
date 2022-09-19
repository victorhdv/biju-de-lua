
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productsTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductType> productsTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productsTypeRepo = productsTypeRepo;
            _productsRepo = productsRepo;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts(
            [FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductsWithTypesSpecification(productSpecParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productSpecParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            return Ok(new Pagination<ProductToReturnDTO>(productSpecParams.PageIndex, productSpecParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        //let swagger know what type of messages it can display
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(400));

            return _mapper.Map<Product, ProductToReturnDTO>(product);
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productsTypeRepo.ListAllAsync();

            return Ok(types);
        }
    }
}