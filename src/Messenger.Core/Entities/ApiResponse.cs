using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Core.Entities
{
    public class ApiResponse
    {
        public double Liveness { get; set; }
        public double MatchScore { get; set; }
        public string MatchedId { get; set; }
        public string Message { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
