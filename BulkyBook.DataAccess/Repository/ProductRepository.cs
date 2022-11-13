using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product obj)
        {
            var productFromDb = _db.Products.FirstOrDefault(c=>c.Id==obj.Id);
            if(productFromDb!= null)
            {
                productFromDb.Title=obj.Title;
                productFromDb.Description=obj.Description;
                productFromDb.Category=obj.Category;
                productFromDb.CategoryId=obj.CategoryId;
                productFromDb.Author=obj.Author;
                productFromDb.ISBN =obj.ISBN;
                productFromDb.ListPrice=obj.ListPrice;
                productFromDb.Price=obj.Price;
                productFromDb.Price100=obj.Price100;
                productFromDb.Price50  =obj.Price50;
                productFromDb.CoverTypeId=  obj.CoverTypeId;
                if(obj.ImageUrl!=null)
                { productFromDb.ImageUrl=obj.ImageUrl; }
            }
            _db.Products.Update(obj);
        }
    }
}
