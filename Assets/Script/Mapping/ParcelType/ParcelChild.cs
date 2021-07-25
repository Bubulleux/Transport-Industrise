
using Script.Mapping.ParcelType;

public class ParcelChild : Parcel
{
    public Parcel parent;

    public override void Initialaze()
    {
        color = parent.color;
        seeTerrain = parent.seeTerrain;
    }

    public override void Interact() { parent.Interact(); }
}