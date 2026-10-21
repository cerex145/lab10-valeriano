using Hotel.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Web.Repositorios
{

    public class HabitacionRepositorio
    {
        private readonly HotelContext _contexto;

        public HabitacionRepositorio(HotelContext contexto) => _contexto = contexto;

        public async Task<List<Habitacion>> ListarActivasAsync(int? tipoHabitacionId = null)
        {
            var consulta = _contexto.Habitacions
                .AsNoTracking()
                .Include(h => h.TipoHabitacion)
                .Where(h => h.Activo);

            if (tipoHabitacionId.HasValue)
                consulta = consulta.Where(h => h.TipoHabitacionId == tipoHabitacionId.Value);

            return await consulta
                .OrderBy(h => h.Numero)
                .ToListAsync();
        }

        public async Task<Habitacion?> ObtenerPorIdAsync(int id) =>
            await _contexto.Habitacions
                .AsNoTracking()
                .Include(h => h.TipoHabitacion)
                .FirstOrDefaultAsync(h => h.HabitacionId == id);

        public async Task<Habitacion?> ObtenerParaEditarAsync(int id) =>
            await _contexto.Habitacions.FirstOrDefaultAsync(h => h.HabitacionId == id);

        public async Task<bool> ExisteNumeroAsync(string numero, int? excluirId = null) =>
            await _contexto.Habitacions
                .AsNoTracking()
                .AnyAsync(h => h.Numero == numero && (!excluirId.HasValue || h.HabitacionId != excluirId.Value));

        public async Task<List<TipoHabitacion>> ListarTiposAsync() =>
            await _contexto.TipoHabitacions
                .AsNoTracking()
                .OrderBy(t => t.Nombre)
                .ToListAsync();

        public async Task<List<Reserva>> ReservasPorHabitacionAsync(int habitacionId) =>
            await _contexto.Reservas
                .AsNoTracking()
                .Include(r => r.Huesped)
                .Where(r => r.HabitacionId == habitacionId)
                .OrderByDescending(r => r.FechaIngreso)
                .ToListAsync();

        public async Task<int> CrearAsync(Habitacion habitacion)
        {
            habitacion.Activo = true;
            _contexto.Habitacions.Add(habitacion);
            await _contexto.SaveChangesAsync();
            return habitacion.HabitacionId;
        }

        public async Task<bool> ActualizarAsync(Habitacion habitacion)
        {
            var actual = await ObtenerParaEditarAsync(habitacion.HabitacionId);
            if (actual is null) return false;

            actual.Numero = habitacion.Numero;
            actual.TipoHabitacionId = habitacion.TipoHabitacionId;
            actual.Piso = habitacion.Piso;

            await _contexto.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivarAsync(int id)
        {
            var habitacion = await ObtenerParaEditarAsync(id);
            if (habitacion is null) return false;

            habitacion.Activo = false;
            await _contexto.SaveChangesAsync();
            return true;
        }

        public async Task<int> ContarActivasAsync() =>
            await _contexto.Habitacions.AsNoTracking().CountAsync(h => h.Activo);

        public async Task<int> ContarOcupadasHoyAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return await _contexto.Reservas
                .AsNoTracking()
                .Where(r => r.Estado != "Cancelada" && r.FechaIngreso <= hoy && r.FechaSalida > hoy)
                .Select(r => r.HabitacionId)
                .Distinct()
                .CountAsync();
        }
    }
}
