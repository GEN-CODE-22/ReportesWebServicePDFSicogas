using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GEN.REPORTES.DAL
{
    public  class clsFtpService
    {
        

        private readonly string ip, usr, password;

        private clsFtpService(string ip, string user, string password)
        {
            this.ip = $"ftp://{ip}";
            this.usr = user ;
            this.password = password;
        }


        public static clsFtpService GetInstance(string ip, string user, string password) => new clsFtpService(ip, user, password);
          
        



        public List<string> searchReportesName()
        {


            var reportesName = new List<string>();


            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.ip);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(this.usr, this.password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                IList<string> files = names.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                var algo =  files.Select(e => e.Contains(".REP"));


                reportesName.AddRange  ((from rep in files where rep.Contains(".REP") select rep).ToList());


               
               


                return reportesName;
            }
            catch (Exception)
            {

                return reportesName;
            }
        }




        private string searchFileFTP(string nameFile)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.ip);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                request.Credentials = new NetworkCredential(this.usr, this.password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string names = reader.ReadToEnd();

                reader.Close();
                response.Close();

                IList<string> files = names.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                string file = files.Where(e => e == nameFile).First();

                return file;
            }
            catch (Exception e)
            {
               
                return null;
            }
        }


        public string DeleteFileFTP(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.ip + "/" + fileName);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(this.usr, this.password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine();
                return response.StatusDescription;
            }
        }


        public string SendFile(string file, string filePath)
        {
            try
            {
                string exist = searchFileFTP(file);
                if (exist != null) DeleteFileFTP(exist);



                string fileFtpPath = this.ip + "/" + file;
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(this.usr, this.password);
                    client.UploadFile(this.ip + "/" + file, WebRequestMethods.Ftp.UploadFile, filePath);
                    //client.UploadFile(this.ip, file);
                }
                return fileFtpPath;
            }
            catch (Exception ex)
            {
                
                return null;
            }

        }



        public string descargarFile(string fileName)
        {
            try
            {

               
                string exist = searchFileFTP(fileName);


                if (exist == null) return null;



                string fileFtpPath = this.ip + "/" + fileName;


                WebClient client = new WebClient();
                
                    client.Credentials = new NetworkCredential(this.usr, this.password);
    
                    byte[] fileData = client.DownloadData(fileFtpPath);

                    string fileNameDirectory = "C:\\Logs\\" + fileName.Replace(".REP", ".txt");

                    if (File.Exists(fileNameDirectory)) File.Delete(fileNameDirectory);

                    using(FileStream file = File.Create(fileNameDirectory))
                {
                    file.Write(fileData, 0, fileData.Length);
                }

                   
                    
                    

                 

                


                return "ok";

                
            }
            catch (Exception ex)
            {
              
                return null;
            }
        }

    }

}
