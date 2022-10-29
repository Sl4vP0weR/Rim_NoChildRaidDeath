global using static LivesMatter.Translations;

namespace LivesMatter;

public static partial class Translations
{
    public const string 
        Settings_DeathChance = "Chance of death on down for {0}: {1}",
        Settings_NotAvailableInGame = "Not available while ingame!",
        PawnType_Prefix = "PawnType_";
    public static TaggedString TryTranslate(this string key, string defaultValue)
    {
        if(key.TryTranslate(out var result))
            return result;
        return defaultValue;
    }
}