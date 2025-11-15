using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace apiCarreras.Hubs
{
    public class CarrerasSimuladasHub : Hub
    {
        public async Task UnirseACarrera(int carreraId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Carrera-{carreraId}");
            await Clients.Caller.SendAsync("Conectado", $"Unido a carrera {carreraId}");
        }
        public async Task UnirseACorredorCorriendoceUnaCarreraCarrerosa(int registroId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Registro-{registroId}");
            await Clients.Caller.SendAsync("Conectado", $"Unido al registro {registroId}, ahora, acose al corredor a gusto.");
        }
    }
}