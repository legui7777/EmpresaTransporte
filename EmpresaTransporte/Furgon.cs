public class Furgon : Vehiculo
{
    public Furgon(string codigoInterno, string matricula, int capacidadCarga, double kilometraje, double costoBase, string estado)
        : base(codigoInterno, matricula, capacidadCarga, kilometraje, costoBase, estado) { }

    // Constructor base fijo de $10000
    public Furgon(string codigoInterno, string matricula, int capacidadCarga, double kilometraje, string estado)
        : base(codigoInterno, matricula, capacidadCarga, kilometraje, 10000, estado) { }

    public override double CalcularCostoOperativo(double distancia, double cargaTransportada)
    {
        return costoBase * cargaTransportada / capacidadCarga;
    }

    public override string ToLineaCSV()
    {
        return "Furgon;" + codigoInterno + ";" + matricula + ";" + capacidadCarga + ";" + kilometraje + ";" + costoBase + ";" + estado;
    }

    public override string MostrarDatos()
    {
        return "[Furgón] Código: " + codigoInterno + " | Matrícula: " + matricula + " | Cap. Carga: " + capacidadCarga + " kg | Km: " + kilometraje + " | Costo Base: $" + costoBase + " | Estado: " + estado;
    }
}