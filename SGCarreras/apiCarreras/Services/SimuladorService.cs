using apiCarreras.DTOs;
using apiCarreras.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;

namespace apiCarreras.Services
{
    public class SimuladorService(ILogger<SimuladorService> logger, IHubContext<CarrerasSimuladasHub> hubContext, httpService httpServicee) : ISimuladorService
    {
        private readonly ConcurrentDictionary<int, CancellationTokenSource> _simulaciones = new();
        private readonly ILogger<SimuladorService> _logger = logger;
        private readonly Random _random = new();
        private readonly IHubContext<CarrerasSimuladasHub> _hubContext = hubContext;
        private readonly Dictionary<int, RegistroDTO> _estadoActual = new();
        private readonly ConcurrentDictionary<int, Dictionary<int, GanaDoorDTO>> _ganaDoors = new();
        private readonly httpService _httpService = httpServicee;
      
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
            _ganaDoors[carrera.Id] = new Dictionary<int, GanaDoorDTO>();
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

                    if (carrera.Inscripciones == null || carrera.Inscripciones.Count == 0)
                    {

                        Console.WriteLine($" Carrera {carrera.Nombre} no tiene corredores");

                    }

                    // Reasignar números a los puntos de control
                    var alrevez = carrera.PuntosDeControl.AsEnumerable().Reverse().ToList();
                    int token = carrera.PuntosDeControl.Count;
                    int countCorredores = carrera.Inscripciones.Count();
                    Console.WriteLine(" --------------------------------------------------------------");
                    Console.WriteLine($" Count de ptos " + token);
                    Console.WriteLine(" --------------------------------------------------------------");
                    int token2 = 0;

                    foreach (var ptos in alrevez)
                    {
                        ptos.NumeroEnCarrera = token - token2;
                        token2++;
                    }

                    carrera.PuntosDeControl = alrevez.AsEnumerable().Reverse().ToList();


                    _logger.LogInformation(" Entrando al bucle principal de simulación para {Nombre}", carrera.Nombre);

                    while (!cts.Token.IsCancellationRequested)
                    {
                        _logger.LogInformation(" Iteración del bucle de {Nombre}", carrera.Nombre);

                        await Task.Delay(TimeSpan.FromSeconds(2), cts.Token);

                        var index = _random.Next(carrera.Inscripciones.Count);
                        var registro = carrera.Inscripciones[index];

                     //   double avance = _random.NextDouble(registro.Distancia, registro.Distancia + 100);
                      //  

                        var rnd = new Random();
                        double min = registro.Distancia;
                        double max = registro.Distancia + 0.10;
                        
                        double valor = _random.NextDouble() * (max - min) + min;
                        valor = Math.Round(valor, 2);
                        double parteDecimal = 0;
                        {
                            parteDecimal = valor - Math.Floor(valor);
                            if (parteDecimal >= 0.10)
                            {
                                valor = valor + 1;
                                valor = valor - 0.10;
                                parteDecimal = valor - Math.Floor(valor);
                            }
                        } while (parteDecimal >= 10) ;

                        double avance = valor;
                        registro.Distancia = avance;
                        double kmtrsPunto = 0;
                        registro.HoraAvance = DateTime.UtcNow;
                        registro.Tiempo = DateTime.UtcNow - carrera.HoraInicio;

                        string registroTiempoEnformato = registro.Tiempo.ToString(@"hh\:mm\:ss");

                        foreach (var ptos in carrera.PuntosDeControl)
                        {
                            var ganaDoorDict = _ganaDoors[carrera.Id];

                            if (carrera.PuntosDeControl.Last().NumeroEnCarrera == ptos.NumeroEnCarrera && avance >= carrera.PuntosDeControl.Last().Distancia && !ganaDoorDict.ContainsKey(registro.Id))
                            {
                                registro.pntoControl = ptos.NumeroEnCarrera;
                                kmtrsPunto = ptos.Distancia;
                                

                                GanaDoorDTO ganaDoor = new GanaDoorDTO();
                                ganaDoor.inscripcionId = registro.Id;
                                ganaDoor.numeroEnCarrera = ganaDoorDict.Count + 1;
                                ganaDoor.tiempoDeFinalizacion = registroTiempoEnformato;
                                ganaDoor.numeroDelCorredor = registro.NumeroEnCarrera;

                                ganaDoorDict[registro.Id] = ganaDoor;
                                Console.WriteLine(" El corredor " + registro.Corredor.NombreCompleto + "llego a la meta como numero: " + ganaDoor.numeroEnCarrera);
                                Console.WriteLine(")()=(=)(=(=)(=)(=)(=)(=)()=(=)(=)(=)(=)(=)(=)(=)(" + _ganaDoors.Count + ")()=(=)(=(=)(=)(=)(=)(=)()=(=)(=)(=)(=)(=)(=)(=)(");

                                

                               // regiMandar.mensake = "mensake";

                               

                                await _hubContext.Clients.Group($"Registro-{registro.Id}")
                               .SendAsync("CorredorActualizado", new
                               {
                                   mensake = "Se llego a la meta",
                                   registroId = ganaDoor.inscripcionId,
                                   numeroEnCarrera = ganaDoor.numeroEnCarrera,
                                   tiempoDeFinalizacion = ganaDoor.tiempoDeFinalizacion,
                                   numeroDelCorredor = ganaDoor.numeroDelCorredor,
                                   //datos del otro 
                                   carreraId = carrera.Id,
                                   carreraNombre = carrera.Nombre,
                                   corredorId = registro.Corredor.Id,
                                   corredorNombre = registro.Corredor.NombreCompleto,
                                   posicionCarrera = registro.PosicionEnCarrera,
                                   tiempo = registroTiempoEnformato,
                                   kilometro = kmtrsPunto

                               });

                                RegistroDTO regiMandar = new RegistroDTO();

                                regiMandar.PosicionEnCarrera = ganaDoor.numeroEnCarrera;
                                regiMandar.tiempoperoenstingayuda = registroTiempoEnformato;
                                regiMandar.Distancia = kmtrsPunto;
                                regiMandar.Id = registro.Id;
                                _estadoActual[registro.Id] = regiMandar;
                                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
                                Console.WriteLine("countCorredores = " + countCorredores + "/_ganaDoors.Count() = " + _ganaDoors[carrera.Id].Count);
                                Console.WriteLine("/Carrera = " + carrera.Nombre);
                                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");



                                if (countCorredores == _ganaDoors[carrera.Id].Count)
                                {
                                    //metodo para guardoar los datos de la carrera
                                    var lista = _ganaDoors[carrera.Id].Values.ToList();
                                    await _httpService.NotificarCarreraTerminada(lista);

                                    this.DetenerSimulacion(carrera.Id);
                                }
                                
                            }
                            else if (ptos.Distancia < avance && registro.pntoControl < ptos.NumeroEnCarrera && !_ganaDoors.ContainsKey(registro.Id))
                            {
                                registro.pntoControl = ptos.NumeroEnCarrera;
                                kmtrsPunto = ptos.Distancia;
                                

                                RegistroDTO regiMandar = new RegistroDTO();

                                regiMandar.PosicionEnCarrera = registro.PosicionEnCarrera;
                                regiMandar.tiempoperoenstingayuda = registroTiempoEnformato;
                                regiMandar.Distancia = kmtrsPunto;
                                regiMandar.Id = registro.Id;

                                _estadoActual[registro.Id] = regiMandar;

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


                        var mensaje = $" {registro.Corredor.NombreCompleto} avanzó a {registro.Distancia}m (Punto {registro.pntoControl}) en {carrera.Nombre}";
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




        public void DetenerSimulacion(int id)
        {
            if (_simulaciones.TryRemove(id, out var cts))
            {
                cts.Cancel();
                _logger.LogInformation("Simulación de carrera {id} detenida", id);
            }
        }

        public object? ObtenerEstadoActual(int registroId)
        {
            if (_estadoActual.TryGetValue(registroId, out var registro))
            {
                return new
                {
                    numeroEnCarrera = registro.PosicionEnCarrera,
                    tiempo = registro.tiempoperoenstingayuda,
                    kilometro = registro.Distancia,
                        Id = registro.Id

            }
            ;
            }
            return null;
        }


    }
}
