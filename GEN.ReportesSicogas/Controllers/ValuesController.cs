using GEN.REPORTES.BLL;
using GEN.REPORTES.DAL;
using GEN.REPORTES.ETY;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;



namespace GEN.ReportesSicogas.Controllers
{
    [RoutePrefix("api/reportes")]

    public class ValuesController : ApiController
    {

        /// <summary>
        /// Guardar el Token y la devolucion de la configuracion
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reporte")]

        public HttpResponseMessage GetReporte([FromBody] BodyReporte body)
        {

            if (body.Server == null) return Request.CreateResponse(HttpStatusCode.NotFound, "No Enviaste un campo Requerido");

            Servidor servidor = ReportesDAL.servidores.Select(e => e).Where(e => e.ServerName == body.Server).FirstOrDefault();

            if (servidor == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Servidor no encontrado");

            var bll = new ReportesBLL(servidor.Ip, body.user, body.password);

            var respuesta = bll.DescargarReporte(body.NameFile);


            if (!respuesta) return Request.CreateResponse(HttpStatusCode.BadRequest, $"Hubo un problema la descargar el archivo");

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            string fileNameDirectory = "C:\\Logs\\" + body.NameFile.Replace(".REP", ".pdf");

            var stream = new FileStream(fileNameDirectory, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            if (File.Exists(fileNameDirectory)) File.Delete(fileNameDirectory);
            if (File.Exists(body.NameFile)) File.Delete(body.NameFile);
            return result;



        }


        [HttpPost]
        [Route("reportes")]

        public HttpResponseMessage GetReportes([FromBody] BodyReporte body)
        {
            if (body.Server == null) return Request.CreateResponse(HttpStatusCode.NotFound, "No Enviaste un campo Requerido");

            Servidor servidor = ReportesDAL.servidores.Select(e => e).Where(e => e.ServerName == body.Server).FirstOrDefault();

            if (servidor == null) return Request.CreateResponse(HttpStatusCode.NotFound, $"Servidor no encontrado");

            var bll = new ReportesBLL(servidor.Ip, body.user, body.password);

            var respuesta = bll.ReportesDisponibles();


            return new HttpResponseMessage
            {
                Content = new StringContent(JsonConvert.SerializeObject(respuesta),
                                            System.Text.Encoding.UTF8, "application/json")
            };

        }

    }
}
