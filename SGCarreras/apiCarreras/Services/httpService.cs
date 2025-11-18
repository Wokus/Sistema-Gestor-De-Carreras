using apiCarreras.DTOs;

namespace apiCarreras.Services
{
    public class httpService
    {
        private readonly HttpClient _http;

        public httpService(HttpClient http)
        {
            _http = http;
        }

        public async Task NotificarCarreraTerminada(List<GanaDoorDTO> ganadores)
        {
            var response = await _http.PostAsJsonAsync(
                "https://localhost:7118/apiController/Simulacion/terminarCarrera", ganadores);

            response.EnsureSuccessStatusCode();
        }

    }
}
