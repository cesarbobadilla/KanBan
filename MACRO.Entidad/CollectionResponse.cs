using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MACRO.Entidad
{
    public class CollectionResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Count { get; set; }

        public CollectionResponse() { }

        public CollectionResponse(IEnumerable<T> data) : this(data, data.Count()) { }

        public CollectionResponse(IEnumerable<T> data, int count)
        {
            this.Data = data;
            this.Count = count;
        }
    }
}
