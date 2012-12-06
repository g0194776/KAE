using System;
using ConsoleApplication1.Configurations;
using KJFramework.Logger;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new Exception("test");
            }
            catch (Exception ex)
            {
                Logs.Logger.Log(ex);
            }
            ConnectionsSection section = ConnectionsSection.Current;
            Console.ReadLine();
        }
    }
}
