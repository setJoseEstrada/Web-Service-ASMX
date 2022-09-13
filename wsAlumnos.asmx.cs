using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls.WebParts;
using Entidades;
using Negocio;
using Datos;

namespace webServicesASMX
{
    /// <summary>
    /// Descripción breve de wsAlumnos
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]//declara un enlace que consume uno o mas metodos
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
     [System.Web.Script.Services.ScriptService]
    public class wsAlumnos : System.Web.Services.WebService
    {

        [WebMethod] //expuesto para que lo comsuma otra aplicacion
        //clases ya creadas vamos a llamar a Negocios
        public string HelloWorld()
        {
            return "Hola a todos";
        }


        [WebMethod] 
        
        public AportacionesIMSS CalcularIMSS(int id)
        {
            DAlumno alumno = new DAlumno();

            return alumno.CalcularIMSS(id);
        }
        [WebMethod]
        public ItemTablaISR CalcularISR(int id)
        {
            DAlumno alumno = new DAlumno();

            List<ItemTablaISR> nuevo1 = alumno.ConsultarTablaISR(id); // mando a llamar mi metodo en una lista para RECORRER SUS VALORES
            alumno.Consultar(id);
            Alumno alumnos = alumno.Consultar(id);
            decimal quincenal = alumnos.Sueldo / 2;
            //Realizar el calculo en esta seccion de negocios
            ItemTablaISR nuevo = new ItemTablaISR();

            var alumnos1 = from Alumno in nuevo1
                          where quincenal > Alumno.LimiteInferior
                           && quincenal < Alumno.LimiteSuperior
                          select Alumno; //Utilizo Linq para buscar los valores que necesito

            foreach (var item in alumnos1)
            {

                nuevo.LimiteInferior = item.LimiteInferior;
                nuevo.LimiteSuperior = item.LimiteSuperior;
                nuevo.CuotaFija = item.CuotaFija;
                nuevo.Excedente = item.Excedente;
                nuevo.Subsidio = item.Subsidio;
                decimal Excedente = (quincenal - nuevo.LimiteInferior) * (nuevo.Excedente / 100);
                nuevo.ISR = Excedente + nuevo.CuotaFija - nuevo.Subsidio;
                break;
            }

            return nuevo;


        }
    }
}
