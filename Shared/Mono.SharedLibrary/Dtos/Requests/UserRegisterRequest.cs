using Mono.SharedLibrary.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Dtos.Requests
{
    public class UserRegisterRequest
    {
        [Display(Name = nameof(DisplayNameResource.firstname), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string FirstName { get; set; }

        [Display(Name = nameof(DisplayNameResource.lastname), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string LastName { get; set; }

        [Display(Name = nameof(DisplayNameResource.email), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        [EmailAddress(ErrorMessageResourceName = nameof(ErrorMessageResource.email_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Email { get; set; }

        [Display(Name = nameof(DisplayNameResource.username), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        [MaxLength(50, ErrorMessageResourceName = nameof(ErrorMessageResource.string_maxlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string UserName { get; set; }

        [Display(Name = nameof(DisplayNameResource.password), ResourceType = typeof(DisplayNameResource))]
        [Required(ErrorMessageResourceName = nameof(ErrorMessageResource.required_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        [MinLength(6, ErrorMessageResourceName = nameof(ErrorMessageResource.string_minlength_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessageResourceName = nameof(ErrorMessageResource.password_confirm_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string ConfirmPassword { get; set; }

        [Phone(ErrorMessageResourceName = nameof(ErrorMessageResource.phone_error), ErrorMessageResourceType = typeof(ErrorMessageResource))]
        public string? PhoneNumber { get; set; }
    }
}
