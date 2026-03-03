using UnityEngine;

[CreateAssetMenu(fileName = "PromotionValue", menuName = "ScriptableObject/PromotionValue")]
public class PromotionValue : ScriptableObject
{
    [Header("昇進役職名")]
    public Post PostType;
    public string PostName;
    public int PromotionScore;
}