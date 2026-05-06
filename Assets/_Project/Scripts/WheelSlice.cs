using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSlice : MonoBehaviour
{
    [SerializeField] private Image _sliceImage;
    [SerializeField] private TMP_Text _sliceText;

    public int Index { get; private set;}

    public void Initialize(int index, Sprite sprite, int amount)
    {
        Index = index;

        if(_sliceImage != null)
            _sliceImage.sprite = sprite;
        
        if(_sliceText != null)
            _sliceText.text = $"x{amount}";
    }
}
