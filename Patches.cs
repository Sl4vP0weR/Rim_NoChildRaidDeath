using System.Reflection.Emit;
using Verse;

[Harmony]
public static class Patches
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(CheckForStateChange)), 
    HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> CheckForStateChange(IEnumerable<CodeInstruction> instructions)
    {
        var list = instructions.ToList();
        var field = typeof(Difficulty).GetField(nameof(Difficulty.enemyDeathOnDownedChanceFactor), AccessTools.all);
        var 
        idx = list.FindIndex(x => x.LoadsField(field));
        idx = list.FindIndex(idx-1, x => x.IsStloc()); // set of total chance
        int Insert(OpCode op, object operand = null) 
        { 
            list.Insert(idx, new(op, operand)); 
            return idx++; 
        }
        Insert(OpCodes.Pop);
        Insert(OpCodes.Ldarg_0); // loading health
        Insert(OpCodes.Call, typeof(Mod).GetMethod(nameof(Mod.GetDeathChance_HealthTracker), AccessTools.all)); // call our method
        return list;
    }
    [HarmonyPatch(typeof(DamageWorker_AddInjury), nameof(FinalizeAndAddInjury), 
        typeof(Pawn), typeof(Hediff_Injury), typeof(DamageInfo), typeof(DamageWorker.DamageResult)), 
    HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> FinalizeAndAddInjury(IEnumerable<CodeInstruction> instructions)
    {
        var list = instructions.ToList();
        var field = typeof(Difficulty).GetField(nameof(Difficulty.allowInstantKillChance), AccessTools.all);
        var
        idx = list.FindIndex(x => x.LoadsField(field))+1;
        int Insert(OpCode op, object operand = null)
        {
            list.Insert(idx, new(op, operand));
            return idx++;
        }
        Insert(OpCodes.Pop);
        Insert(OpCodes.Ldarg_1); // loading pawn
        Insert(OpCodes.Call, typeof(Mod).GetMethod(nameof(Mod.GetDeathChance_Injury), AccessTools.all)); // call our method
        return list;
    }
}