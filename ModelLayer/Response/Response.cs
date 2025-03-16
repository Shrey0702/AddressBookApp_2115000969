using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Response
{
    public class Response<T>
    {
        // later we can use specified response for specified tasks now we are just using the same response for all tasks
        // due to which out code looks more clean and easy to understand
        // but it is less informative and less specific
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; } = default(T);
    }
}
