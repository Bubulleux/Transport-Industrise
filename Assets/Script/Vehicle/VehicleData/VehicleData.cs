using UnityEngine;

namespace Script.Vehicle.VehicleData
{
    [CreateAssetMenu(fileName = "Vehicle", menuName = "MyGame/Vehicle")]
    public class VehicleData : ScriptableObject
    {
        public string model;
        public string description;
        public float speed;
        public int price;
        public int maxMaterialTransport;
        public MaterialData[] materialCanTransport;
    }
}
