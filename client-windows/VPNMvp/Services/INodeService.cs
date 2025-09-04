using System.Collections.Generic;
using System.Threading.Tasks;
using VPNMvp.Models;


namespace VPNMvp.Services
{
public interface INodeService
{
Task<List<Node>> GetNodesAsync();
}
}