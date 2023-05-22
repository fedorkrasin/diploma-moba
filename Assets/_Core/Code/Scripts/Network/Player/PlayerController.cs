using Unity.Netcode;
using UnityEngine;

namespace Core.Network.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float _speed = 3;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                enabled = false;
            }
        }

        private void Update()
        {
            var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            transform.position += dir * (_speed * Time.deltaTime);
            Debug.Log(dir);
        }
    }
}