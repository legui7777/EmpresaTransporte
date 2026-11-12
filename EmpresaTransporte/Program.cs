using System;
using System.Collections.Generic;
using System.IO;

namespace EmpresaTransporte
{
	class Program
		
	{
		public static void Menu(EmpresaTransporte et1){
		
			int opcion = 0; //opcion a elegir
			do{
				try
				{
					//		Despliegue del Menu		//
					Console.WriteLine("====================================");
	    			Console.WriteLine("      SISTEMA DE GESTIÓN				");
	    			Console.WriteLine("====================================");
	    			Console.WriteLine("1. Registrar chofer nuevo");
	    			Console.WriteLine("2. Registrar vehículo nuevo");
	    			Console.WriteLine("3. Planificar viaje");
	    			Console.WriteLine("4. Eliminar chofer/vehículo");
	    			Console.WriteLine("5. Submenú de impresión / reportes");
	    			Console.WriteLine("6. Salir");
	    			Console.WriteLine("====================================");
	    			Console.WriteLine("Seleccione una opción: ");
	    			opcion = int.Parse(Console.ReadLine());
	    			
					switch(opcion)
					{
						case 1:
							et1.RegistrarChofer(ObtenerChofer());
							Console.WriteLine("Chofer registrado y persistido correctamente.");
							break;
						case 2:
							et1.RegistrarVehiculo(ObtenerVehiculo());
							Console.WriteLine("Vehículo registrado y persistido correctamente.");
							break;
						case 3:
							if (et1.Vehiculos.Count == 0 || et1.Choferes.Count == 0)
							{
								Console.WriteLine("Debe haber al menos un vehículo y un chofer registrados para planificar un viaje.");
							}
							else
							{
								Viaje nuevoViaje = ObtenerViaje(et1);
								et1.PlanificarViaje(nuevoViaje);
								Console.WriteLine("Viaje planificado exitosamente.");
							}
							break;
						case 4:
							EliminarRegistro(et1);
							break;
						case 5:
							Submenu(et1);
							break;
						case 6:
							Console.WriteLine("Saliendo del sistema...");
							break;
						default:
	            			Console.WriteLine("Opción inválida.");
	            			break;
					}
				}
				catch (CapacidadExcedidaException ex)
				{
					Console.WriteLine("[ERROR - Capacidad Excedida]: " + ex.Message);
				}
				catch (ChoferOcupadoException ex)
				{
					Console.WriteLine("[ERROR - Chofer Ocupado]: " + ex.Message);
				}
				catch (EliminacionAsignadaException ex)
				{
					Console.WriteLine("[ERROR - Eliminación No Permitida]: " + ex.Message);
				}
				catch (FormatException)
				{
					Console.WriteLine("[ERROR]: Formato de dato ingresado inválido.");
				}
				catch (Exception ex)
				{
					Console.WriteLine("[ERROR]: " + ex.Message);
				}
			}while(opcion!=6);
		}
		
		public static void EliminarRegistro(EmpresaTransporte et1)
		{
			Console.WriteLine("¿Qué desea eliminar?");
			Console.WriteLine("1. Chofer (por DNI)");
			Console.WriteLine("2. Vehículo (por Matrícula)");
			Console.Write("Seleccione una opción: ");
			int tipo = int.Parse(Console.ReadLine());

			if (tipo == 1)
			{
				Console.Write("Ingrese el DNI del chofer a eliminar: ");
				int dni = int.Parse(Console.ReadLine());
				et1.EliminarChofer(dni);
			}
			else if (tipo == 2)
			{
				Console.Write("Ingrese la matrícula del vehículo a eliminar: ");
				string matricula = Console.ReadLine();
				et1.EliminarVehiculo(matricula);
			}
			else
			{
				Console.WriteLine("Opción inválida.");
			}
		}
		
		public static void Submenu(EmpresaTransporte et1){
			int opcion = 0;
			do{
				try
				{
					Console.WriteLine("====================================");
	    			Console.WriteLine("        SISTEMA DE GESTIÓN		   ");
	    			Console.WriteLine("====================================");
	    			Console.WriteLine("   SUBMENÚ DE IMPRESION / REPORTES  ");
	    			Console.WriteLine("====================================");
	    			Console.WriteLine("1. Listado de choferes: mostrar nombre, edad, sueldo básico y viajes asignados");
	    			Console.WriteLine("2. Listar vehículos");
	    			Console.WriteLine("3. Viajes programados por fecha: mostrar código, ruta, distancia, y costo estimado");
	    			Console.WriteLine("4. Listar viajes asociados a un chofer");
	    			Console.WriteLine("5. Listar viajes asociados a un vehículo");
	    			Console.WriteLine("6. Información económica (Sueldos netos / Costos semanales / Facturación por período)");
	    			Console.WriteLine("7. Regresar al Menú");
	    			Console.WriteLine("====================================");
	    			Console.WriteLine("Seleccione una opción: ");
					opcion = int.Parse(Console.ReadLine());
					switch(opcion)
					{
						case 1:
							et1.ListarChoferes();
							break;
						case 2:
							et1.ListarVehiculos();
							break;
						case 3:
							Console.Write("Ingrese la fecha a consultar (dd/mm/aaaa): ");
							DateTime fecha = Convert.ToDateTime(Console.ReadLine());
							et1.ListarViajesPorFecha(fecha);
							break;
						case 4:
							Console.Write("Ingrese el DNI del chofer: ");
							int dni = Convert.ToInt32(Console.ReadLine());
							et1.ListarViajesDeChofer(dni);
							break;
						case 5:
							Console.Write("Ingrese la matrícula del vehículo: ");
							string matricula = Console.ReadLine();
							et1.ListarViajesDeVehiculo(matricula);
							break;
						case 6:
							MostrarReportesEconomicos(et1);
							break;
						case 7:
							break;
						default:
	            			Console.WriteLine("Opción inválida.");
	            			break;
					}
				}
				catch (FormatException)
				{
					Console.WriteLine("[ERROR]: Formato de dato o fecha inválido.");
				}
				catch (Exception ex)
				{
					Console.WriteLine("[ERROR]: " + ex.Message);
				}
				
			}while(opcion!=7);
			
		}

		public static void MostrarReportesEconomicos(EmpresaTransporte et1)
		{
			Console.WriteLine("--- INFORMACIÓN ECONÓMICA ---");
			Console.WriteLine("1. Monto neto total mensual a desembolsar en sueldos");
			Console.WriteLine("2. Costos operativos estimados de la flota para una semana");
			Console.WriteLine("3. Total facturable por viajes realizados en un período");
			Console.Write("Seleccione una opción: ");
			int op = int.Parse(Console.ReadLine());

			if (op == 1)
			{
				double totalSueldos = et1.CalcularMontoNetoTotalSueldos();
				Console.WriteLine("Monto neto total a desembolsar en sueldos (con 21% deducción): $" + totalSueldos);
			}
			else if (op == 2)
			{
				Console.Write("Ingrese la fecha de inicio de la semana (dd/mm/aaaa): ");
				DateTime inicioSemana = Convert.ToDateTime(Console.ReadLine());
				double costosSemana = et1.CalcularCostosOperativosSemana(inicioSemana);
				Console.WriteLine("Monto total de costos operativos de la flota para la semana: $" + costosSemana);
			}
			else if (op == 3)
			{
				Console.Write("Ingrese la fecha de inicio del período (dd/mm/aaaa): ");
				DateTime inicio = Convert.ToDateTime(Console.ReadLine());
				Console.Write("Ingrese la fecha de fin del período (dd/mm/aaaa): ");
				DateTime fin = Convert.ToDateTime(Console.ReadLine());
				double facturable = et1.CalcularTotalFacturablePeriodo(inicio, fin);
				Console.WriteLine("Total facturable en el período seleccionado: $" + facturable);
			}
			else
			{
				Console.WriteLine("Opción inválida.");
			}
		}
		
		public static Chofer ObtenerChofer()
		{
    		string nombre;
    		string direccion;
    		string estadoCivil;
    		int dni;
    		DateTime fechaNacimiento;
    		double sueldo;

    		Console.Write("Escriba el nombre completo: ");
    		nombre = Console.ReadLine();

    		Console.Write("Escriba la dirección: ");
    		direccion = Console.ReadLine();

    		Console.Write("Escriba el estado civil: ");
    		estadoCivil = Console.ReadLine();

    		Console.Write("Escriba el número de DNI: ");
    		dni = Convert.ToInt32(Console.ReadLine());

    		Console.Write("Escriba la fecha de nacimiento: ");
    		fechaNacimiento = Convert.ToDateTime(Console.ReadLine());

    		Console.Write("Escriba el sueldo bruto: ");
    		sueldo = Convert.ToDouble(Console.ReadLine());

    		return new Chofer(nombre, direccion, estadoCivil, dni, fechaNacimiento, sueldo);
		}
		
		public static Vehiculo ObtenerVehiculo()
		{
    		Console.WriteLine("Tipo de vehículo:");
    		Console.WriteLine("1. Moto");
    		Console.WriteLine("2. Furgón");

    		int tipo = Convert.ToInt32(Console.ReadLine());

    		Console.Write("Código interno: ");
    		string codigo = Console.ReadLine();

    		Console.Write("Matrícula: ");
    		string matricula = Console.ReadLine();

    		Console.Write("Capacidad de carga: ");
    		int capacidad = Convert.ToInt32(Console.ReadLine());

    		Console.Write("Kilometraje: ");
    		double kilometraje = Convert.ToDouble(Console.ReadLine());

    		Console.Write("Estado: ");
    		string estado = Console.ReadLine();

    		if (tipo == 1)
    		{
        		return new MotoReparto(codigo, matricula, capacidad, kilometraje, estado);
    		}
    		else
    		{
        		return new Furgon(codigo, matricula, capacidad, kilometraje, estado);
    		}
		}
		
		public static Viaje ObtenerViaje(EmpresaTransporte et1)
		{
    		Console.Write("Código del viaje: ");
    		string codigo = Console.ReadLine();

    		Console.Write("Origen: ");
    		string origen = Console.ReadLine();

    		Console.Write("Destino: ");
    		string destino = Console.ReadLine();

    		Console.Write("Distancia (km): ");
    		double distancia = Convert.ToDouble(Console.ReadLine());

    		Console.Write("Fecha programada: ");
    		DateTime fechaProgramada = Convert.ToDateTime(Console.ReadLine());

    		Console.Write("Fecha de finalización: ");
    		DateTime fechaFin = Convert.ToDateTime(Console.ReadLine());

    		Console.Write("Carga a transportar: ");
    		double carga = Convert.ToDouble(Console.ReadLine());
    		
    		Console.WriteLine("Vehículos disponibles:");

			for (int i = 0; i < et1.Vehiculos.Count; i++)
			{
    			Console.WriteLine((i + 1) + ". " + et1.Vehiculos[i].Matricula + " (" + et1.Vehiculos[i].MostrarDatos() + ")");
			}

			Console.Write("Seleccione un vehículo (número de opción o matrícula): ");
			string entradaVehiculo = Console.ReadLine().Trim();
			Vehiculo vehiculo = null;
			int opcionVehiculo;
			if (int.TryParse(entradaVehiculo, out opcionVehiculo) && opcionVehiculo >= 1 && opcionVehiculo <= et1.Vehiculos.Count)
			{
				vehiculo = et1.Vehiculos[opcionVehiculo - 1];
			}
			else
			{
				foreach (Vehiculo v in et1.Vehiculos)
				{
					if (string.Equals(v.Matricula, entradaVehiculo, StringComparison.OrdinalIgnoreCase) ||
					    string.Equals(v.CodigoInterno, entradaVehiculo, StringComparison.OrdinalIgnoreCase))
					{
						vehiculo = v;
						break;
					}
				}
			}
			if (vehiculo == null)
			{
				throw new Exception("El vehículo seleccionado no existe en la lista.");
			}
    
    		List<Chofer> listaChoferes = new List<Chofer>();

			Console.Write("¿Cuántos choferes tendrá el viaje?: ");
			int cantidad = Convert.ToInt32(Console.ReadLine());
			if (cantidad <= 0 || cantidad > et1.Choferes.Count)
			{
				throw new Exception("La cantidad de choferes debe estar entre 1 y " + et1.Choferes.Count + ".");
			}

			for (int i = 0; i < cantidad; i++)
			{
    			Console.WriteLine("Seleccione el chofer " + (i + 1) + " (número de opción, DNI o nombre):");

    			for (int j = 0; j < et1.Choferes.Count; j++)
    			{
        			Console.WriteLine((j + 1) + ". " + et1.Choferes[j].Nombre + " (DNI: " + et1.Choferes[j].Dni + ")");
    			}
	    	
	    		string entradaChofer = Console.ReadLine().Trim();
	    		Chofer choferSeleccionado = null;
	    		int opChofer;
	    		if (int.TryParse(entradaChofer, out opChofer) && opChofer >= 1 && opChofer <= et1.Choferes.Count)
	    		{
	    			choferSeleccionado = et1.Choferes[opChofer - 1];
	    		}
	    		else
	    		{
	    			foreach (Chofer c in et1.Choferes)
	    			{
	    				if (c.Dni.ToString() == entradaChofer ||
	    				    string.Equals(c.Nombre, entradaChofer, StringComparison.OrdinalIgnoreCase))
	    				{
	    					choferSeleccionado = c;
	    					break;
	    				}
	    			}
	    		}

	    		if (choferSeleccionado == null)
	    		{
	    			throw new Exception("El chofer indicado no existe en la lista.");
	    		}
	    		if (!listaChoferes.Contains(choferSeleccionado))
	    		{
	    			listaChoferes.Add(choferSeleccionado);
	    		}
			}

    		return new Viaje(codigo, origen, destino, distancia, fechaProgramada, fechaFin, carga, vehiculo, listaChoferes);
}
		
		public static void Main(string[] args)
		{
			EmpresaTransporte et1 = new EmpresaTransporte("EmpresaTransporte", "28645984562");
			// Se recupera la información de flota y choferes al iniciar el sistema
			et1.CargarDatos();
			Menu(et1);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}