using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VPNMvp.Models;
using VPNMvp.Services;

namespace VPNMvp.ViewModels
{
    public class NodeListViewModel : BaseViewModel
    {
        private readonly INodeService _nodeService;
        private readonly IVpnConnectionService _vpnService;

        public NodeListViewModel(INodeService nodeService, IVpnConnectionService vpnService)
        {
            _nodeService = nodeService;
            _vpnService = vpnService;
            Nodes = new ObservableCollection<Node>();
            ConnectCommand = new RelayCommand(async p => await ToggleConnectionAsync(p as Node));

            _vpnService.ConnectionStateChanged += (isConnected, node) =>
            {
                ConnectionStatus = isConnected ? $"Connected to {node?.Name}" : "Disconnected";
                IsConnected = isConnected;
                ConnectedNode = node;
                Raise(nameof(ConnectionStatus));
                Raise(nameof(IsConnected));
                Raise(nameof(ConnectedNode));
                RaiseCanExecuteChanged();
            };

            Task.Run(LoadNodesAsync);
        }

        public ObservableCollection<Node> Nodes { get; }

        private string _connectionStatus = "Disconnected";
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set { _connectionStatus = value; Raise(nameof(ConnectionStatus)); }
        }

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; Raise(nameof(IsConnected)); }
        }

        private Node? _connectedNode;
        public Node? ConnectedNode
        {
            get => _connectedNode;
            set { _connectedNode = value; Raise(nameof(ConnectedNode)); }
        }

        public ICommand ConnectCommand { get; }

        private async Task LoadNodesAsync()
        {
            try
            {
                var list = await _nodeService.GetNodesAsync();
                App.Current.Dispatcher.Invoke(() =>
                {
                    Nodes.Clear();
                    foreach (var n in list) Nodes.Add(n);
                });
            }
            catch (Exception ex)
            {
                App.Current.Dispatcher.Invoke(() => MessageBox.Show($"Failed to load nodes: {ex.Message}", "Error"));
            }
        }

        private async Task ToggleConnectionAsync(Node? node)
        {
            if (node == null) return;

            try
            {
                // If not connected -> connect to requested node
                if (!_vpnService.IsConnected)
                {
                    ConnectionStatus = $"Connecting to {node.Name}...";
                    Raise(nameof(ConnectionStatus));
                    await _vpnService.ConnectAsync(node);
                    return;
                }

                // If connected to the same node -> disconnect
                if (_vpnService.IsConnected && _vpnService.ConnectedNode != null && _vpnService.ConnectedNode.Name == node.Name)
                {
                    ConnectionStatus = "Disconnecting...";
                    Raise(nameof(ConnectionStatus));
                    await _vpnService.DisconnectAsync();
                    return;
                }

                // If connected to a different node -> switch (disconnect then connect)
                if (_vpnService.IsConnected && _vpnService.ConnectedNode != null && _vpnService.ConnectedNode.Name != node.Name)
                {
                    ConnectionStatus = $"Switching to {node.Name}...";
                    Raise(nameof(ConnectionStatus));
                    await _vpnService.DisconnectAsync();
                    await Task.Delay(200); // small pause
                    await _vpnService.ConnectAsync(node);
                    return;
                }
            }
            catch (Exception ex)
            {
                App.Current.Dispatcher.Invoke(() => MessageBox.Show($"Connection error: {ex.Message}", "Error"));
                ConnectionStatus = _vpnService.IsConnected ? $"Connected to {_vpnService.ConnectedNode?.Name}" : "Disconnected";
                Raise(nameof(ConnectionStatus));
            }
        }

        private void RaiseCanExecuteChanged()
        {
            if (ConnectCommand is RelayCommand rc)
            {
                rc.RaiseCanExecuteChanged();
            }
        }
    }
}
