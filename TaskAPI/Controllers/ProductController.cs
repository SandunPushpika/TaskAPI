using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.Web.Controllers {
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase {

        private readonly IProductService _service;

        public ProductController(IProductService service) {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,GUEST")]
        public async Task<ActionResult<List<ProductModel>>> GetAllProducts() {
            return Ok((await _service.GetAllProducts()).ToList());
        }

        [HttpGet("search")]
        [Authorize(Roles = "ADMIN,GUEST")]
        public async Task<ActionResult<List<ProductModel>>> SearchProductsByName([FromQuery] string name) {
            return Ok((await _service.GetProductsByName(name)).ToList());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,GUEST")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id) {
            var product = await _service.GetProductById(id);

            if(product == null) return NotFound("Product Not Found!");
            
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ProductModel>> AddNewProduct(ProductModel product) {
            var p = await _service.AddNewProduct(product);
            return Created($"{product.product_id}", p);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> UpdateProduct(ProductModel model, int id) {
            var res = await _service.UpdateProduct(model, id);
            
            if(res == false) return NotFound("Id Not Found!");
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> DeleteProduct(int id) { 
            var res = await _service.DeleteProduct(id);

            if (res == false) return NotFound("Id Not Found!");

            return NoContent();
        }

    }
}
