using API_Waylan_Origin.DTOs.PedidosDto;

namespace API_Waylan_Origin.Interfaces.Pedidos
{
    public interface IPedidoService
    {
        Task<PedidoReadDto> CrearPedido(int usuarioId, PedidoCreateDto pedidoDto);
    }
}
