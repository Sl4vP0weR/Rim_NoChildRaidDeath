namespace LivesMatter;

public record struct ChanceSlider(PawnType PawnType, FloatRange? ChanceRange = null) : IExposable
{
    float chance = 0f;
    public float Chance => chance/100f;
    public string TranslatedType => $"{PawnType_Prefix}{PawnType}".Translate();
    public void Draw(Rect rect)
    {
        var 
        label = nameof(Settings_DeathChance).TryTranslate(Settings_DeathChance);
        label = label.Formatted(TranslatedType, Mathf.Round(chance));
        var range = ChanceRange ??= new(0, 100);
     
        chance = Widgets.HorizontalSlider(rect, chance, range.min, range.max, label: label);
    }
    public void ExposeData()
    {
        Scribe_Values.Look(ref chance, TranslatedType, 0f);
    }
}