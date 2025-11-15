using apiCarreras.DTOs;

namespace apiCarreras.Services
{
    public interface ISimuladorService
    {

        void IniciarSimulacion(CarreraDTO carrera);

        void DetenerSimulacion(int carreraId);

        void IniciarSimulacion_ElectricBoogaloo(CarreraDTO carrera);
    }
}
