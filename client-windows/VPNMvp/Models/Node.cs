namespace VPNMvp.Models
{
public class Node
{
public string Name { get; set; } = "";
public string Country { get; set; } = "";
public int LatencyMs { get; set; }
public string PublicKey { get; set; } = "";
public string EndpointIp { get; set; } = "";
}
}