using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Sengoku.API.Models;
using Sengoku.API.Models.Responses;

namespace Sengoku.API.Repositories
{
    public class ProductsRepository
    {
        private readonly IMongoDBContext _ctx;
        private readonly IMongoCollection<Products> _productsCollection;
        private const int DefaultProductPerPage = 20;
        private const int DefaultSortOrder = -1;
        private readonly ProjectionDefinition<Products> staticExclution = Builders<Products>.Projection.Exclude("_id");

        public ProductsRepository(IMongoDBContext context)
        {
            var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);

            _ctx = context;
            _productsCollection = _ctx.GetCollection<Products>("Products");
        }

        public async Task<List<Products>> GetAllProducts(int productsPerPage = DefaultProductPerPage, int page = 0,
            int sort = DefaultSortOrder)
        {
            var skip = productsPerPage * page;
            var limit = productsPerPage;

            var products = await _productsCollection
                .Find(products => true)
                .Project<Products>(staticExclution)
                .ToListAsync();

            return products;
        }

        public async Task<Products> GetProductById(string productId)
        {
            return await _productsCollection.Find(Builders<Products>.Filter.Eq(x => x.Product_Id, productId))
                .Project<Products>(staticExclution)
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Products>> GetProductByName(string productName, int productsPerPage = DefaultProductPerPage,
            int page = 0, int sort = DefaultSortOrder, params string[] keywords)
        {
            var skip = productsPerPage * page;
            var limit = productsPerPage;

            var products = await _productsCollection
                .Find(Builders<Products>.Filter.Text(string.Join(",", keywords)))
                .Project<Products>(staticExclution)
                .Limit(limit)
                .Skip(skip)
                .ToListAsync();

            return products;
        }
        public async Task<IReadOnlyList<Products>> GetProductBySupplier(string suppName, int productsPerPage = DefaultProductPerPage,
            int page = 0, int sort = DefaultSortOrder)
        {
            var skip = productsPerPage * page;
            var limit = productsPerPage;

            var products = await _productsCollection
                .Find(Builders<Products>.Filter.Eq(found => found.Supplier, suppName))
                .Project<Products>(staticExclution)
                .ToListAsync();

            return products;
        }
        public async Task<Products> GetProductPrice(string productId)
        {
            var result = await _productsCollection
                .Find(Builders<Products>.Filter.Eq(x => x.Product_Id, productId))
                .Project<Products>(Builders<Products>.Projection.Include("price")).FirstOrDefaultAsync();

            return result;
        }
        public async Task<ProductResponse> AddProductAsync(string productName, decimal price,
            int stock, string supplier, string description = null)
        {
            string randId = Helpers.MakeRandomID();
            if(CheckProductDb(randId))
            {
                await AddProductAsync(productName, price, stock, supplier, description);
            }
            try
            {
                var newProduct = new Products()
                {
                    Product_Id = randId,
                    Product_Name = productName,
                    Description = description,
                    Price = price,
                    Stock = stock,
                    Supplier = supplier
                };
                await _productsCollection.InsertOneAsync(newProduct);
                return new ProductResponse(true, "Product Created", newProduct);
            }
            catch (Exception ex)
            {

                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new ProductResponse(false, "A Product with this name already exists.")
                    : new ProductResponse(false, ex.Message);
            }
        }
        public async Task<ProductResponse> DeleteProduct(string productId)
        {
            try
            {
                var deletedProduct = await _productsCollection.FindOneAndDeleteAsync(Builders<Products>
                    .Filter.Eq(x => x.Product_Id, productId));
                if (!_productsCollection.AsQueryable<Products>().Any(exists => exists.Product_Id == deletedProduct.Product_Id))
                    return new ProductResponse(true, "Product Deleted", deletedProduct);
                return new ProductResponse(false, "Product Deletion was unsuccessful", deletedProduct);
            }
            catch (Exception ex)
            {
                return new ProductResponse(false, ex.ToString());
            }
        }
        public async Task<ProductResponse> UpdateProductName(string productName, string updatedName)
        {
            try
            {
                var updatedProduct = await _productsCollection.FindOneAndUpdateAsync(
                    Builders<Products>.Filter.Where(rec => rec.Product_Name == productName),
                    Builders<Products>.Update
                    .Set(rec => rec.Product_Name, updatedName),
                    options: new FindOneAndUpdateOptions<Products>
                    {
                        ReturnDocument = ReturnDocument.After
                    });
                return new ProductResponse(true, "Product Updated", updatedProduct);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new ProductResponse(false, "Product could not be updated.")
                    : new ProductResponse(false, ex.Message);
            }
        }
        public async Task<ProductResponse> UpdateProductDescription(string description, string updateDesc)
        {
            try
            {
                var updatedProduct = await _productsCollection.FindOneAndUpdateAsync(
                    Builders<Products>.Filter.Where(rec => rec.Description == description),
                    Builders<Products>.Update
                    .Set(rec => rec.Description, updateDesc),
                    options: new FindOneAndUpdateOptions<Products>
                    {
                        ReturnDocument = ReturnDocument.After
                    });

                return new ProductResponse(true, "Product Updated", updatedProduct);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new ProductResponse(false, "Product could not be updated.")
                    : new ProductResponse(false, ex.Message);
            }
        }
        public async Task<ProductResponse> UpdateProductPrice(decimal price, decimal newPrice)
        {
            try
            {
                var updatedProduct = await _productsCollection.FindOneAndUpdateAsync(
                    Builders<Products>.Filter.Where(rec => rec.Price == price),
                    Builders<Products>.Update
                    .Set(rec => rec.Price, newPrice),
                    options: new FindOneAndUpdateOptions<Products>
                    {
                    ReturnDocument = ReturnDocument.After
                    });

                return new ProductResponse(true, "Product Updated", updatedProduct);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new ProductResponse(false, "Product could not be updated.")
                    : new ProductResponse(false, ex.Message);
            }
        }
        public async Task<ProductResponse> UpdateProductStock(int stock, int newStock)
        {
            try
            {
                var updatedProduct = await _productsCollection.FindOneAndUpdateAsync(
                    Builders<Products>.Filter.Where(rec => rec.Stock == stock),
                    Builders<Products>.Update
                    .Set(rec => rec.Stock, newStock),
                    options: new FindOneAndUpdateOptions<Products>
                    {
                        ReturnDocument = ReturnDocument.After
                    });

                return new ProductResponse(true, "Product Updated", updatedProduct);
            }
            catch (Exception ex)
            {
                return ex.Message.StartsWith("MongoError: E1100 duplicate key error")
                    ? new ProductResponse(false, "Product could not be updated.")
                    : new ProductResponse(false, ex.Message);
            }
        }
        private bool CheckProductDb(string productId, string supplier = null)
        {
            if(productId != null)
            {
                if (_productsCollection.AsQueryable<Products>().Any(exists => exists.Product_Id == productId)) { return true; }
            }
            if(supplier != null)
            {
                if (_productsCollection.AsQueryable<Products>().Any(exists => exists.Supplier == supplier)) { return true; }
            }

            return false;
        }
    }
}
