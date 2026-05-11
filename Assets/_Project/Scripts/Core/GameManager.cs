using System;
using UnityEngine;
using WheelOfFortune.Reward;
using WheelOfFortune.Wheel;
using WheelOfFortune.Zone;

namespace WheelOfFortune.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private WheelController _wheelController;
        [SerializeField] private ZoneController _zoneController;
        [SerializeField] private RewardBag _rewardBag;

        public GameState CurrentState { get; private set; } = GameState.Booting;

        public event Action<GameState> OnStateChanged;

        private void Awake()
        {
            _wheelController.OnSpinStarted += HandleSpinStarted;
            _wheelController.OnSpinResolved += HandleSpinResolved;

            _zoneController.OnPlayerCashedOut += HandleCashout;
            _zoneController.OnPlayerBombed += HandleBombed;
            _zoneController.OnPlayerGaveUp += HandleGiveUp;
            _zoneController.OnPlayerRevived += HandleRevive;
        }

        private void Start()
        {
            ChangeState(GameState.ReadyToSpin);
        }

        private void OnDestroy()
        {
            _wheelController.OnSpinStarted -= HandleSpinStarted;
            _wheelController.OnSpinResolved -= HandleSpinResolved;
            _zoneController.OnPlayerCashedOut -= HandleCashout;
            _zoneController.OnPlayerBombed -= HandleBombed;
            _zoneController.OnPlayerGaveUp -= HandleGiveUp;
            _zoneController.OnPlayerRevived -= HandleRevive;
        }

        public bool RequestSpin()
        {
            if (CurrentState != GameState.ReadyToSpin) return false;
            _wheelController.Spin();
            return true;
        }

        public bool RequestLeave()
        {
            if (CurrentState != GameState.ReadyToSpin) return false;
            if (!_zoneController.CurrentZoneType.AllowsLeaving()) return false;
            _zoneController.Cashout();
            return true;
        }

        public bool RequestGiveUp()
        {
            if (CurrentState != GameState.ShowingBombPanel) return false;
            _zoneController.GiveUp();
            return true;
        }

        public bool RequestRevive()
        {
            if (CurrentState != GameState.ShowingBombPanel) return false;
            _zoneController.Revive();
            return true;
        }

        public bool RequestContinue()
        {
            if (CurrentState != GameState.ShowingCashoutPanel) return false;
            ChangeState(GameState.ReadyToSpin);
            return true;
        }


        private void HandleSpinStarted()
        {
            ChangeState(GameState.Spinning);
        }

        private void HandleSpinResolved()
        {
            ChangeState(GameState.ResolvingReward);
            ChangeState(GameState.ReadyToSpin);
        }


        private void HandleBombed()
        {
            ChangeState(GameState.ShowingBombPanel);
        }

        private void HandleCashout()
        {
            ChangeState(GameState.ShowingCashoutPanel);
        }

        private void HandleGiveUp()
        {
            ChangeState(GameState.ReadyToSpin);
        }

        private void HandleRevive()
        {
            ChangeState(GameState.ReadyToSpin);
        }

        private void ChangeState(GameState next)
        {
            if (CurrentState == next) return;

            GameState previous = CurrentState;
            CurrentState = next;

            UnityEngine.Debug.Log($"[GameManager] State: {previous} -> {next}");
            OnStateChanged?.Invoke(next);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_wheelController == null)
            {
                _wheelController = FindObjectOfType<WheelController>(true);
            }
            if (_zoneController == null)
            {
                _zoneController = FindObjectOfType<ZoneController>(true);
            }
            if (_rewardBag == null)
            {
                _rewardBag = FindObjectOfType<RewardBag>(true);
            }
        }
#endif
    }
}