
using UnityEngine;

[CreateAssetMenu(fileName = "RewardData", menuName = "WheelOfFortune/RewardData")]
public class RewardData : ScriptableObject
{
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _baseAmount;

    public string DisplayName => _displayName;
    public Sprite Icon => _icon;       
    public int BaseAmount => _baseAmount;
}
