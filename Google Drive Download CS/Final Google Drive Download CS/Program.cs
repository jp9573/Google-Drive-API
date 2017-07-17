using Google.Apis.Drive.v2;
using System;
using System.Collections.Generic;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;

namespace Final_Google_Drive_Download_CS
{
    class Program
    {
        static void Main(string[] args)
        {
            String CLIENT_ID = "MyCLIENT ID";
            String CLIENT_SECRET = "MySECRET CODE";
            DriveService service = AuthenticateOauth(CLIENT_ID, CLIENT_SECRET, Environment.UserName);

            string FileName = "My document";
            string MimeType = "text/plain";
            string Q = "title = '"+ FileName +"' and mimeType = '"+ MimeType +"'";

            IList<File> _Files = GetFiles(service, Q);
            File f = null;
            if(_Files.Count > 0)
            {
                for(int i = 0; i < _Files.Count; i++)
                {
                    if(_Files[i].OriginalFilename == FileName)
                    {
                        f = _Files[i];
                        break;
                    }
                }
            }

            if (_Files.Count > 0 && f != null)
            {
                File newFile = f;
                bool isDownloaded = downloadFile(service, newFile, @"C:\Users\Jay\Desktop\RavitechWorld\downloaded.txt");
                if (isDownloaded == true)
                    Console.WriteLine("Download Successful!");
                else
                    Console.WriteLine("Download Failed!!");
                Console.ReadKey();
            }else
            {
                Console.WriteLine("No File Found");
                Console.ReadKey();
            }
        }

        public static Boolean downloadFile(DriveService _service, File _fileResource, string _saveTo)
        {
            if (!String.IsNullOrEmpty(_fileResource.DownloadUrl))
            {
                try
                {
                    var x = _service.HttpClient.GetByteArrayAsync(_fileResource.DownloadUrl);
                    byte[] arrBytes = x.Result;
                    System.IO.File.WriteAllBytes(_saveTo, arrBytes);
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static DriveService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {            
            string[] scopes = new string[] { DriveService.Scope.Drive};


            try
            {
                
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("Daimto.Drive.Auth.Store")).Result;

                DriveService service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Daimto Drive API Sample",
                });
                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                return null;
            }

        }

        public static IList<File> GetFiles(DriveService service, string search)
        {     
            IList<File> Files = new List<File>();
            try
            {
                FilesResource.ListRequest list = service.Files.List();
                list.MaxResults = 1000;
                if (search != null)
                {
                    list.Q = search;
                }
                FileList filesFeed = list.Execute();
                
                while (filesFeed.Items != null)
                {
                    foreach (File item in filesFeed.Items)
                    {
                        Files.Add(item);
                    }
                    if (filesFeed.NextPageToken == null)
                    {
                        break;
                    }

                    list.PageToken = filesFeed.NextPageToken;

                    filesFeed = list.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Files;
        } 
    }
}
