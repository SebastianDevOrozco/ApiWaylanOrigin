using API_Waylan_Origin.DTOs.PedidosDto;
using API_Waylan_Origin.Enums;

namespace API_Waylan_Origin.Interfaces.Pedidos
{
    public interface IPedidoService
    {
        Task<PedidoReadDto> CrearPedido(int usuarioId, PedidoCreateDto pedidoDto);
        Task<IEnumerable<PedidoReadDto>> ListarPedidos(int usuarioId);

        Task<IEnumerable<PedidoReadAdminDto>> ListarTodosLosPedidos(); 
        Task<PedidoReadAdminDto> PedidoCodigo(string codigo);
        Task CambiarEstadoPedido(string codigo, EstadoPedido NuevoEstado);
    }
}
