using System;
using System.Collections.Generic;



public class Chofer
	{
		private string nombre, direccion, estadoCivil;
		private int dni;
		private DateTime fechaDeNacimiento;
		private double sueldoBasico;
		// public List<Viaje> listaViajes; // BUG: No se inicializaba la lista (provocaba NullReferenceException)
		public List<Viaje> listaViajes = new List<Viaje>();
		public Chofer(string nombre, string direccion, string estadoCivil, int dni, DateTime fechaDeNacimiento, double sueldoBasico)
		{
			this.nombre=nombre;
			this.direccion=direccion;
			this.estadoCivil=estadoCivil;
			this.dni=dni;
			this.fechaDeNacimiento=fechaDeNacimiento;
			this.sueldoBasico=sueldoBasico;
			this.listaViajes = new List<Viaje>();
		}
		public string Nombre{
			get{return nombre;}
		}
		public string Direccion{
			get{return direccion;}
		}
		public string EstadoCivil{
			get{return estadoCivil;}
		}
		public int Dni{
			get{return dni;}
		}
		public DateTime FechaDeNacimiento{
			get{return fechaDeNacimiento;}
		}
		public double SueldoBasico{
			get{return sueldoBasico;}
		}
		public List<Viaje> Viajes{
			get{return listaViajes;}
		}
		public string ToLineaCSV(){
			return nombre + ";" + direccion + ";" + estadoCivil + ";" + dni + ";" + fechaDeNacimiento.ToString("yyyy-MM-dd") + ";" + sueldoBasico;
		}
		public double calcularSueldoNeto(){
			// return sueldoBasico * 0.81; // BUG: 0.81 aplica 19% de deducción; el enunciado pide 21% (1 - 0.21 = 0.79)
			return sueldoBasico * 0.79;
		}
		public int calcularEdad(){
			int edad = DateTime.Today.Year - fechaDeNacimiento.Year;
    		if (fechaDeNacimiento.Date > DateTime.Today.AddYears(-edad))
    		{
        		edad--;
    		}
    		return edad;
		}
		public bool disponible(DateTime inicio, DateTime fin){
			foreach (Viaje viaje in listaViajes){
        		if (inicio < viaje.FechaFin && fin > viaje.FechaProgramada){
            		return false;
        		}
    		}
    		return true;
		}
	}