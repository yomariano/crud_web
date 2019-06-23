using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using InterviewWeb.Infrastructure;
using InterviewWeb.Infrastructure.Models;

namespace InterviewWeb.ApiControllers
{
    [Route("api/products")]
    public class ProductsApiController : ApiController
    {
        private readonly IProductRepository _repository;
        public ProductsApiController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [Route("~/api/products")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _repository.GetAll();
            return Ok(result.Where(p => !p.DateDiscontinued.HasValue).ToList());
        }

        [Route("~/api/products/{id}")]
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var product = _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.DateDiscontinued != null)
            {
                return StatusCode(HttpStatusCode.Gone);
            }
            return Ok(product);
        }

        [HttpPost]
        public IHttpActionResult Post(string value)
        {
            if (_repository.Post(value))
            {
                return NotFound();
            }

            return Ok();

        }

        [HttpPut]
        public IHttpActionResult Put(int id, string value)
        {

            if (_repository.Put(id, value))
            {
                return NotFound();
            }

            return Ok();
        }

        [Route("~/api/products/{id}")]
        [HttpDelete]

        public IHttpActionResult Delete(int id)
        {
            if (!_repository.Delete(id))
            {
                return NotFound();
            }

            return Ok();
        }
    }
}