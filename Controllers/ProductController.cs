using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductListWithCache5.Interfaces;
using ProductListWithCache5.Models;

namespace ProductListWithCache5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            return Ok(_productRepo.GetAll());
        }

        [HttpGet("GetProductsById")]
        public IActionResult GetProductById(int id)
        {
            return Ok(_productRepo.GetById(id));
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct(Product product)
        {
            var products = _productRepo.Create(product);
            return CreatedAtAction(nameof(GetProductById), new {id = products.Id}, products);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct(Product product, int id)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }
            var products = _productRepo.GetById(id);
            if (products == null)
            {
                return NotFound();
            }
            _productRepo.Update(product);
            return Ok(products);
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var products = _productRepo.GetById(id);
            if (products == null)
            {
                return NotFound();
            }
            _productRepo.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
