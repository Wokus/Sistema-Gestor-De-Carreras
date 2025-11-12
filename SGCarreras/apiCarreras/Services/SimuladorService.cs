using apiCarreras.DTOs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using apiCarreras.Hubs;

namespace apiCarreras.Services
{
    public class SimuladorService : ISimuladorService
    {
        private readonly ConcurrentDictionary<int, CancellationTokenSource> _simulaciones = new();
        private readonly ILogger<SimuladorService> _logger;
        private readonly Random _random = new();
        private readonly IHubContext<CarrerasSimuladasHub> _hubContext;


        public SimuladorService(ILogger<SimuladorService> logger, IHubContext<CarrerasSimuladasHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public void IniciarSimulacion(CarreraDTO carrera)
        {
            if (_simulaciones.ContainsKey(carrera.Id))
            {
                _logger.LogWarning("La carrera {Id} ya está en simulación", carrera.Id);
                return;
            }

            carrera.HoraInicio = DateTime.UtcNow;
            var cts = new CancellationTokenSource();
            _simulaciones[carrera.Id] = cts;

            _ = Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation("Iniciando simulación para {Nombre}", carrera.Nombre);
                    Console.WriteLine($" Iniciando simulación para: {carrera.Nombre}");

                    if (carrera.PtosDeControl == null || carrera.PtosDeControl.Count == 0)
                    {
                        _logger.LogWarning("Carreraaaaa {Nombre} no tiene puntos de control", carrera.Nombre);
                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene puntos de control");
                       
                    }

                    if (carrera.Registros == null || carrera.Registros.Count == 0)
                    {
                        _logger.LogWarning("Carrera {Nombre} no tiene corredores", carrera.Nombre);
                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene corredores");
                      
                    }
                    _logger.LogWarning("Carreraaaaaaaaaaaaaaa");
                    // Reasignar números a los puntos de control
                    var alrevez = carrera.PtosDeControl.AsEnumerable().Reverse().ToList();
                    int token = carrera.PtosDeControl.Count;
                    int token2 = 0;

                    foreach (var ptos in alrevez)
                    {
                        ptos.numeroEnCarrera = token - token2;
                        token2++;
                    }

                    carrera.PtosDeControl = alrevez.AsEnumerable().Reverse().ToList();


                    _logger.LogInformation(" Entrando al bucle principal de simulación para {Nombre}", carrera.Nombre);

                    while (!cts.Token.IsCancellationRequested)
                    {
                        _logger.LogInformation(" Iteración del bucle de {Nombre}", carrera.Nombre);

                        await Task.Delay(TimeSpan.FromSeconds(2), cts.Token);

                        var index = _random.Next(carrera.Registros.Count);
                        var registro = carrera.Registros[index];

                        int avance = _random.Next(registro.distancia, registro.distancia + 100);
                        registro.distancia = avance;

                        double kmtrsPunto = 0;
                        foreach (var ptos in carrera.PtosDeControl)
                        {
                            if (ptos.Distancia < avance && registro.pntoControl < ptos.numeroEnCarrera)
                            {
                                registro.pntoControl = ptos.numeroEnCarrera;
                                kmtrsPunto = ptos.Distancia;
                                registro.HoraAvance = DateTime.UtcNow;
                                break;
                            }
                        }

                      
                        var mensaje = $" {registro.Corredor.NombreCompleto} avanzó a {registro.distancia}m (Punto {registro.pntoControl}) en {carrera.Nombre}";
                        Console.WriteLine(mensaje);

                        _logger.LogInformation(
                            "Carrera {Nombre}: {Corredor} avanza, distancia {distancia}",
                            carrera.Nombre,
                            registro.Corredor.NombreCompleto,
                            registro.distancia);

                        // 🔥 Emitir actualización por SignalR
                        await _hubContext.Clients.Group($"Carrera-{carrera.Id}")
                            .SendAsync("CarreraActualizada", new
                            {
                                carreraId = carrera.Id,
                                carreraNombre = carrera.Nombre,
                                corredorId = registro.Corredor.Id,
                                corredorNombre = registro.Corredor.NombreCompleto,
                                posicionCarrera = registro.PosicionEnCarrera,
                                tiempo = registro.HoraAvance,
                                kilometro = kmtrsPunto
                            });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, " Error en la simulación de la carrera {Nombre}", carrera.Nombre);
                    Console.WriteLine($" Error en simulación de {carrera.Nombre}: {ex.Message}");
                }
            }, cts.Token);
        }


        public void DetenerSimulacion(int carreraId)
        {
            if (_simulaciones.TryRemove(carreraId, out var cts))
            {
                cts.Cancel();
                _logger.LogInformation("Simulación de carrera {Id} detenida", carreraId);
            }
        }
    }
}
