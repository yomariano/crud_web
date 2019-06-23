using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewWeb.Infrastructure.Models;

namespace InterviewWeb.Infrastructure
{
    public interface IProductRepository
    {
        Task<IList<Product>> GetAll();

        Product GetById(int id);

        bool Post(string value);

        bool Put(int id, string value);

        bool Delete(int id);
    }
}