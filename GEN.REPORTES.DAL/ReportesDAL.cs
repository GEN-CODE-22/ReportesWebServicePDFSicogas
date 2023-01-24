
using GEN.REPORTES.ETY;
//using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;
//using iTextSharp.text;
//using iTextSharp;
//using PdfSharp;
//using PdfSharp.Drawing;
//using PdfSharp.Pdf;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Text.RegularExpressions;
using System.Web.Hosting;

namespace GEN.REPORTES.DAL



{
    public class ReportesDAL

    {

        private readonly clsFtpService ftpService;


        public ReportesDAL(string ipSicogas, string usr, string pass)
        {
            this.ftpService = clsFtpService.GetInstance(ipSicogas,usr,pass);
        }



        private  bool DownloadReporte(string nameReporte)
        {



            string response = ftpService.descargarFile(nameReporte);

            if (response != null) return true;


            return false;





        }


        public bool convertReporteToPDF(string nameReporte)
        {
            var descargado = DownloadReporte(nameReporte);

            if (!descargado) return false;



           

            string fileNameDirectory = "C:\\Logs\\" + nameReporte.Replace(".REP",".txt");


            string s = "";


            ///Creamos el PDF 
            using (StreamReader rdr = new StreamReader(fileNameDirectory))
            {

                s = rdr.ReadToEnd();
                //Create a New instance on Document Class
                Document doc = new Document(PageSize.A4.Rotate(), 50, 50, 25, 25);

                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                Font font = new Font(bf, 10, Font.NORMAL);


                if (File.Exists(fileNameDirectory.Replace(".txt", ".pdf"))) File.Delete(fileNameDirectory.Replace(".txt", ".pdf"));

                //Create a New instance of PDFWriter Class for Output File
                PdfWriter.GetInstance(doc, new FileStream(fileNameDirectory.Replace(".txt", ".pdf"), FileMode.Create));
                //Open the Document
                doc.Open();

                //var imageUrl = HostingEnvironment.MapPath("~/images/logo.png");



                //var img = iTextSharp.text.Image.GetInstance(imageUrl);
                //img.SetAbsolutePosition(200, 700);








                //doc.Add(img);

                s = Regex.Replace(s, @"[^\u0000-\u007F]+", string.Empty);

                var paragraph = new Paragraph(s, font); 


                paragraph.Alignment = Element.ALIGN_JUSTIFIED_ALL;
                paragraph.IndentationRight = 15;
                paragraph.IndentationLeft = 15;
                paragraph.SpacingAfter = 40;
                paragraph.SpacingBefore = 1;

                //Add the content of Text File to PDF File
                doc.Add(paragraph);
                //Close the Document
                doc.Close();

            }

            //Eliminar el archivo del Servidor de sicogas


            ftpService.DeleteFileFTP(nameReporte);




            return true;


        }


        public List<string> GetAllReportes() => ftpService.searchReportesName();
        


        static public List<Servidor> servidores = new List<Servidor>()
        {
            new Servidor(){ Planta="12",ServerName = "Acambaro", Ip = "172.16.43.200", UserPeople = new List<string>(){"GIDME", "GISAL" } },
            new Servidor(){ Planta="29",ServerName = "Aguascalientes", Ip = "172.16.24.200", UserPeople =new List<string>() {"GICND" } },
            new Servidor(){ Planta="19",ServerName = "Arandas", Ip = "172.16.53.200", UserPeople = new List<string>(){"GIGLN"} },
            new Servidor(){ Planta="01",ServerName = "Celaya", Ip = "172.16.81.200", UserPeople = new List<string>(){"GIMCG"} },
            new Servidor(){ Planta="24",ServerName = "Culiacan", Ip = "172.16.49.200", UserPeople =new List<string>() {"GIPHM"} },
            new Servidor(){ Planta="07",ServerName = "Dolores", Ip = "172.16.26.1", UserPeople =new List<string>() {"GISBP"} },
            new Servidor(){ Planta="66",ServerName = "Guadalaj", Ip = "172.16.45.200", UserPeople =new List<string>() {"GIFEN"} },
            new Servidor(){ Planta="26",ServerName = "Guamuchi", Ip = "172.16.88.200", UserPeople = new List<string>(){"GIPHM"} },
            new Servidor(){ Planta="04",ServerName = "Irapuato", Ip = "172.16.40.200", UserPeople =new List<string>() {"GIRSE"} },
            new Servidor(){ Planta="16",ServerName = "LaPiedad", Ip = "172.16.47.200", UserPeople =new List<string>() {"GICGA"} },
            new Servidor(){ Planta="14",ServerName = "LazaroCardenas", Ip = "172.16.46.200", UserPeople =new List<string>() {"GITPL"} },
            new Servidor(){ Planta="20",ServerName = "Leon", Ip = "172.16.85.200", UserPeople =new List<string>() {"GIVSH" } },
            new Servidor(){ Planta="25",ServerName = "Mazatlan", Ip = "172.16.35.200", UserPeople = new List<string>(){"GIAZM"} } ,
            new Servidor(){ Planta="27",ServerName = "Mochis", Ip = "172.16.33.200", UserPeople = new List<string>(){"GIPHM"} },
            new Servidor(){ Planta="21",ServerName = "Monterrey", Ip = "172.16.42.200", UserPeople = new List<string>(){"GIPJW"} },
            new Servidor(){ Planta="09",ServerName = "Morelia", Ip = "172.16.32.200", UserPeople =new List<string>() {"GIGGL"} },
            new Servidor(){ Planta="18",ServerName = "Moroleon", Ip = "172.16.87.200", UserPeople = new List<string>(){"GIGGM"} },
            new Servidor(){ Planta="02",ServerName = "Nietocde", Ip = "172.16.1.203"} ,
            new Servidor(){ Planta="02",ServerName = "Queretar", Ip = "172.16.30.200", UserPeople =new List<string>() {"GISRR"} },
            new Servidor(){ Planta="17",ServerName = "Salvatierra", Ip = "172.16.44.200", UserPeople =new List<string>() {"GISAL"} },
            new Servidor(){ Planta="13",ServerName = "SanJoseI", Ip = "172.16.48.200", UserPeople =new List<string>() {"GISRR"} },
            new Servidor(){ Planta="06",ServerName = "SanJuan", Ip = "172.16.38.200", UserPeople =new List<string>() {"GISCR"} },
            new Servidor(){ Planta="23",ServerName = "SanLuis", Ip = "172.16.99.200", UserPeople = new List<string>(){"GICCM"} },
            new Servidor(){ Planta="03",ServerName = "SnMiguel", Ip = "172.16.39.200", UserPeople = new List<string>(){"GIAGN"} },
            new Servidor(){ Planta="65",ServerName = "Tepatitlan", Ip ="172.16.65.200", UserPeople =new List<string>() {"GIFEN"} },
            new Servidor(){ Planta="28",ServerName = "Tula", Ip = "172.16.38.199", UserPeople =new List<string>() {"GILMR"} },
            new Servidor(){ Planta="15",ServerName = "Uruapan", Ip = "172.16.98.200", UserPeople =new List<string>() {"GIZDG"} },
            new Servidor(){ Planta="10",ServerName = "Veracruz", Ip = "172.16.41.200", UserPeople = new List<string>(){"GICRO"} },
            new Servidor(){ Planta="08",ServerName = "Xalapa", Ip = "172.16.37.200", UserPeople =new List<string>() {"GICPA"} },
        };

    }
}
