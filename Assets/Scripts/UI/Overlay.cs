using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Overlay : MonoBehaviour {
        
        [SerializeField] private Image overlay;
        
        private void Awake() {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
            overlay.enabled = false;
        }

        public void PlayFlash(Color color, float duration, Ease easeIn, Ease easeOut, Action onComplete = null) {
            overlay.enabled = true;
            var originalColor = overlay.color;
            
            overlay.DOColor(color, duration * 0.5f).SetEase(easeIn).OnComplete(() => {
                overlay.DOColor(originalColor, duration * 0.5f).SetEase(easeOut).OnComplete(() => {
                    onComplete?.Invoke();
                });
            }); 
        }
    }
}
