﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
       
        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u=>u.Id==id);
            if(orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                orderFromDb.PaymentDate = DateTime.Now;
                if(paymentStatus != null)
                    orderFromDb.PaymentStatus = paymentStatus;
            }
        }
    }
}
