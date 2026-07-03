using API_Waylan_Origin.DTOs.NotasDto;
using System.Runtime.ConstrainedExecution;

namespace API_Waylan_Origin.Interfaces.Notas
{
    public interface INotaService
    {
        //CREAR
        Task<NotaReadDto> CrearNota(NotaCreateDto notaCreate);

        //LEER
        Task<IEnumerable<NotaReadDto>> ListarNotas();

        //ACTUALIZAR
        Task<NotaReadDto> ActualizarNota(int Id, NotaUpdateDto notaUpdate);
    }
}
