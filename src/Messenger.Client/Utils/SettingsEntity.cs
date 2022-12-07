using Messenger.Client.MVVM;
using System.Text.Json.Serialization;

namespace Messenger.Client.Utils
{
    public class SettingsEntity : ViewModelBase
    {
        bool _showPosition;
        [JsonPropertyName("show_position")]
        public bool ShowPosition
        {
            get => _showPosition;
            set => SetProperty(ref _showPosition, value);
        }

        bool _showAge;
        [JsonPropertyName("show_age")]
        public bool ShowAge
        {
            get => _showAge;
            set => SetProperty(ref _showAge, value);
        }

        bool _showDepartment;
        [JsonPropertyName("show_department")]
        public bool ShowDepartment
        {
            get => _showDepartment;
            set => SetProperty(ref _showDepartment, value);
        }

        Messenger.Core.Entities.Engine _engine;
        [JsonPropertyName("engine_id")]
        public Messenger.Core.Entities.Engine Engine 
        {
            get => _engine;
            set => SetProperty(ref _engine, value);
        }

        [JsonPropertyName("luna-gate")]
        public string LunaGate { get; set; }

        [JsonPropertyName("n-tech-gate")]
        public string NTechGate { get; set; }

        [JsonPropertyName("tevian-gate")]
        public string TevianGate { get; set; }

        [JsonPropertyName("all-engines-gate")]
        public string AllEnginesGate { get; set; }
    }
}
