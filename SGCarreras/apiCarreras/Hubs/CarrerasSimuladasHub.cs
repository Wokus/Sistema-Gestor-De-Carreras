using apiCarreras.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace apiCarreras.Hubs
{
    public class CarrerasSimuladasHub : Hub
    {
        private readonly ISimuladorService _simulador;
        public CarrerasSimuladasHub(ISimuladorService simulador)
        {
            _simulador = simulador;
        }

        public async Task UnirseACarrera(int carreraId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Carrera-{carreraId}");
            await Clients.Caller.SendAsync("Conectado", $"Unido a carrera {carreraId}");
        }
        public async Task UnirseACorredorCorriendoceUnaCarreraCarrerosa(int registroId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Registro-{registroId}");
            await Clients.Caller.SendAsync("Conectado", $"Unido al registro {registroId}, ahora, acose al corredor a gusto.");

            var estado =  _simulador.ObtenerEstadoActual(registroId);

            var json = JsonSerializer.Serialize(estado, new JsonSerializerOptions
            {
                WriteIndented = true // hace el JSON más legible
            });

            Console.WriteLine("=== ESTADO ACTUAL ===");
            Console.WriteLine(json);
            Console.WriteLine("=====================");

            if (estado != null)
            {
                // Enviar estado inmediatamente
                await Clients.Caller.SendAsync("CorredorActualizado", estado);
            }

        }
    }
}
