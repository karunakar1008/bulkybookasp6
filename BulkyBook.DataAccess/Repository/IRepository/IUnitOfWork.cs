namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public ICoverTypeRepository CoverTypeRepository { get; }
        public IProductRepository ProductRepository { get; }
        public ICompanyRepository CompanyRepository { get; }
        void Save();
    }
}
