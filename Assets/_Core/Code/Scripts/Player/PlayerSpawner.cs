using System;
using Zenject;
using Object = UnityEngine.Object;

namespace Core.Player
{
    public class PlayerSpawner
    {
        private readonly PlaceholderFactory<PlayerBehaviour> _playerFactory;
        
        public PlayerBehaviour Player { get; private set; }

        public PlayerSpawner(PlaceholderFactory<PlayerBehaviour> playerFactory)
        {
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
        }

        public PlayerBehaviour Spawn()
        {
            Player = _playerFactory.Create();
            return Player;
        }

        public bool TryDestroy()
        {
            bool isPlayerExists = Player;
            if (isPlayerExists) Object.Destroy(Player.gameObject);
            Player = null;
            return isPlayerExists;
        }
    }
}