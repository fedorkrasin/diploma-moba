using System.Threading.Tasks;
using UnityEngine;

namespace Core.Networking.Client
{
    public class ClientInstance : MonoBehaviour
    {
        private ClientService _clientService;
        
        public async Task CreateClient()
        {
            _clientService = new ClientService();
            await _clientService.InitAsync();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            _clientService?.Dispose();
        }
    }
}