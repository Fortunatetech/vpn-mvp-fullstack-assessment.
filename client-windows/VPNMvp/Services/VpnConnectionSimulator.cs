using System;
using System.Threading.Tasks;
using VPNMvp.Models;

namespace VPNMvp.Services
{
    public class VpnConnectionSimulator : IVpnConnectionService
    {
        private bool _isConnected = false;
        private Node? _currentNode;

        public bool IsConnected => _isConnected;
        public Node? ConnectedNode => _currentNode;

        public event Action<bool, Node?>? ConnectionStateChanged;

        public async Task<bool> ConnectAsync(Node node)
        {
            // Simulate connection steps
            await Task.Delay(700); // handshake
            await Task.Delay(300); // key exchange
            await Task.Delay(300); // finalize

            _isConnected = true;
            _currentNode = node;

            // notify UI
            ConnectionStateChanged?.Invoke(true, node);

            return true;
        }

        public async Task DisconnectAsync()
        {
            if (!_isConnected) return;

            // Simulate disconnect
            await Task.Delay(300);

            var prev = _currentNode;
            _isConnected = false;
            _currentNode = null;

            ConnectionStateChanged?.Invoke(false, prev);
        }
    }
}
