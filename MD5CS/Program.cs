using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MD5CS
{
    class Program
    {     

        static void Main(string[] args)
        {
            string Message;
          
            Console.WriteLine("Press key");
            Message = Console.ReadLine();

            string result;
                    
            MD5Sharp md5 = new MD5Sharp(Message);
            result = md5.calcMD5();

            Console.WriteLine($"Result: {result}");
                    
            Console.ReadKey(); 
                   
        }
    }
}
