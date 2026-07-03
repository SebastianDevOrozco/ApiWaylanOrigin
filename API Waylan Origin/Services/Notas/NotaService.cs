using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.NotasDto;
using API_Waylan_Origin.Interfaces.Notas;
using API_Waylan_Origin.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Waylan_Origin.Services.Notas
{
    public class NotaService : INotaService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public NotaService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<NotaReadDto> CrearNota(NotaCreateDto notaCreate)
        {
            var nuevaNota = _mapper.Map<Nota>(notaCreate);

            _appDbContext.Notas.Add(nuevaNota);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<NotaReadDto>(nuevaNota);
        }

        public async Task<IEnumerable<NotaReadDto>> ListarNotas()
        {
            var notas = await _appDbContext.Notas
                .ToListAsync();

            if(notas == null)
                return new List<NotaReadDto>();

            return _mapper.Map<IEnumerable<NotaReadDto>>(notas);
        }

        public async Task<NotaReadDto> ActualizarNota(int Id, NotaUpdateDto notaUpdate)
        {
            var nota = await _appDbContext.Notas
                .FirstOrDefaultAsync(n => n.id == Id);

            if(nota == null)
                throw new KeyNotFoundException($"La Nota con el ID {Id} No Existe");

            _mapper.Map(notaUpdate, nota);

            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<NotaReadDto>(nota);
        }
    }
}
