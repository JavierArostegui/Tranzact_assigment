using System;
using System.Collections.Generic;  

namespace Tranzact_assigment
{
    class Program
    {
        static void Main(string[] args)
        {
            ProccessTranzact();
            Console.Read();
        }

        public static async void ProccessTranzact()
        {            
            Console.WriteLine("\nProcessing. It may take a while...\n");
            Console.WriteLine("Period \t\t Language \t Domain \t\t ViewCount");
            DateTime prevDay = new DateTime();
            List<Documents> docList = new List<Documents>();
            List<Documents> docListToRead = new List<Documents>();
            List<LanguageDomain> orderList = new List<LanguageDomain>();
            List<LanguageDomain> orderListComplete = new List<LanguageDomain>();
            List<pageViews> orderListPageView = new List<pageViews>();            
            DocuNames objDocNamesToRead = new DocuNames();
            FileDownload objArch = new FileDownload();
            GZIP objGZIP = new GZIP();
            
            int day;
            int lote;
            string folderName = "";
            for (int i = 0 ; i < Resources.numberDays; i++)
            {
                lote = 0;
                day = Resources.numberDays - i;
                prevDay = Resources.today.AddDays(-day);
                folderName = objGZIP.getFolderName(prevDay);
                objGZIP.createDirectory(folderName);

                for (int j = 0; j < 24; j=j+3) 
                {                    
                    DocuNames objDocNames = new DocuNames();
                    lote = j + 1;                    
                    docList = objDocNames.getDocumentList(prevDay, lote, folderName);
                    await objArch.DownloadMultipleFilesAsync(docList, folderName);
                }
                objGZIP.DecompressFiles(folderName);

                docListToRead = objDocNamesToRead.getDocumentListToRead(prevDay);
                (orderList, orderListPageView) = objGZIP.readFiles(folderName, docListToRead);

                foreach (LanguageDomain lanDom in orderList)
                {
                    orderListComplete.Add(lanDom);
                }

                orderList.Clear();
                docListToRead.Clear();         
                
            }      

            foreach (LanguageDomain lanDom in orderListComplete)
            {
                Console.WriteLine(lanDom.getLine());
            }            

            Console.WriteLine("\n\nPeriod \t\t Page \t\t ViewCount");

            foreach (pageViews pagView in orderListPageView)
            {
                Console.WriteLine(pagView.getLine());
            }

            Console.WriteLine("\nThe process has finished. Press any key to continue...");
            Console.Read();
        }
    }
}
