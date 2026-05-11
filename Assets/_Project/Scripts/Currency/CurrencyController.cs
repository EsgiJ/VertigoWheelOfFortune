using System;
using UnityEngine;

using WheelOfFortune.Core;

namespace WheelOfFortune.Currency
{   
    public class CurrencyController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private GameConfig _gameConfig;

        public int Gold { get; private set; }   

        public event Action<int> OnCurrencyChanged;

        private void Awake()
        {
            Gold = _gameConfig.StartingGold;
        }

        private void Start()
        {
            OnCurrencyChanged?.Invoke(Gold);
        }

        public bool CanAfford(int amount)
        {
            return Gold >= amount;
        }

        public bool TrySpend(int amount)
        {
            if(CanAfford(amount))
            {
                Gold -= amount;
                OnCurrencyChanged?.Invoke(Gold);
                return true;
            }

            return false;
        }

        public void AddCurrency(int amount)
        {
            Gold += amount;
            OnCurrencyChanged?.Invoke(Gold);
        }

        public void SetCurrency(int amount)
        {
            Gold = Mathf.Max(0, amount);
            OnCurrencyChanged?.Invoke(Gold);
        }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_gameConfig == null)
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:GameConfig");
            if (guids.Length > 0)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                _gameConfig = UnityEditor.AssetDatabase.LoadAssetAtPath<GameConfig>(path);
            }
        }
    }
#endif
    }
}