namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICoverTypeRepository CoverTypeRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        public IApplicationUserRepository ApplicationUserRepository { get;  }
        public IShoppingCartRepository ShoppingCartRepository { get;  }
        public IOrderDetailRepository OrderDetailRepository { get; }
        public IOrderHeaderRepository OrderHeaderRepository { get; }
        void Save();
    }
}
