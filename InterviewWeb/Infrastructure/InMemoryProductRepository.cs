using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewWeb.Infrastructure.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;

namespace InterviewWeb.Infrastructure
{

    public class InMemoryProductRepository : IProductRepository
    {
        internal SODbContext _context;
        public InMemoryProductRepository(SODbContext context)
        {
            _context = context;
        }

        public async Task<IList<Product>> GetAll()
        {
            return  await _context.Products.AsNoTracking().ToListAsync();
        }

        public Product GetById(int id)
        {

            return _context.Products.AsNoTracking().FirstOrDefault(x => String.Equals(x.Id.ToString(), id.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        public bool Post(string value)
        {
            if (!value.IsNullOrWhiteSpace())
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 10000000);
                var p = new Product
                {
                    DateCreated = DateTime.UtcNow,
                    DateDiscontinued = null,
                    Id = randomNumber,
                    InternalCode = "CODE_",
                    Name = value
                };
                _context.Entry(p).State = EntityState.Detached;

                _context.Products.Add(p);
                _context.SaveChanges();
                return true;
            }

            return false;


        }

        public bool Put(int id, string value)
        {
            var p = _context.Products.FirstOrDefault(x => x.Id == id);
            if (p == null) return false;

            p.Name = value;
            _context.Entry(p).State = EntityState.Detached;

            _context.Products.Update(p);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
            var local = _context.Products.Local.FirstOrDefault(p => p.Id == id);
            if (local == null)
                return false;
            if (product == null)
                return false;

            _context.DetachLocal<Product>(product, product.Id, EntityState.Deleted);
            _context.SaveChanges();
            return true;
        }
    }

    public static class Helper
    {

        public static void DetachLocal<T>(this SODbContext context, T t, int entryId, EntityState state)
            where T : class, IIdentifier
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }

            context.Entry(t).State = state;
        }
    }

    public interface IIdentifier
    {
        int Id { get; set; }
    }

}