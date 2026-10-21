namespace Hotel.Web.ViewModels
{

    public class HabitacionDisponibleViewModel
    {
        public int HabitacionId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Piso { get; set; }
        public int Capacidad { get; set; }
        public decimal PrecioNoche { get; set; }
    }

    public class DisponibilidadViewModel
    {
        public DateOnly? Ingreso { get; set; }
        public DateOnly? Salida { get; set; }

        public bool Buscado { get; set; }

        public int Noches =>
            Ingreso.HasValue && Salida.HasValue && Salida > Ingreso
                ? Salida.Value.DayNumber - Ingreso.Value.DayNumber
                : 0;

        public List<HabitacionDisponibleViewModel> Habitaciones { get; set; } = new();

        public decimal TotalEstimado(HabitacionDisponibleViewModel habitacion) =>
            Noches * habitacion.PrecioNoche;

        public decimal PrecioMinimo => Habitaciones.Count == 0 ? 0 : Habitaciones.Min(h => h.PrecioNoche);
    }
}
