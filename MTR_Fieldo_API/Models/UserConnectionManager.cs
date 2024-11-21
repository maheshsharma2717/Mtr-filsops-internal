using System.Collections.Concurrent;

namespace MTR_Fieldo_API.Models
{
    public class UserConnectionManager
    {
        private readonly ConcurrentDictionary<int, string> _userConnections = new ConcurrentDictionary<int, string>();

        public static int userCount = 1;

        public void AddConnection(int userId, string connectionId)
        {
            _userConnections.TryAdd(userId, connectionId);
        }

        public void RemoveConnection(string connectionId)
        {
            foreach (var (userId, connId) in _userConnections)
            {
                if (connId == connectionId)
                {
                    _userConnections.TryRemove(userId, out _);
                    break;
                }
            }
        }

        public string GetConnectionId(int userId)
        {
            _userConnections.TryGetValue(userId, out string connectionId);
            return connectionId;
        }
        public IEnumerable<string> GetAllConnectionIds(int userId)
        {
            List<string> connectionIds = new List<string>();

            foreach (var (uid, connId) in _userConnections)
            {
                if (uid == userId)
                {
                    connectionIds.Add(connId);
                }
            }

            return connectionIds;
        }
    }
}
