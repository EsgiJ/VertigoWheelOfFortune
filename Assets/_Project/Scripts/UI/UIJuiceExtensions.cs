using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace WheelOfFortune.UI
{
    public static class UIJuiceExtensions
    {
        public static Tween PlayPressFeedback(this Component target, float targetScale = 0.92f)
        {
            if (target == null) return null;

            var t = target.transform;
            t.DOKill();
            t.localScale = Vector3.one;

            Sequence seq = DOTween.Sequence().SetLink(target.gameObject);
            seq.Append(t.DOScale(targetScale, 0.06f).SetEase(Ease.OutQuad));
            seq.Append(t.DOScale(1f, 0.12f).SetEase(Ease.OutBack));
            return seq;
        }

        public static Tween PlayPulse(this Component target, float punchScale = 1.25f, float duration = 0.25f)
        {
            if (target == null) return null;

            var t = target.transform;
            t.DOKill();
            t.localScale = Vector3.one;

            Sequence seq = DOTween.Sequence().SetLink(target.gameObject);
            seq.Append(t.DOScale(punchScale, duration * 0.4f).SetEase(Ease.OutQuad));
            seq.Append(t.DOScale(1f, duration * 0.6f).SetEase(Ease.OutBack));
            return seq;
        }

        public static Tween PlayPopIn(this Component target, float duration = 0.3f, Ease ease = Ease.OutBack)
        {
            if (target == null) return null;

            var t = target.transform;
            t.DOKill();
            t.localScale = Vector3.zero;

            return t.DOScale(1f, duration).SetEase(ease).SetLink(target.gameObject).SetUpdate(true);
        }

        public static Tween PlayPopOut(this Component target, float duration = 0.2f, Action onComplete = null)
        {
            if (target == null) return null;

            var t = target.transform;
            t.DOKill();

            return t.DOScale(0f, duration)
                    .SetEase(Ease.InBack)
                    .SetLink(target.gameObject)
                    .SetUpdate(true)
                    .OnComplete(() => onComplete?.Invoke());
        }

        public static Tween PlayFade(this CanvasGroup canvasGroup, float targetAlpha, float duration = 0.25f)
        {
            if (canvasGroup == null) return null;

            canvasGroup.DOKill();
            return canvasGroup.DOFade(targetAlpha, duration)
                              .SetLink(canvasGroup.gameObject)
                              .SetUpdate(true);
        }

        public static Tween PlayShake(this Component target, float duration = 0.5f, float strength = 12f)
        {
            if (target == null) return null;

            var t = target.transform;
            t.DOKill();
            t.localScale = Vector3.one;
            return t.DOShakePosition(duration, strength, 25, 90, false, true)
                    .SetLink(target.gameObject)
                    .SetUpdate(true);
        }
    }
}