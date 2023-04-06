using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly Dictionary<PlayerAnimationTrigger, int> Triggers = new()
        {
            { PlayerAnimationTrigger.Attack, Animator.StringToHash(PlayerAnimationTrigger.Attack.ToString()) },
            { PlayerAnimationTrigger.Hit, Animator.StringToHash(PlayerAnimationTrigger.Hit.ToString()) },
            { PlayerAnimationTrigger.Death, Animator.StringToHash(PlayerAnimationTrigger.Death.ToString()) },
        };

        private static readonly Dictionary<PlayerAnimationBool, int> Bools = new()
        {
            { PlayerAnimationBool.IsRunning, Animator.StringToHash(PlayerAnimationBool.IsRunning.ToString()) },
        };
        
        [SerializeField] private Animator _animator;
        
        public void TriggerAnimation(PlayerAnimationTrigger key)
        {
            foreach (var mappingValue in Triggers.Values)
            {
                _animator.ResetTrigger(mappingValue);
            }

            _animator.SetTrigger(Triggers[key]);
        }

        public void ToggleAnimation(PlayerAnimationBool key, bool value)
        {
            if (_animator.GetBool(Bools[key]) != value)
            {
                _animator.SetBool(Bools[key], value);
            }
        }

        public enum PlayerAnimationTrigger
        {
            Attack,
            Hit,
            Death,
        }

        public enum PlayerAnimationBool
        {
            IsRunning
        }
    }
}