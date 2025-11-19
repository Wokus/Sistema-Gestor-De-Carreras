using apiCarreras.DTOs;
using System.Text.Json;

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
            Console.WriteLine("GANADORES DESDE HTTPS SERVICE");
            Console.WriteLine("989898988888889898989898989989898998989898989898989999898989898989898989");
            string json = JsonSerializer.Serialize(ganadores);
            Console.WriteLine("989898988888889898989898989989898998989898989898989999898989898989898989");
            Console.WriteLine(json);

            var response = await _http.PostAsJsonAsync(
                "https://localhost:7118/apiController/Simulacion/terminarCarrera", ganadores);

            response.EnsureSuccessStatusCode();
        }

    }
}
