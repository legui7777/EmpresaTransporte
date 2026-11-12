public class MotoReparto : Vehiculo
{
    public MotoReparto(string codigoInterno, string matricula, int capacidadCarga, double kilometraje, double costoBase, string estado)
        : base(codigoInterno, matricula, capacidadCarga, kilometraje, costoBase, estado) { }

    // Constructor con el costo base fijo de $5000 indicado en el enunciado
    public MotoReparto(string codigoInterno, string matricula, int capacidadCarga, double kilometraje, string estado)
        : base(codigoInterno, matricula, capacidadCarga, kilometraje, 5000, estado) { }

    // el enunciado no da la fórmula; cada 100 km suman un 100% del costo base
    public override double CalcularCostoOperativo(double distancia, double cargaTransportada)
    {
        return costoBase * (1 + distancia / 100) * cargaTransportada / capacidadCarga;
    }

    public override string ToLineaCSV()
    {
        return "MotoReparto;" + codigoInterno + ";" + matricula + ";" + capacidadCarga + ";" + kilometraje + ";" + costoBase + ";" + estado;
    }

    public override string ToString()
    {
        return "[Moto de Reparto] " + base.ToString();
    }
}