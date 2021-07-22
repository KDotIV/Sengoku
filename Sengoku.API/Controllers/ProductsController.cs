using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;
using Sengoku.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sengoku.API.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductsRepository _productsRepository;

        public ProductsController(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult> GetProductsAsync(int limit = 20, int page = 0,
            int sort = -1)
        {
            var products = await _productsRepository.GetAllProducts(limit, page, sort);
            return Ok(products);
        }
        [HttpGet]
        [Route("GetProduct/{productId}", Name = "GetProductId")]
        public async Task<ActionResult> GetProductByIdAsync(string productId)
        {
            var result = await _productsRepository.GetProductById(productId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetProductByName/{productName}", Name = "GetProductName")]
        public async Task<ActionResult> GetProductByNameAsync(string productName)
        {
            var products = await _productsRepository.GetProductByName(productName);
            return Ok(products);
        }
        [HttpGet]
        [Route("GetProductBySupplier/{supplierName}", Name = "GetProductSupplier")]
        public async Task<ActionResult> GetProductBySupplierAsync(string supplierName)
        {
            var products = await _productsRepository.GetProductBySupplier(supplierName);
            return Ok(products);
        }
        [HttpGet]
        [Route("GetProduct/{productId}", Name = "GetProductPrice")]
        public async Task<ActionResult> GetProductPriceAsync(string productId)
        {
            var result = await _productsRepository.GetProductPrice(productId);
            return Ok(result);
        }
        [HttpPost]
        [Route("RegisterProduct")]
        public async Task<ActionResult> RegisterProduct([FromBody] Products product)
        {
            var response = await _productsRepository.AddProductAsync(product.Product_Name, product.Price, product.Stock,
                product.Supplier, product.Description);
            if(!response.Success) { return BadRequest(new { error = response.ErrorMessage }); }

            return Ok(response);
        }
        [HttpDelete("DeleteProduct/{productId}")]
        public async Task<ActionResult> DeleteProductAsync(string productId)
        {
            var result = await _productsRepository.DeleteProduct(productId);
            return Ok(result);
        }
        [HttpPut("UpdateProductName/{productId}")]
        public async Task<ActionResult> UpdateProductNameAsync([FromBody] Products product, string productId)
        {
            var productToUpdate = await _productsRepository.GetProductById(productId);
            var nameResult = await _productsRepository.UpdateProductName(productToUpdate.Product_Name, product.Product_Name);

            return Ok(nameResult);
        }
        [HttpPut("UpdateProductDesc/{productId}")]
        public async Task<ActionResult> UpdateProductDescAsync([FromBody] Products product, string productId)
        {
            var productToUpdate = await _productsRepository.GetProductById(productId);
            var result = await _productsRepository.UpdateProductDescription(productToUpdate.Description, product.Description);

            return Ok(result);
        }
        [HttpPut("UpdateProductPrice/{productId}")]
        public async Task<ActionResult> UpdateProductPriceAsync([FromBody] Products product, string productId)
        {
            var productToUpdate = await _productsRepository.GetProductById(productId);
            var result = await _productsRepository.UpdateProductPrice(productToUpdate.Price, product.Price);

            return Ok(result);
        }
        [HttpPut("UpdateProductStock/{productId}")]
        public async Task<ActionResult> UpdateProductStockAsync([FromBody] Products product, string productId)
        {
            var productToUpdate = await _productsRepository.GetProductById(productId);
            var result = await _productsRepository.UpdateProductPrice(productToUpdate.Stock, product.Stock);

            return Ok(result);
        }
    }
}
