namespace Tranzact_assigment
{
    class LanguageDomain
    { 
        public string period {get; set;}
        public string language {get; set;}
        public string code {get; set;}
        public string domain {get; set;}        
        public ulong viewCount{get; set;}        

        public string getLine()
        {
            return period + "\t " + language + "\t\t " + domain + "\t\t " + viewCount.ToString();
        }
    }
}