using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.CoreService.Interfaces
{
    public interface ICoreServiceManager
    {
        IUserRepository User { get; }
        IFileRepository File { get; }
    }
}
