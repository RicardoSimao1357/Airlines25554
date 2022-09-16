﻿using Airlines25554.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Airlines25554.Data
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DataContext _context;

        public CustomerRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Customers.Include(p => p.User);
        }
    }
}
