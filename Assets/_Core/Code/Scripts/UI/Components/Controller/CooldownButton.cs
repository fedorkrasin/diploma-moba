using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.Components.Controller
{
    public class CooldownButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }

        [SerializeField] private Image _cooldownFade;

        public void SetCooldown(float duration)
        {
            StartCoroutine(Animation());
            
            IEnumerator Animation()
            {
                Button.interactable = false;
                var timer = duration;
                while (timer > 0)
                {
                    _cooldownFade.fillAmount = timer / duration;
                    timer -= Time.unscaledDeltaTime;
                    yield return new WaitForSeconds(Time.unscaledDeltaTime);
                }

                Button.interactable = true;
            }
        }
    }
}