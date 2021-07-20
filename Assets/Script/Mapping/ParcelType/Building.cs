using Newtonsoft.Json;

namespace Script.Mapping.ParcelType
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Building : Parcel
    {
        public float height;
    }
}

