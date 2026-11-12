using System;
using System.Collections.Generic;
using System.IO;



public class Viaje
{
    private string codigo;
    private string origen;
    private string destino;
    private double distancia;
    private DateTime fechaProgramada;
    private DateTime fechaFin;
    private double carga;
    private Vehiculo vehiculo;
    private List<Chofer> choferes;

   
    public Viaje(string codigo, string origen, string destino, double distancia, DateTime fechaProgramada, DateTime fechaFin, double carga, Vehiculo vehiculo, List<Chofer> choferes)
    {
        if (carga > vehiculo.CapacidadCarga)
        {
            throw new CapacidadExcedidaException("La carga a transportar supera la capacidad máxima del vehículo seleccionado.");
        }
        foreach (var chofer in choferes)
        {
           
            if (!chofer.disponible(fechaProgramada, fechaFin))
       
            {
                throw new ChoferOcupadoException("El chofer " + chofer.Nombre + " ya se encuentra asignado a otro viaje en el mismo horario.");
            }
        }
        
        this.codigo = codigo;
        this.origen = origen;
        this.destino = destino;
        this.distancia = distancia;
        this.fechaProgramada = fechaProgramada;
        this.fechaFin = fechaFin;
        this.carga = carga;
        this.vehiculo = vehiculo;
        this.choferes = choferes;

        foreach (var chofer in this.choferes)
        {
            chofer.listaViajes.Add(this);
        }
    }

    public string Codigo { get { return codigo; } }
    public string Origen { get { return origen; } }
    public string Destino { get { return destino; } }
    public double Distancia { get { return distancia; } }
    public DateTime FechaProgramada { get { return fechaProgramada; } }
    public DateTime FechaFin { get { return fechaFin; } }
    public double Carga { get { return carga; } }
    public Vehiculo VehiculoAsignado { get { return vehiculo; } }
    public List<Chofer> Choferes { get { return choferes; } }
    
    
    public double CalcularCostoTotal()
    {
        double costoVehiculo = vehiculo.CalcularCostoOperativo(distancia, carga);
        
        double sueldoChoferes = 0;
        foreach (var chofer in choferes)
        {
            sueldoChoferes += chofer.SueldoBasico;
        }

        return costoVehiculo + sueldoChoferes;
    }

    
    public void GenerarInformeCSV()
    {
        string path = "Informe_Viaje_" + codigo + ".csv";

        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.WriteLine("IdentificacionViaje;Vehiculo;Choferes;KilometrajeRecorrido;CostoOperativoVehiculo;CostoSueldosChoferes;CostoTotal");


            List<string> listaNombres = new List<string>();
            double sueldoChoferes = 0;
            foreach (var chofer in choferes)
            {
                listaNombres.Add(chofer.Nombre);
                sueldoChoferes += chofer.SueldoBasico;
            }
            string nombresChoferes = string.Join(" / ", listaNombres);
            double costoOperativo = vehiculo.CalcularCostoOperativo(distancia, carga);
            double costoTotal = CalcularCostoTotal();

            sw.WriteLine("{0};{1};{2};{3} km;${4};${5};${6}",
                codigo,
                vehiculo.Matricula,
                nombresChoferes,
                distancia,
                costoOperativo,
                sueldoChoferes,
                costoTotal);
        }

        Console.WriteLine("¡Verificación exitosa! Se generó el archivo: " + path);
    }
}
    