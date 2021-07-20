using UnityEngine;

namespace Script.Mapping
{
    [CreateAssetMenu(fileName = "Industrise", menuName = "MyGame/Industrise")]
    public class IndustriseData : ScriptableObject
    {
        public string name;
        public MaterialData[] materialInpute;
        public MaterialData[] materialOutpute;
        public Color32 color;
        public float height;
    }
}
