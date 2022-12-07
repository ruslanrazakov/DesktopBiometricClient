using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Messenger.Core.Entities
{
    public class ApiRequest
    {
        [JsonPropertyName("frames")]
        public List<byte[]> Frames { get; set; }
    }
}
