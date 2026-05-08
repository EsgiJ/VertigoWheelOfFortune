using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WheelOfFortune.Currency
{   
    public class CurrencyController : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _startingCurrency = 100;

        public int Gold { get; private set; }   

        public event Action<int> OnCurrencyChanged;

        private void Awake()
        {
            Gold = _startingCurrency;
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
    }
}