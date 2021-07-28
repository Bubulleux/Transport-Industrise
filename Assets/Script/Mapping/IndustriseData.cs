using UnityEngine;

namespace Script.Mapping
{
    [CreateAssetMenu(fileName = "Industrise", menuName = "MyGame/Industrise")]
    public class IndustriseData : ScriptableObject
    {
        public string name;
        public ProductData[] productIn;
        public ProductData[] productOut;
        public Color32 color;
        public float height;
    }
}
