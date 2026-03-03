using UnityEngine;

[CreateAssetMenu(fileName = "StampPressureData")]
public class StampPressureData : ScriptableObject
{
    [System.Serializable]
    public class PressureData
    {
        public Sprite Image;
        public float Value;
    }
    public PressureData[] PressureDatas;
}
