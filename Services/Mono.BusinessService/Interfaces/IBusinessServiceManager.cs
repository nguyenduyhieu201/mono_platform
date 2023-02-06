using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.BusinessService.Interfaces
{
    public interface IBusinessServiceManager
    {
        ITeacherRepository Teacher { get; }
        IStudentRepository Student { get; }
        IDocumentRepository Document { get; }
    }
}
