using System.Text.Json.Serialization;

namespace Messenger.Core.Entities
{
    public class SettingsEntity
    {
        [JsonPropertyName("show_gender")]
        public bool ShowGender { get; set; }

        [JsonPropertyName("show_age")]
        public bool ShowAge { get; set; }

        [JsonPropertyName("show_department")]
        public bool ShowDepartment { get; set; }

        [JsonPropertyName("engine_id")]
        public Messenger.Core.Entities.Engine Engine { get; set; }

        [JsonPropertyName("luna-id")]
        public string LunaId { get; set; }

        [JsonPropertyName("n-tech-id")]
        public string NTechId { get; set; }

        [JsonPropertyName("tevian-id")]
        public string TevianId { get; set; }

        [JsonPropertyName("upload-file-gate")]
        public string UploadFileGate { get; set; }

        [JsonPropertyName("liveness-gate")]
        public string LivenessGate { get; set; }

        [JsonPropertyName("best-match-gate")]
        public string BestMatchGate { get; internal set; }

        [JsonPropertyName("task-result-gate")]
        public string TaskResultGate { get; internal set; }
    }
}
