using System;
using Core.Characters;
using Core.Characters.Data;
using Object = UnityEngine.Object;

namespace Core.Player
{
    public class PlayerSpawner
    {
        private readonly CharactersList _charactersList;
        
        public PlayerBehaviour Player { get; private set; }

        public PlayerSpawner(CharactersList charactersList)
        {
            _charactersList = charactersList ? charactersList : throw new ArgumentNullException(nameof(charactersList));
        }

        public PlayerBehaviour Spawn(CharacterType type)
        {
            var data = _charactersList.Get(type);
            var character = Object.Instantiate(data.CharacterPrefab);
            Player = character.GetComponent<PlayerBehaviour>();
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