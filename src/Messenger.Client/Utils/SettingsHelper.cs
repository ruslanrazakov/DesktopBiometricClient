using Messenger.Client.Models;
using Messenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messenger.Client.Utils
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
            Engine.Luna => Get(settingsPath).LunaGate,
            Engine.Tevian => Get(settingsPath).TevianGate,
            Engine.NTech => Get(settingsPath).NTechGate,
            Engine.All => Get(settingsPath).AllEnginesGate,
            _ => ""
        };

        public static void Save(SettingsEntity settings, string settingsPath)
        {
            string optionsJson = JsonSerializer.Serialize(settings);
            File.WriteAllText(settingsPath, optionsJson);
        }
    }
}
