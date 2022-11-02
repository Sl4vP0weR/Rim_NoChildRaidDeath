global using Mod = LivesMatter.Mod;

namespace LivesMatter;

public sealed class Mod : Verse.Mod
{
    public static Mod Instance { get; private set; }
    public static readonly Harmony Harmony = new(nameof(LivesMatter));
    public static ModSettings Settings => Instance.GetSettings<ModSettings>();
    public Mod(ModContentPack content) : base(content)
    {
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Instance = this;
        try
        {
            Harmony.PatchAll();
            Log.Message($"{nameof(LivesMatter)} patched successfully...");
        }
        catch (Exception ex) { Log.Error(ex + ""); }
    }

    public override string SettingsCategory() => nameof(LivesMatter);
    public override void DoSettingsWindowContents(Rect rect)
    {
        base.DoSettingsWindowContents(rect);
        Settings.Draw(rect);
    }

    public static float ChanceOfDeath(PawnType type) => Settings.Sliders[type].Chance;
    public static float GetDeathChance_HealthTracker(Pawn_HealthTracker health)
    {
        var pawn = health.hediffSet.pawn;
        return GetDeathChance_Injury(pawn);
    }
    public static float GetDeathChance_Injury(Pawn pawn)
    {
#if v1_4
        var isChild = pawn.DevelopmentalStage.HasAny(DevelopmentalStage.Newborn | DevelopmentalStage.Baby | DevelopmentalStage.Child);
        if (isChild) return ChanceOfDeath(PawnType.Child);
#endif
        if (pawn.IsColonist) return ChanceOfDeath(PawnType.Colonist);
        if (pawn.IsPrisoner) return ChanceOfDeath(PawnType.Prisoner);
        if (pawn.IsSlave) return ChanceOfDeath(PawnType.Slave);
        if (pawn.HostileTo(Find.FactionManager.OfPlayer)) return ChanceOfDeath(PawnType.Enemy);

        return ChanceOfDeath(PawnType.Other);
    }
}