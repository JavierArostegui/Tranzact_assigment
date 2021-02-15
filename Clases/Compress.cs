using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;  
using System.Linq;

namespace Tranzact_assigment
{
    class GZIP{
        
        private static DirectoryInfo directorySelected;

        public GZIP(){
            directorySelected = new DirectoryInfo(Resources.FilesPath);
        }
        
        public static void Decompress(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);                        
                    }
                }
            }
        }

        public void DecompressFiles(string folderName)
        {
            string newPath = Resources.FilesPath + folderName;
            DirectoryInfo newDirectorySelected = new DirectoryInfo(newPath);
            
            foreach (FileInfo fileToDecompress in newDirectorySelected.GetFiles("*.gz"))
            {
                Decompress(fileToDecompress);
            }

        }
  

        public void createDirectory(string folderName){
            
            try
            {   
                string path = Resources.FilesPath + folderName;
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("There is Problem creating directory " + folderName);
                Console.WriteLine(e);
                throw;
            }

        }

        public string getFolderName(DateTime folderDate)
        {
            string folderName = "";
            folderName = folderDate.Year.ToString() + folderDate.Month.ToString("00") + folderDate.Day.ToString("00");
            return folderName;
        }

        public (List<LanguageDomain>, List<pageViews>) readFiles(string folderName, List<Documents> doclist)
        {
            string path;
            string[] words;            
            
            string[] lines; 
            ulong cantViews = 0;
            ulong totalViews = 0;
            ulong allFilesViews = 0;

            
            List<Domain> DomainsList = new List<Domain>();        
            DomainsList.Add(new Domain {code = "", trailing = "wikipedia"});
            DomainsList.Add(new Domain {code = "b", trailing = "wikibooks"});
            DomainsList.Add(new Domain {code = "d", trailing = "wiktionary"});
            DomainsList.Add(new Domain {code = "f", trailing = "wikimediafoundation"});
            DomainsList.Add(new Domain {code = "m", trailing = "wikimedia"});
            DomainsList.Add(new Domain {code = "n", trailing = "wikinews"});
            DomainsList.Add(new Domain {code = "q", trailing = "wikiquote"});
            DomainsList.Add(new Domain {code = "s", trailing = "wikisource"});
            DomainsList.Add(new Domain {code = "v", trailing = "wikiversity"});
            DomainsList.Add(new Domain {code = "voy", trailing = "wikivoyage"});
            DomainsList.Add(new Domain {code = "w", trailing = "mediawiki"});
            DomainsList.Add(new Domain {code = "wd", trailing = "wikidata"});

            List<LanguageDomain> LanDomList = new List<LanguageDomain>();
            List<pageViews> pageViewList = new List<pageViews>();

            Domain auxDomain = new Domain();
            
            string[] domainStr;
            string language;
            string prevLanguage = "";
            string domainCode;
            string prevDomainCode = "";
            string stratOfFile = "";
            string page = "";
            

            foreach (Documents doc in doclist)
            {                
                path = Resources.FilesPath + folderName + @"\" + doc.docName;
                lines = System.IO.File.ReadAllLines(path);
                stratOfFile = "S";
                prevLanguage = "";
                prevDomainCode = "";
                totalViews = 0;

                foreach (string line in lines)
                {
                    words = line.Split(' ');
                    
                    domainStr = words[0].Split("."); //Get only the domain
                    if (domainStr.Length > 1)
                    {
                        language = domainStr[0]; 
                        domainCode = domainStr[1];
                    }
                    else
                    {
                        language = domainStr[0]; 
                        domainCode = "";
                    }
                    
                    page = words[1];

                    cantViews  = Convert.ToUInt64(words[2]);
                    allFilesViews = allFilesViews + cantViews;

                    if (stratOfFile == "S")
                    {
                        totalViews = totalViews + cantViews;

                        prevLanguage   = language;
                        prevDomainCode = domainCode;
                        stratOfFile = "N";

                    }
                    else
                    {
                        if (language != prevLanguage)
                        {
                            LanDomList = updateData(LanDomList, DomainsList, prevDomainCode, folderName, prevLanguage, totalViews);                            
                            totalViews = 0;

                            //new domain
                            totalViews = totalViews + cantViews;

                            prevLanguage   = language;
                            prevDomainCode = domainCode;
                        }
                        else //same laguage
                        {
                            if (domainCode != prevDomainCode) //
                            {
                                //Store domain total                                
                                LanDomList = updateData(LanDomList, DomainsList, prevDomainCode, folderName, prevLanguage, totalViews);                                
                                totalViews = 0;

                                //new domain
                                totalViews = totalViews + cantViews;
                            }
                            else //same DomainCode
                            {
                                totalViews = totalViews + cantViews;
                            }
                            prevLanguage   = language;
                            prevDomainCode = domainCode;
                        }
                    }
                    //second part
                    //pageViewList = updateDatePageView(pageViewList, folderName, page, totalViews);
                }
                //last domain                
                LanDomList = updateData(LanDomList, DomainsList, prevDomainCode, folderName, prevLanguage, totalViews);               


                path = "";
                Array.Clear(lines, 0, lines.Length);
            }            

            List<LanguageDomain> orderList = LanDomList.OrderBy(x => x.language).ToList();
            List<pageViews> orderListPageViwes = pageViewList.OrderBy(x => x.page).ToList();            

            return (orderList, orderListPageViwes);
        }

        public static List<LanguageDomain> updateData(List<LanguageDomain> LanDomList, List<Domain> DomainsList, string prevDomainCode, string  folderName, string prevLanguage, ulong totalViews)
        {
            bool flag = false;
            Domain auxDomain = new Domain();
            string domTrailing;
            auxDomain = DomainsList.Find( x => x.code == prevDomainCode);
            domTrailing = auxDomain.trailing;

            foreach (LanguageDomain lanDom in LanDomList)
            {
                if (lanDom.period == folderName && lanDom.language == prevLanguage && lanDom.domain == domTrailing)
                {
                    //Object already exits in the List
                    lanDom.viewCount = lanDom.viewCount + totalViews;
                    flag = true;       
                    break;             
                }
            }

            if (flag != true)
            {
                //Add new item to the list
                LanDomList.Add( new LanguageDomain {period = folderName, language = prevLanguage, code = prevDomainCode, domain = domTrailing, viewCount = totalViews});
            }             
            return LanDomList;
        }

        public static List<pageViews> updateDatePageView(List<pageViews> pageViewList, string folderName, string pPage,ulong totalViews)
        {

            bool flag = false;

            foreach (pageViews pView in pageViewList)
            {
                if (pView.period == folderName && pView.page == pPage)
                {
                    pView.viewCount = pView.viewCount + totalViews;
                    flag = true;
                    break;
                }
            }

            if (flag != true)
            {
                pageViewList.Add( new pageViews {period = folderName, page = pPage, viewCount = totalViews });
            }
            return pageViewList;
        }        
    }

}