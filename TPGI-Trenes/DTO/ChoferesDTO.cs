namespace TPGI_Trenes.DTO
{
    public class ChoferesDTO
    {
        public int Nro_Chofer { get; set; }
        public string? Chofer { get; set; }
        public int DNI { get; set; }
        public string? Barrio { get; set; }
        public string? Telefono { get; set; }
        public int Viajes_Realizados { get; set; }
        public int Trenes_Conducidos { get; set; }
        public int Minutos_Conducidos { get; set; }
        public double Promedio_Minutos_Viaje { get; set; }
        public string? Estado_Carga_Horaria { get; set; }
    }
}