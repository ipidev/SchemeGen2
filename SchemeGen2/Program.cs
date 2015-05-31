using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchemeGen2
{
    class Program
    {
        static void Main(string[] args)
        {
            Scheme testScheme = new Scheme(true);
            testScheme.Initialise();

            byte[] schemeBytes = testScheme.GetBytes();

            try
            {
                FileStream fs = new FileStream("testScheme.wsc", FileMode.Create, FileAccess.Write);

                fs.Write(schemeBytes, 0, schemeBytes.Length);

                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
