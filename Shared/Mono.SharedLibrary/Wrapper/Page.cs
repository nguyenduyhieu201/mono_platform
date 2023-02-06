using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Wrapper
{
    public class Page<T> : IPage<T> where T : class
    {
        public int TotalItems { get; set; }
        public IList<T>? Items { get; set; }

        public Page() { }
        public Page(int totalItems, IList<T>? items)
        {
            TotalItems = totalItems;
            Items = items;
        }
    }
}
