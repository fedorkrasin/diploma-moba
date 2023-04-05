using Core.Characters;
using Core.Player.Stats;
using Core.Player.Systems;
using UnityEngine;

namespace Core.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ControllerSystem _controller;
        [SerializeField] private PlayerBehaviour _player;
        [SerializeField] private PlayerSpellsList _spellsList;
        
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;

        [SerializeField] private Character _characterType;
        private ICharacter _character;
        
        private SpellSystem _spells;
        private Transform _transform;
        private Vector3 _direction;

        private void OnEnable()
        {
            _spells = new SpellSystem(_spellsList);
            _transform = transform;
            _direction = _transform.forward;

            _character = CharacterConstructor.Get(_characterType);

            _controller.AttackButtonClicked += Attack;
            _controller.SpellButtonClicked += UseSpell;
            _controller.UltimateButtonClicked += UseUltimate;
        }

        private void OnDisable()
        {
            _controller.UltimateButtonClicked -= UseUltimate;
            _controller.SpellButtonClicked -= UseSpell;
            _controller.AttackButtonClicked -= Attack;
        }

        private void FixedUpdate()
        {
            if (_controller.IsMoving)
            {
                Move();
                Rotate();
            }
        }

        private void Move()
        {
            var moveDirection = new Vector3(_controller.MovementValue.x, 0f, _controller.MovementValue.y);
            
            if (moveDirection.magnitude > 0f)
            {
                _direction = moveDirection;
            }

            _rigidbody.MovePosition(_transform.position + _moveSpeed * Time.deltaTime * moveDirection);
        }

        private void Rotate()
        {
            _transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_direction), _rotationSpeed * Time.deltaTime);
        }

        private void Attack()
        {
            Debug.Log("Attack");
        }

        private void UseSpell()
        {
            if (_player.Mana.Spend(_spellsList.Simple.ManaCost))
            {
                _character.UseSpell();
                _controller.SpellButton.SetCooldown(_spellsList.Simple.Cooldown);
            }
            else
            {
                Debug.Log("Not enough mana!");
            }
        }

        private void UseUltimate()
        {
            if (_player.Mana.Spend(_spellsList.Ultimate.ManaCost))
            {
                _character.UseUltimate();
                _controller.UltimateButton.SetCooldown(_spellsList.Ultimate.Cooldown);
            }
            else
            {
                Debug.Log("Not enough mana!");
            }
        }
    }
}
