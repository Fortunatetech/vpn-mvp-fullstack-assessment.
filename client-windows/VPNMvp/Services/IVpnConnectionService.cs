using System;
using System.Threading.Tasks;
using VPNMvp.Models;

namespace VPNMvp.Services
{
    public interface IVpnConnectionService
    {
        Task<bool> ConnectAsync(Node node);
        Task DisconnectAsync();
        bool IsConnected { get; }
        Node? ConnectedNode { get; }
        event Action<bool, Node?>? ConnectionStateChanged; // (isConnected, node)
    }
}
