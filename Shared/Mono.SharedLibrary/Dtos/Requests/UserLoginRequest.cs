using Mono.SharedLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Requests
{
    public class UserLoginRequest
    {
        [Display(Name = nameof(DisplayNameResource.username), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? UserName { get; set; }

        [Display(Name = nameof(DisplayNameResource.password), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? Password { get; set; }
    }
}
