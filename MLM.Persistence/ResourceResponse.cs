using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM.Persistence
{
    public class ResourceResponse<T>
    {
        public T Item { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string ErrorDescription { get; set; }

        public ResourceResponse() { }

        public bool IsSuccess()
        {
            return this.HttpStatusCode == HttpStatusCode.OK;
        }
    }

}
