using Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi.Models;

namespace WebUi.Hubs
{
    //https://docs.microsoft.com/en-us/aspnet/signalr/overview/advanced/dependency-injection

    public class TalkActive : Hub
    {
        private static readonly List<AccountModel> CurrentConnections = new List<AccountModel>();
        private readonly ISettingRepository _repoSetting;
        public TalkActive(ISettingRepository repoSetting)
        {
            _repoSetting = repoSetting;
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public async Task AddToGroup(AccountModel m)
        {
            m.ConnectionId = Context.ConnectionId;
            CurrentConnections.Add(m);
            await Groups.AddToGroupAsync(Context.ConnectionId, m.AccountSid);
        }

        public async Task RemoveFromGroup(AccountModel m)
        {
            var t = CurrentConnections
                .FirstOrDefault(z => z.ConnectionId == Context.ConnectionId);
            if (t != null) CurrentConnections.Remove(t);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, m.AccountSid);
        }

        public List<AccountModel> GetAllClient()
        {
            return CurrentConnections;
        }
    }
}
