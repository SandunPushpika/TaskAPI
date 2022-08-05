using Microsoft.Extensions.Logging;
using TaskAPI.Core.Entities;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.BusinessLogic.Services {
    public class ProductService : IProductService {

        private readonly IDbContext _context;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IDbContext context, ILogger<ProductService> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task<ProductModel> AddNewProduct(ProductModel model) {

            string query = "insert into products (product_name,product_description,product_price) values (@product_name,@product_description,@product_price)";
            return await _context.AddObject<ProductModel>(query, model);

        }

        public async Task DeleteProduct(int id) {

            var model = await GetProductById(id);

            if (model == null) {
                _logger.LogError(new Exception("Id not found"), "{id} not found", id);
                return;
            }
            
            string query = "delete from products where product_id = @id";
            await _context.DeleteById(query, id);

        }

        public async Task<IEnumerable<ProductModel>> GetAllProducts() {

             return await _context.GetAllObjects<ProductModel>("Select * from products");

        }

        public async Task<ProductModel> GetProductById(int id) {

            string query = "select * from products where product_id = @id";
            var product = await _context.GetObjectById<ProductModel>(query, id);

            if (product == null) {
                _logger.LogError("Id not found!");
                return null;
            }

            return product;
        }

        public async Task<IEnumerable<ProductModel>> GetProductsByName(string name) {

            string query = $"select * from products where product_name like '%{name}%'";
            return await _context.GetAllObjects<ProductModel>(query);

        }

        public async Task UpdateProduct(ProductModel product, int id) {

            var model = await GetProductById(id);

            if (model == null) {
                _logger.LogError(new Exception("Id not found"), "{id} not found", id);
                return;
            }

            product.product_id = id;

            string query = "update products set product_name = @product_name, product_description = @product_description, product_price = @product_price where product_id=@product_id";
            await _context.UpdateObject(query,product);

        }
    }
}
