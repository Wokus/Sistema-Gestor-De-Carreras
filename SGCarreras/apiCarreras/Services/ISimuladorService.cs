using apiCarreras.DTOs;

namespace apiCarreras.Services
{
    public interface ISimuladorService
    {
        /// <summary>
        /// Inicia la simulación de una carrera.
        /// </summary>
        /// <param name="carrera">DTO de la carrera a simular.</param>
        void IniciarSimulacion(CarreraDTO carrera);

        /// <summary>
        /// Detiene la simulación en curso de una carrera específica.
        /// </summary>
        /// <param name="carreraId">Identificador de la carrera.</param>
        void DetenerSimulacion(int carreraId);
    }
}
