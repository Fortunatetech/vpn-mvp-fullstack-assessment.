using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using VPNMvp.ViewModels;

namespace VPNMvp.Views
{
    public partial class NodeListView : UserControl
    {
        public NodeListView()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<NodeListViewModel>();
        }
    }
}
