using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VPNMvp.Models;


namespace VPNMvp.Services
{
public class NodeService : INodeService
{
private readonly IHttpClientFactory _factory;


public NodeService(IHttpClientFactory factory)
{
_factory = factory;
}


public async Task<List<Node>> GetNodesAsync()
{
var client = _factory.CreateClient("Api");
var list = await client.GetFromJsonAsync<List<Node>>("/api/v1/nodes");
return list ?? new List<Node>();
}
}
}