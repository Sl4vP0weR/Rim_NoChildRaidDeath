namespace LivesMatter;

public class ModSettings : Verse.ModSettings
{
    readonly Dictionary<PawnType, ChanceSlider> sliders = new();
    public IReadOnlyDictionary<PawnType, ChanceSlider> Sliders => sliders;

    readonly static IReadOnlyList<PawnType> pawnTypes = Enum.GetValues(typeof(PawnType)).OfType<PawnType>().ToList();

    public ModSettings()
    {
        foreach(var type in pawnTypes)
            sliders.Add(type, new(type));
    }
    public Rect ViewRect { get; private set; }
    public void Draw(Rect rect)
    {
        if (Find.Scenario is not null)
        {
            Widgets.Label(rect, nameof(Settings_NotAvailableInGame).TryTranslate(Settings_NotAvailableInGame));
            return;
        }
        ViewRect = new Rect(0, 100, rect.width, rect.height - 16f);

        Listing_Standard listing = new();

        listing.Begin(ViewRect);
        foreach(var type in pawnTypes)
        {
#if v1_4
            if (!ModLister.BiotechInstalled)
                if(type == PawnType.Child)
                    continue;
#endif
            sliders[type] = Draw(listing, sliders[type]);
        }
        listing.End();
    }
    ChanceSlider Draw(Listing listing, ChanceSlider slider)
    {
        slider.Draw(listing.GetRect(50));
        return slider;
    }
    public override void ExposeData()
    {
        base.ExposeData();
        foreach (var slider in sliders.Values)
            slider.ExposeData();
    }
}