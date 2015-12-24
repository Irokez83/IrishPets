using System;

namespace IrishPets.Models
{
    public class Logger
    {
        public static string LogDirectoryPath = Environment.CurrentDirectory;

        public static void Log(string _lines)
        {
            try
            {
                var __file = new System.IO.StreamWriter($"{LogDirectoryPath}\\Error.log", true);
                __file.WriteLine($"{DateTime.Now.ToString("yyyy.MM.dd HH:mm")} --> {_lines}");
                __file.Close();
            }
            catch
            {
            }
        }
    }
}