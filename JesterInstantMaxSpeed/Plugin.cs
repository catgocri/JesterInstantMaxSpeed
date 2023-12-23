using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace JesterInstantMaxSpeed
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        private const string pluginGUID = "catgocri.JesterInstantMaxSpeed";
        private const string pluginName = "Jester Instant Max Speed";
        private const string pluginVersion = "1.0.0";
        readonly Harmony _harmony = new Harmony(pluginGUID);
        internal static new ManualLogSource Logger { get; private set; }
        void Awake()
        {
            Logger = base.Logger;
            _harmony.PatchAll();
            Logger.LogInfo($"Plugin {pluginName} has loaded! v{pluginVersion}");
        }

        [HarmonyPatch]
        class Patches
        {
            [HarmonyPatch(typeof(JesterAI), "Update"), HarmonyTranspiler]
            static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
            {
                CodeMatcher matcher = new CodeMatcher(instructions);
                matcher.MatchForward(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Callvirt),
                    new CodeMatch(OpCodes.Call),
                    new CodeMatch(OpCodes.Ldc_R4, 1.35f),
                    new CodeMatch(OpCodes.Mul)
                    ).Advance(9);
                matcher.SetOperandAndAdvance(18f);
                return matcher.InstructionEnumeration();
            }
        }
    }
}
