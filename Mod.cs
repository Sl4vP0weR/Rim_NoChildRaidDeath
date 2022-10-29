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

    public static ChanceSlider GetChance(PawnType type) => type switch
    {
        PawnType.Child => Settings.Childs,
        PawnType.Prisoner => Settings.Prisoners,
        PawnType.Slave => Settings.Slaves,
        _ => Settings.Default
    };
    public static bool ChanceDeath(PawnType type) => Rand.Chance(GetChance(type).Chance);
    public static bool _IsDeathAllowed(Pawn_HealthTracker health)
    {
        var pawn = health.hediffSet.pawn;
        var result = IsDeathAllowed(pawn);
#if DEBUG
        Log.Clear();
        Log.TryOpenLogWindow();
        Log.Message($"{pawn.NameFullColored}: {result}");
#endif
        return result;
    }

    private static bool IsDeathAllowed(Pawn pawn)
    {
        bool ChanceAlive(bool condition = true, PawnType type = PawnType.Default) => condition && !ChanceDeath(type);
#if v1_4
        var isChild = pawn.DevelopmentalStage.HasAny(DevelopmentalStage.Newborn | DevelopmentalStage.Baby | DevelopmentalStage.Child);
        if (ChanceAlive(isChild, PawnType.Child)) return false;
#endif
        if (ChanceAlive(pawn.IsPrisoner, PawnType.Prisoner)) return false;
        if (ChanceAlive(pawn.IsSlave, PawnType.Slave)) return false;
        if (ChanceAlive()) return false;
        return true;
    }
}