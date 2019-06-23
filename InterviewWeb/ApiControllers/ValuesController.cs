using System.Collections.Generic;
using System.Web.Http;

namespace InterviewWeb.ApiControllers
{
    public class ValuesController : ApiController
    {
        public ValuesController()
        {
                
        }

        public IEnumerable<string> Get()
        {

            return new List<string>();
        }

        public string Get(int id)
        {

            return string.Empty;
        }

        public void Post(string value)
        {


        }

        public void Put(int id, string value)
        {


        }

        public void Delete(int id)
        {


        }
    }
}
