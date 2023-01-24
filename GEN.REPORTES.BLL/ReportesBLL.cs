using GEN.REPORTES.DAL;
using GEN.REPORTES.ETY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEN.REPORTES.BLL
{
    public class ReportesBLL
    {

        private readonly ReportesDAL reportesDAL;

        private string Ip;

        public ReportesBLL(string ip,string usr, string pass)
        {
            this.reportesDAL = new ReportesDAL(ip,usr,pass);
            this.Ip = ip;
        }

        public bool DescargarReporte(string nameReporte) => reportesDAL.convertReporteToPDF(nameReporte);


        public List<string> ReportesDisponibles() => reportesDAL.GetAllReportes();



        public List<Servidor> GetServidores() => ReportesDAL.servidores;
    
    
    
    }
}
