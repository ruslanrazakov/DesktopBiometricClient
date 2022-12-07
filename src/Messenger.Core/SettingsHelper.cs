using Messenger.Core.Entities;
using System.IO;
using System.Text.Json;

namespace Messenger.Core
{
    public class SettingsHelper
    {
        static SettingsEntity optionsEntity = new();

        public static SettingsEntity Get(string settingsPath)
        {
            string optionsString = File.ReadAllText(settingsPath);
            return JsonSerializer.Deserialize<SettingsEntity>(optionsString);
        }

        public static Engine GetEngine(string settingsPath)
            => SettingsHelper.Get(settingsPath).Engine;

        public static string GetEngineApi(string settingsPath, Engine engine) => engine switch
        {
            Engine.Luna => Get(settingsPath).LunaId,
            Engine.Tevian => Get(settingsPath).TevianId,
            Engine.NTech => Get(settingsPath).NTechId,
            _ => ""
        };

        public static void Save(SettingsEntity settings, string settingsPath)
        {
            string optionsJson = JsonSerializer.Serialize(settings);
            File.WriteAllText(settingsPath, optionsJson);
        }
    }
}
