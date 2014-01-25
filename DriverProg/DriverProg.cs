using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UserProg;
using DisplayProg;
using SetUpProg;


namespace DriverProg
{
    class DriverProg
    {
        static void Main(string[] args)
        {
            File.Delete(".//..//..//..//Log.txt");
            File.Delete(".//..//..//..//As3ActualData.bin");
            File.Delete(".//..//..//..//As3SavedCodeIndex.bin");

            string[] array = new string[0];

            SetUpProg.SetUpProg.Main(array);
            UserProg.UserProg.Main(array);
            DisplayProg.DisplayProg.Main(array);
           
        }
    }
}
