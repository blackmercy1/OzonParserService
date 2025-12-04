namespace OzonParserService.Domain.ConveerCollection;

public partial class CollectionColor : AggregateRoot<CollectionColorId>
{
    public string Name { get; private set; }

    public int CodeColor { get; private set; }

    public int InitialColor { get; private set; }

    public int FinalColor { get; private set; }

    public bool Active { get; private set; }

    private CollectionColor(
        CollectionColorId id,
        string name,
        int codeColor,
        int initialColor,
        int finalColor,
        bool active)
        : base(id)
    {
        Name = name;
        CodeColor = codeColor;
        InitialColor = initialColor;
        FinalColor = finalColor;
        Active = active;
    }

    public static CollectionColor Create(
        string crmId,
        string name,
        int codeColor,
        int initialColor,
        int finalColor,
        bool active)
        => new CollectionColor(
            CollectionColorId.Create(crmId),
            name,
            codeColor,
            initialColor,
            finalColor,
            active);

#pragma warning disable CS8618
    private CollectionColor() { }
#pragma warning restore CS8618
}