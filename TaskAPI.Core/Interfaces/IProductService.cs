using TaskAPI.Core.Entities;

namespace TaskAPI.Core.Interfaces {
    public interface IProductService {

        public Task<ProductModel> AddNewProduct(ProductModel model);

        public Task<IEnumerable<ProductModel>> GetAllProducts();

        public Task<ProductModel> GetProductById(int id);

        public Task<IEnumerable<ProductModel>> GetProductsByName(string name);

        public Task<Boolean> UpdateProduct(ProductModel product, int id);

        public Task<Boolean> DeleteProduct(int id);

    }
}
