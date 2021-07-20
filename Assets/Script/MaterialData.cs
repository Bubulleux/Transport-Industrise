using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "Material", menuName = "MyGame/Material")]
    public class MaterialData : ScriptableObject
    {
        public string name;
        public int buyPrice;
        public int sellPrice;
    }
}
