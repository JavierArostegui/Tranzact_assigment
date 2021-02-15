namespace Tranzact_assigment
{
    class pageViews
    { 
        public string period {get; set;}
        public string page {get; set;}               
        public ulong viewCount{get; set;}
        
        public string getLine()
        {
            return period + "\t " + page + "\t\t " + viewCount.ToString();
        }
    }
}