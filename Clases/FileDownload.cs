using System;
using System.Net;
using System.Collections.Generic;  
using System.Threading.Tasks;
using System.Linq;
 
namespace Tranzact_assigment
{
    class FileDownload
    {
        public FileDownload(){}        

        private async Task DownloadFileAsync(Documents doc, string folderName)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    string downloadToDirectory = Resources.FilesPath + folderName + @"\" + doc.docName;                    
                    webClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    await webClient.DownloadFileTaskAsync(new Uri(doc.docURL), @downloadToDirectory);                    
                }         
            }
            catch (Exception e)
            {
                Console.WriteLine("Was not able to download file " + doc.docName);
                Console.Write(e);   
                throw;
            }
        }

        public async Task DownloadMultipleFilesAsync(List<Documents> doclist, string folderName)
        {               
            await Task.WhenAll(doclist.Select(doc => DownloadFileAsync(doc, folderName)));            
            
        }
    }
}