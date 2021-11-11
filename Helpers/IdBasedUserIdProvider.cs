using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

public class IdBasedUserIdProvider : IUserIdProvider
{
    public virtual string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
    }
}