using apiCarreras.DTOs;
using apiCarreras.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;

namespace apiCarreras.Services
{
    public class SimuladorService(ILogger<SimuladorService> logger, IHubContext<CarrerasSimuladasHub> hubContext) : ISimuladorService
    {
        private readonly ConcurrentDictionary<int, CancellationTokenSource> _simulaciones = new();
        private readonly ILogger<SimuladorService> _logger = logger;
        private readonly Random _random = new();
        private readonly IHubContext<CarrerasSimuladasHub> _hubContext = hubContext;

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
                    Console.WriteLine("///////////////////////////// INFO DE CARRERAS /////////////////////////////");

                    string json = JsonSerializer.Serialize(carrera);
                    Console.WriteLine(json);
                    Console.WriteLine("///////////////////////////// INFO DE CARRERAS /////////////////////////////");
                    Console.WriteLine($" Iniciando simulación para: {carrera.Nombre}");

                    // Validaciones de null
                    if (carrera.PuntosDeControl == null || carrera.PuntosDeControl.Count == 0)
                    {
                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene puntos de control");
                        return;
                    }

                    if (carrera.Registros == null || carrera.Registros.Count == 0)
                    {
                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene corredores");
                        return;
                    }

                    // Reasignar números a los puntos de control
                    var alrevez = carrera.PuntosDeControl.AsEnumerable().Reverse().ToList();
                    int token = carrera.PuntosDeControl.Count;
                    Console.WriteLine(" --------------------------------------------------------------");
                    Console.WriteLine($" Count de ptos " + token);
                    Console.WriteLine(" --------------------------------------------------------------");
                    int token2 = 0;

                    foreach (var ptos in alrevez)
                    {
                        ptos.NumeroEnCarrera = token - token2;
                        token2++;
                    }

                    carrera.PuntosDeControl = [.. alrevez.AsEnumerable().Reverse()];

                    _logger.LogInformation(" Entrando al bucle principal de simulación para {Nombre}", carrera.Nombre);

                    while (!cts.Token.IsCancellationRequested)
                    {
                        _logger.LogInformation(" Iteración del bucle de {Nombre}", carrera.Nombre);

                        await Task.Delay(TimeSpan.FromSeconds(2), cts.Token);

                        // Validar que los registros no sean null
                        if (carrera.Registros == null || carrera.Registros.Count == 0)
                        {
                            _logger.LogWarning("No hay registros disponibles en la carrera {Nombre}", carrera.Nombre);
                            continue;
                        }

                        var index = _random.Next(carrera.Registros.Count);
                        var registro = carrera.Registros[index];

                        // Validar que el registro y el corredor no sean null
                        if (registro?.Corredor == null)
                        {
                            _logger.LogWarning("Registro o corredor nulo en índice {Index}", index);
                            continue;
                        }

                        int avance = _random.Next(registro.Distancia, registro.Distancia + 100);
                        registro.Distancia = avance;

                        double kmtrsPunto = 0;
                        foreach (var ptos in carrera.PuntosDeControl)
                        {
                            if (ptos.Distancia < avance && registro.PuntoControl < ptos.NumeroEnCarrera)
                            {
                                registro.PuntoControl = ptos.NumeroEnCarrera;
                                kmtrsPunto = ptos.Distancia;
                                registro.HoraAvance = DateTime.UtcNow;
                                break;
                            }
                        }

                        var mensaje = $" {registro.Corredor.NombreCompleto} avanzó a {registro.Distancia}m (Punto {registro.PuntoControl}) en {carrera.Nombre}";
                        Console.WriteLine(mensaje);

                        // Emitir actualización por SignalR
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
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Simulación de carrera {Nombre} cancelada", carrera.Nombre);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, " Error en la simulación de la carrera {Nombre}", carrera.Nombre);
                    Console.WriteLine($" Error en simulación de {carrera.Nombre}: {ex.Message}");
                }
                finally
                {
                    _simulaciones.TryRemove(carrera.Id, out _);
                }
            }, cts.Token);
        }


        public void IniciarSimulacion_ElectricBoogaloo(CarreraDTO carrera)
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
                    Console.WriteLine("///////////////////////////// INFO DE CARRERAS /////////////////////////////");

                    string json = JsonSerializer.Serialize(carrera);
                    Console.WriteLine(json);
                    Console.WriteLine("///////////////////////////// INFO DE CARRERAS /////////////////////////////");
                    Console.WriteLine($" Iniciando simulación para: {carrera.Nombre}");

                    if (carrera.PuntosDeControl == null || carrera.PuntosDeControl.Count == 0)
                    {

                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene puntos de control");

                    }

                    if (carrera.Registros == null || carrera.Registros.Count == 0)
                    {

                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene corredores");

                    }

                    // Reasignar números a los puntos de control
                    var alrevez = carrera.PuntosDeControl.AsEnumerable().Reverse().ToList();
                    int token = carrera.PuntosDeControl.Count;
                    Console.WriteLine(" --------------------------------------------------------------");
                    Console.WriteLine($" Count de ptos " + token);
                    Console.WriteLine(" --------------------------------------------------------------");
                    int token2 = 0;

                    foreach (var ptos in alrevez)
                    {
                        ptos.numeroEnCarrera = token - token2;
                        token2++;
                    }

                    carrera.PuntosDeControl = alrevez.AsEnumerable().Reverse().ToList();


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
                        foreach (var ptos in carrera.PuntosDeControl)
                        {


                            if (ptos.Distancia < avance && registro.pntoControl < ptos.numeroEnCarrera)
                            {
                                registro.pntoControl = ptos.numeroEnCarrera;
                                kmtrsPunto = ptos.Distancia;
                                registro.HoraAvance = DateTime.UtcNow;
                                registro.Tiempo = DateTime.UtcNow - carrera.HoraInicio;

                                string registroTiempoEnformato = $"{(int)registro.Tiempo.TotalHours:D2}:{registro.Tiempo.Minutes:D2}";
                               
                                // break;
                                //gestion de posiciones

                                //  foreach (var regi in carrera.Registros)
                                // {
                                //   if (regi.)
                                //    {

                                //    }
                                //  }

                                await _hubContext.Clients.Group($"Registro-{registro.Id}")
                                .SendAsync("CorredorActualizado", new
                                {
                                    carreraId = carrera.Id,
                                    carreraNombre = carrera.Nombre,
                                    corredorId = registro.Corredor.Id,
                                    corredorNombre = registro.Corredor.NombreCompleto,
                                    posicionCarrera = registro.PosicionEnCarrera,
                                    tiempo = registroTiempoEnformato,
                                    kilometro = kmtrsPunto
                            });


                            }
                        }


                        var mensaje = $" {registro.Corredor.NombreCompleto} avanzó a {registro.distancia}m (Punto {registro.pntoControl}) en {carrera.Nombre}";
                        Console.WriteLine(mensaje);


                      
                        
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
