using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "Product", menuName = "MyGame/Product")]
    public class ProductData : ScriptableObject
    {
        public string productionName;
        public int buyPrice;
        public int sellPrice;
    }
}
