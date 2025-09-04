using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;


namespace VPNMvp
{
public partial class App : Application
{
public static IServiceProvider Services { get; private set; } = null!;


protected override void OnStartup(StartupEventArgs e)
{
var services = new ServiceCollection();


// Register HttpClient with base address for mock backend
services.AddHttpClient("Api", c =>
{
c.BaseAddress = new Uri("http://localhost:4000");
});


// Register services and views/viewmodels
services.AddSingleton<Services.IAuthService, Services.AuthService>();
services.AddSingleton<Services.INodeService, Services.NodeService>();
services.AddSingleton<Services.IVpnConnectionService, Services.VpnConnectionSimulator>();


services.AddTransient<ViewModels.SignInViewModel>();
services.AddTransient<ViewModels.NodeListViewModel>();


services.AddTransient<Views.SignInView>();
services.AddTransient<Views.NodeListView>();


Services = services.BuildServiceProvider();


base.OnStartup(e);
}
}
}