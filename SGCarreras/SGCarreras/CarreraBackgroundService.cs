using Microsoft.EntityFrameworkCore;
using SGCarreras.Data;
using SGCarreras.Models;
using static SGCarreras.Models.Estado;

namespace SGCarreras.Services
{
    public class CarreraBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CarreraBackgroundService> _logger;

        public CarreraBackgroundService(IServiceProvider serviceProvider, ILogger<CarreraBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Servicio de activación automática de carreras iniciado");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<SGCarrerasContext>();

                    var carrerasParaActivar = await context.Carrera
                        .Where(c => c.Estado == EstadoEnum.En_espera &&
                                    c.Fecha <= DateTime.Now)
                        .ToListAsync(stoppingToken);

                    if (carrerasParaActivar.Any())
                    {
                        foreach (var carrera in carrerasParaActivar)
                        {
                            carrera.Estado = EstadoEnum.Activo;
                            _logger.LogInformation($"Carrera '{carrera.Nombre}' (ID: {carrera.Id}) activada automáticamente. Fecha: {carrera.Fecha}");
                        }

                        await context.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation($"{carrerasParaActivar.Count} carreras activadas automáticamente");
                    }
                    else
                    {
                        _logger.LogDebug("No hay carreras pendientes de activación");
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el servicio de activación automática de carreras");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }

            _logger.LogInformation("Servicio de activación automática de carreras detenido");
        }
    }
}
