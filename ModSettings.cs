namespace LivesMatter;

public class ModSettings : Verse.ModSettings
{
    public ChanceSlider
        Default = new(PawnType.Default),
        Childs = new(PawnType.Child),
        Slaves = new(PawnType.Slave),
        Prisoners = new(PawnType.Prisoner);
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
        Draw(listing, ref Default);
#if v1_4
        if (ModLister.BiotechInstalled)
            Draw(listing, ref Childs);
#endif
        Draw(listing, ref Slaves);
        Draw(listing, ref Prisoners);
        listing.End();
    }
    void Draw(Listing listing, ref ChanceSlider slider) => slider.Draw(listing.GetRect(50));
    public override void ExposeData()
    {
        base.ExposeData();
        Default.ExposeData();
        Childs.ExposeData();
        Slaves.ExposeData();
        Prisoners.ExposeData();
    }
}