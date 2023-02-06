using Mono.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mono.SharedLibrary.Messages
{
    public class SendNotificationEvent
    {
        public User sender { get; set; }
        public IReadOnlyList<User> receivers { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string redirectUrl { get; set; }
        public DateTime sentTime { get; set; }
        public string provider { get; set; }
    }
}

