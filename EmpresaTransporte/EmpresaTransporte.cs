using System;
using System.IO;
using System.Collections.Generic;

namespace EmpresaTransporte
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class EmpresaTransporte
	{
		private string nombre;
		private string cuit;
		private List<Viaje> viajes = new List<Viaje>();
		private List<Chofer> choferes = new List<Chofer>();
		private List<Vehiculo> vehiculos = new List<Vehiculo>();
		
		public EmpresaTransporte(string nombre, string cuit)
		{
			this.nombre = nombre;
			this.cuit = cuit;
		}
		public List<Chofer> Choferes
		{
    		get { return choferes; }
		}

		public List<Vehiculo> Vehiculos
		{
    		get { return vehiculos; }
		}
		public List<Viaje> Viajes
		{
    		get { return viajes; }
		}
		
		private const string ARCHIVO_CHOFERES = "choferes.csv";
		private const string ARCHIVO_VEHICULOS = "vehiculos.csv";

		public void RegistrarChofer(Chofer chofer)
		{
			choferes.Add(chofer);
			GuardarDatos();
		}
		public void RegistrarVehiculo(Vehiculo vehiculo)
		{
			vehiculos.Add(vehiculo);
			GuardarDatos();
		}
		public void PlanificarViaje(Viaje viaje)
		{
			viajes.Add(viaje);
			viaje.GenerarInformeCSV();
		}
		// public void EliminarChofer(int dni){} // Faltaba implementar lógica y EliminacionAsignadaException
		public void EliminarChofer(int dni)
		{
			Chofer choferEncontrado = null;
			foreach (Chofer c in choferes)
			{
				if (c.Dni == dni)
				{
					choferEncontrado = c;
					break;
				}
			}

			if (choferEncontrado == null)
			{
				Console.WriteLine("No se encontró ningún chofer con el DNI " + dni + ".");
				return;
			}

			if (choferEncontrado.listaViajes.Count > 0)
			{
				throw new EliminacionAsignadaException("No se puede eliminar al chofer " + choferEncontrado.Nombre + " (DNI: " + dni + ") porque está asignado a viajes activos.");
			}

			choferes.Remove(choferEncontrado);
			GuardarDatos();
			Console.WriteLine("Chofer eliminado y cambios persistidos correctamente.");
		}
		
		// public void EliminarVehiculo(string matricula){} // Faltaba implementar lógica y EliminacionAsignadaException
		public void EliminarVehiculo(string matricula)
		{
			Vehiculo vehiculoEncontrado = null;
			foreach (Vehiculo v in vehiculos)
			{
				if (string.Equals(v.Matricula, matricula, StringComparison.OrdinalIgnoreCase))
				{
					vehiculoEncontrado = v;
					break;
				}
			}

			if (vehiculoEncontrado == null)
			{
				Console.WriteLine("No se encontró ningún vehículo con la matrícula " + matricula + ".");
				return;
			}

			foreach (Viaje viaje in viajes)
			{
				if (string.Equals(viaje.VehiculoAsignado.Matricula, matricula, StringComparison.OrdinalIgnoreCase))
				{
					throw new EliminacionAsignadaException("No se puede eliminar el vehículo con matrícula " + matricula + " porque está asignado al viaje activo " + viaje.Codigo + ".");
				}
			}

			vehiculos.Remove(vehiculoEncontrado);
			GuardarDatos();
			Console.WriteLine("Vehículo eliminado y cambios persistidos correctamente.");
		}
		
		//public double CalcularMontoNetoTotalSueldos(){}
		public double CalcularMontoNetoTotalSueldos()
		{
			double total = 0;
			foreach (Chofer c in choferes)
			{
				total += c.calcularSueldoNeto();
			}
			return total;
		}
		
		//public double CalcularCostosOperativosSemana(){}
		public double CalcularCostosOperativosSemana(DateTime inicioSemana)
		{
			double total = 0;
			DateTime desde = inicioSemana.Date;
			DateTime hasta = desde.AddDays(7);

			foreach (Viaje v in viajes)
			{
				if (v.FechaProgramada.Date >= desde && v.FechaProgramada.Date < hasta)
				{
					total += v.VehiculoAsignado.CalcularCostoOperativo(v.Distancia, v.Carga);
				}
			}
			return total;
		}
		
		//public double CalcularTotalFacturablePeriodo(){}
		public double CalcularTotalFacturablePeriodo(DateTime inicio, DateTime fin)
		{
			double total = 0;
			DateTime desde = inicio.Date;
			DateTime hasta = fin.Date;

			foreach (Viaje v in viajes)
			{
				if (v.FechaProgramada.Date >= desde && v.FechaProgramada.Date <= hasta)
				{
					total += v.CalcularCostoTotal();
				}
			}
			return total;
		}
		
		// public void ListarChoferes(){}
		public void ListarChoferes()
		{
			if (choferes.Count == 0)
			{
				Console.WriteLine("No hay choferes registrados.");
				return;
			}

			Console.WriteLine("--- LISTADO DE CHOFERES ---");
			foreach (Chofer c in choferes)
			{
				List<string> codigosViajes = new List<string>();
				foreach (Viaje v in c.listaViajes)
				{
					codigosViajes.Add(v.Codigo + " (" + v.Origen + " -> " + v.Destino + ")");
				}
				string viajesTexto = codigosViajes.Count > 0 ? string.Join(", ", codigosViajes) : "Sin viajes asignados";

				Console.WriteLine("Nombre: {0} | DNI: {1} | Edad: {2} años | Sueldo Básico: ${3} | Sueldo Neto: ${4} | Viajes: {5}",
					c.Nombre, c.Dni, c.calcularEdad(), c.SueldoBasico, c.calcularSueldoNeto(), viajesTexto);
			}
		}
		
		// public void ListarVehiculos(){}
		public void ListarVehiculos()
		{
			if (vehiculos.Count == 0)
			{
				Console.WriteLine("No hay vehículos registrados.");
				return;
			}

			Console.WriteLine("--- LISTADO DE VEHÍCULOS ---");
			foreach (Vehiculo v in vehiculos)
			{
				// Polimorfismo: cada subclase (Furgon / MotoReparto) redefine ToString()
				Console.WriteLine(v.ToString());
			}
		}
		
		// public void ListarViajesPorFecha(){}
		public void ListarViajesPorFecha(DateTime fecha)
		{
			bool encontrado = false;
			Console.WriteLine("--- VIAJES PROGRAMADOS PARA EL " + fecha.ToString("dd/MM/yyyy") + " ---");
			foreach (Viaje v in viajes)
			{
				if (v.FechaProgramada.Date == fecha.Date)
				{
					Console.WriteLine("Código: {0} | Ruta: {1} -> {2} | Distancia: {3} km | Costo Estimado: ${4}",
						v.Codigo, v.Origen, v.Destino, v.Distancia, v.CalcularCostoTotal());
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				Console.WriteLine("No hay viajes programados para la fecha indicada.");
			}
		}
		
		// public void ListarViajesPorChofer(){} // Método duplicado respecto al UML (ListarViajesDeChofer)
		// public void ListarViajesdeChofer(){}
		public void ListarViajesDeChofer(int dni)
		{
			Chofer choferEncontrado = null;
			foreach (Chofer c in choferes)
			{
				if (c.Dni == dni)
				{
					choferEncontrado = c;
					break;
				}
			}

			if (choferEncontrado == null)
			{
				Console.WriteLine("No se encontró ningún chofer con el DNI " + dni + ".");
				return;
			}

			Console.WriteLine("--- VIAJES DEL CHOFER: " + choferEncontrado.Nombre + " (DNI: " + dni + ") ---");
			if (choferEncontrado.listaViajes.Count == 0)
			{
				Console.WriteLine("El chofer no tiene viajes asignados.");
				return;
			}

			foreach (Viaje v in choferEncontrado.listaViajes)
			{
				Console.WriteLine("Código: {0} | Ruta: {1} -> {2} | Inicio: {3} | Fin: {4} | Vehículo: {5} | Costo Total: ${6}",
					v.Codigo, v.Origen, v.Destino, v.FechaProgramada, v.FechaFin, v.VehiculoAsignado.Matricula, v.CalcularCostoTotal());
			}
		}
		
		// public void ListarViajesDeVehiculo(){}
		public void ListarViajesDeVehiculo(string matricula)
		{
			bool encontrado = false;
			Console.WriteLine("--- VIAJES DEL VEHÍCULO MATRÍCULA: " + matricula + " ---");
			foreach (Viaje v in viajes)
			{
				if (string.Equals(v.VehiculoAsignado.Matricula, matricula, StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine("Código: {0} | Ruta: {1} -> {2} | Inicio: {3} | Fin: {4} | Distancia: {5} km | Carga: {6} kg | Costo Total: ${7}",
						v.Codigo, v.Origen, v.Destino, v.FechaProgramada, v.FechaFin, v.Distancia, v.Carga, v.CalcularCostoTotal());
					encontrado = true;
				}
			}

			if (!encontrado)
			{
				Console.WriteLine("No se encontraron viajes asociados al vehículo con matrícula " + matricula + ".");
			}
		}
		
		// public void GuardarDatos(){}
		public void GuardarDatos()
		{
			try
			{
				using (StreamWriter swChoferes = new StreamWriter(ARCHIVO_CHOFERES, false))
				{
					foreach (Chofer c in choferes)
					{
						swChoferes.WriteLine(c.ToLineaCSV());
					}
				}

				using (StreamWriter swVehiculos = new StreamWriter(ARCHIVO_VEHICULOS, false))
				{
					foreach (Vehiculo v in vehiculos)
					{
						// Polimorfismo: ToLineaCSV() es abstracto en Vehiculo e implementado en Furgon y MotoReparto
						swVehiculos.WriteLine(v.ToLineaCSV());
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al persistir los datos: " + ex.Message);
			}
		}
		
		// public void CargarDatos(){}
		public void CargarDatos()
		{
			try
			{
				if (File.Exists(ARCHIVO_CHOFERES))
				{
					choferes.Clear();
					using (StreamReader srChoferes = new StreamReader(ARCHIVO_CHOFERES))
					{
						string linea;
						while ((linea = srChoferes.ReadLine()) != null)
						{
							if (string.IsNullOrWhiteSpace(linea)) continue;
							string[] partes = linea.Split(';');
							if (partes.Length >= 6)
							{
								string nombreChofer = partes[0];
								string direccion = partes[1];
								string estadoCivil = partes[2];
								int dni = int.Parse(partes[3]);
								DateTime fechaNac = DateTime.Parse(partes[4]);
								double sueldo = double.Parse(partes[5]);

								choferes.Add(new Chofer(nombreChofer, direccion, estadoCivil, dni, fechaNac, sueldo));
							}
						}
					}
				}

				if (File.Exists(ARCHIVO_VEHICULOS))
				{
					vehiculos.Clear();
					using (StreamReader srVehiculos = new StreamReader(ARCHIVO_VEHICULOS))
					{
						string linea;
						while ((linea = srVehiculos.ReadLine()) != null)
						{
							if (string.IsNullOrWhiteSpace(linea)) continue;
							string[] partes = linea.Split(';');
							if (partes.Length >= 7)
							{
								string tipo = partes[0];
								string codigoInterno = partes[1];
								string matricula = partes[2];
								int capacidad = int.Parse(partes[3]);
								double kilometraje = double.Parse(partes[4]);
								double costoBase = double.Parse(partes[5]);
								string estado = partes[6];

								if (tipo == "MotoReparto")
								{
									vehiculos.Add(new MotoReparto(codigoInterno, matricula, capacidad, kilometraje, costoBase, estado));
								}
								else if (tipo == "Furgon")
								{
									vehiculos.Add(new Furgon(codigoInterno, matricula, capacidad, kilometraje, costoBase, estado));
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error al recuperar los datos iniciales: " + ex.Message);
			}
		}
			
	}
}
