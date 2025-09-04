using System.Windows;


namespace VPNMvp.Utils
{
public static class NotificationHelper
{
public static void Notify(string title, string message)
{
MessageBox.Show(message, title);
}
}
}