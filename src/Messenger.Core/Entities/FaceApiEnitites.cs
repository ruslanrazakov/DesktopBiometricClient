using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messenger.Core.Entities
{

    class FaceApiTaskRequest
    {
        [JsonPropertyName("engine_id")]
        public string EngineId { get; set; }

        [JsonPropertyName("file_hash")]
        public string FileHash { get; set; }
    }

    class FaceApiTaskResponse
    {
        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }
    }

    interface IFaceApiTaskResult { }

    class FaceApiTaskResult : IFaceApiTaskResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public Result Result { get; }
    }

    public class Result
    {
        [JsonPropertyName("face_id")]
        public string FaceId { get; set; }
    }


    public class FaceApiTaskResultError : IFaceApiTaskResult
    {
        [JsonPropertyName("detail")]
        public List<Detail> Detail { get; set; }
    }

    public class Detail
    {
        [JsonPropertyName("loc")]
        public List<string> Loc { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
