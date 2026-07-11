using API_Waylan_Origin.DTOs.Wompi;

namespace API_Waylan_Origin.Interfaces.Wompi
{
    public interface IPagosService
    {
        Task ProcesarNotificacionAsync(WompiWebHookDto notificacion);
    }
}
