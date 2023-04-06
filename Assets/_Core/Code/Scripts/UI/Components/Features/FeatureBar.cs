using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Components.Features
{
    public class FeatureBar : MonoBehaviour
    {
        [SerializeField] private Image _foreground;
        [SerializeField] private float _reduceSpeed;

        private float _value = 1;

        private void Update()
        {
            UpdateVisual();
        }
        
        public void UpdateValue(float currentHealth, float maxHealth)
        {
            _value = currentHealth / maxHealth;
        }

        private void UpdateVisual()
        {
            if (Math.Abs(_foreground.fillAmount - _value) > 0.01f)
            {
                _foreground.fillAmount = Mathf.MoveTowards(_foreground.fillAmount, _value, _reduceSpeed * Time.deltaTime);
            }
        }
    }
}