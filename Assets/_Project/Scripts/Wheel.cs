using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Wheel : MonoBehaviour
{
    [SerializeField] private RectTransform _wheelBase;
    [SerializeField] private Button _spinButton;

    [SerializeField] private float _spinDuration = 4f;
    [SerializeField] private int _minSpinRounds= 3;

    private bool _isSpinning = false;

    void Awake()
    {
        _spinButton.onClick.AddListener(Spin);
    }
    
    private void Spin()
    {
        if(_isSpinning)
            return;

        _isSpinning = true;
        _spinButton.interactable = false;

        float randomEndAngle = Random.Range(0f, 360f);
        float total = _minSpinRounds * 360f + randomEndAngle;

        _wheelBase.transform.DORotate(new Vector3(0f, 0f, -total), _spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutCubic)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
               _isSpinning = false;
               _spinButton.interactable = true; 
            });
    }
}
