using System;
using System.Reflection.Emit;
using Verse.Noise;
using Verse;

[Harmony]
public static class Patch_HealthTracker
{
    [HarmonyPatch(typeof(Pawn_HealthTracker), nameof(CheckForStateChange)), HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> CheckForStateChange(IEnumerable<CodeInstruction> instructions, ILGenerator gen)
    {
        var list = instructions.ToList();
        var field = typeof(Difficulty).GetField(nameof(Difficulty.enemyDeathOnDownedChanceFactor), AccessTools.all);
        var 
        idx = list.FindIndex(x => x.LoadsField(field));
        idx = list.FindIndex(idx-1, x => x.IsStloc());
        int Insert(OpCode op, object operand = null) 
        { 
            list.Insert(idx, new CodeInstruction(op, operand)); 
            return idx++; 
        }
        Insert(OpCodes.Ldarg_0); // loading instance
        Insert(OpCodes.Call, typeof(Mod).GetMethod(nameof(Mod.IsDeathAllowed), AccessTools.all)); // call our method
        var lab = gen.DefineLabel(); // defining new label for jump
        var statement =
        Insert(OpCodes.Brtrue_S, lab); // start of if statement

        Insert(OpCodes.Pop);
        Insert(OpCodes.Ldc_R4, 0f);

        list[Insert(OpCodes.Nop)].WithLabels(lab); // labeled instruction for jump if player doesn't have bypass
        return list;
    }
}