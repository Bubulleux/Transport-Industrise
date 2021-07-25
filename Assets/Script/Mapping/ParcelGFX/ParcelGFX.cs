using System.Linq;
using Script.Mapping.ParcelType;
using UnityEngine;


public class ParcelGFX : MonoBehaviour
{
    public Parcel parcel;

    public virtual void UpdateGFX()
    {
        transform.position = new Vector3(parcel.pos.x + 0.5f, parcel.corner.Max() + 0.01f, parcel.pos.y + 0.5f);
        transform.rotation = Quaternion.Euler(0, ((int)parcel.orientation + 1) * 90, 0);
    }
}