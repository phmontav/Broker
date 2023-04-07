using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBrokerLibrary.Models
{
    public class EmailSettings
    {
        public string? fromAddress { get; set; }
        public List<string>? toEmail { get; set; }
        public string? subject { get; set; }
        public string? plainTextContent { get; set; }
        public string? htmlContent { get; set; }
    }
}

