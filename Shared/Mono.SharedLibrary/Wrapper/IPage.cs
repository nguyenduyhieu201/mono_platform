using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Wrapper
{
    public interface IPage<T> where T : class
    {
        int TotalItems { get; set; }
        IList<T>? Items { get; set; }
    }
}
