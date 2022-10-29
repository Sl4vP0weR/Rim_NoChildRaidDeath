﻿global using Mod = NoChildRaidDeath.Mod;

namespace NoChildRaidDeath;

public class Mod : Verse.Mod
{
    public static Mod Instance;
    public static readonly Harmony Harmony = new(nameof(NoChildRaidDeath));
    public Mod(ModContentPack content) : base(content)
    {
        try
        {
            Harmony.PatchAll();
            Log.Message($"{nameof(NoChildRaidDeath)} patched successfully...");
        }
        catch (Exception ex) { Log.Error(ex + ""); }
    }

    public static bool IsDeathAllowed(Pawn_HealthTracker health)
    {
        var pawn = health.hediffSet.pawn;
        var stage = pawn.DevelopmentalStage;
        var result = !stage.HasAny(DevelopmentalStage.Baby | DevelopmentalStage.Newborn | DevelopmentalStage.Child);
        return result;
    }
}