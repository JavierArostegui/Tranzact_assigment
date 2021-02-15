using System;  
using System.IO;
 
namespace Tranzact_assigment
{
    static class Resources
    {
        public static string FilesPath = Directory.GetCurrentDirectory() + @"\Files\";
        public static string URL = @"https://dumps.wikimedia.org/other/pageviews/"; 
        public static string preName = @"pageviews-";
        public static int numberDays = 5;
        public static DateTime today = DateTime.Now;
    }
}