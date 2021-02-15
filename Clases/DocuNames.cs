
using System;
using System.Collections.Generic;  
using System.IO;
 
namespace Tranzact_assigment
{
    class DocuNames    
    {
        List<Documents> DocuList;        
        public DocuNames() 
        {            
            this.DocuList = new List<Documents>();
        }        

        public List<Documents> getDocumentList(DateTime prevDay, int ite, string folderName){
            int cantFile = 3; //request that the server admited at the same time
            string name = "";
            string url  = "";
            string hour = "";
            string fullPath = "";            
            
            for (int i=0; i<cantFile; i++)
            {                
                fullPath = Resources.FilesPath + folderName + @"\";
                hour = getHour(ite);
                if (hour == "00")
                {
                    prevDay = prevDay.AddDays(1);
                    name = Resources.preName + prevDay.Year.ToString() + prevDay.Month.ToString("00") + prevDay.Day.ToString("00") + "-" + hour + "0000.gz";
                }
                else
                {
                    name = Resources.preName + prevDay.Year.ToString() + prevDay.Month.ToString("00") + prevDay.Day.ToString("00") + "-" + hour + "0000.gz";
                }
                url = Resources.URL + prevDay.Year.ToString() + "/" + prevDay.Year.ToString() + "-" + prevDay.Month.ToString("00")  + "/" + name;

                fullPath = fullPath + name;                
                if (!File.Exists(fullPath))
                {
                    Documents doc = new Documents(name, url);
                    this.DocuList.Add(doc);
                }                

                name = "";
                url = "";
                fullPath = "";
                ite = ite + 1;
            }
            return this.DocuList;
        } 

            public List<Documents> getDocumentListToRead(DateTime prevDay){
            int cantFile = 24; //files per day
            string name = "";
            string url  = "";
            string hour = "";
            
            for (int i=0; i<cantFile; i++)
            {                
                hour = getHour(i+1);
                if (hour == "00")
                {
                    prevDay = prevDay.AddDays(1);
                    name = Resources.preName + prevDay.Year.ToString() + prevDay.Month.ToString("00") + prevDay.Day.ToString("00") + "-" + hour + "0000";
                }
                else
                {
                    name = Resources.preName + prevDay.Year.ToString() + prevDay.Month.ToString("00") + prevDay.Day.ToString("00") + "-" + hour + "0000";
                }               
                Documents doc = new Documents(name, url);
                this.DocuList.Add(doc);
                name = "";
                url = "";                
            }
            return this.DocuList;
        } 


        static string getHour(int hour){
            string hourstr = "";
            if (hour >= 10)
            {
                if (hour == 24)
                {
                    hourstr = "00";
                }
                else
                {   
                    hourstr = hour.ToString();
                }
            }
            else
            {
                hourstr = "0" + hour.ToString();
            }

            return hourstr;
        }
    }
}