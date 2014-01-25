using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedClasses
{
    public class UserInterface
    {
        StreamWriter SW = new StreamWriter(".\\..\\..\\..\\Log.txt", true);

        public UserInterface()
        {
        }

        public string[] GetTrans(string aline)
        {
            char[] splitter = { ' ' };
            string[] splitline = aline.Split(splitter,2);

            return splitline;
        }

        public void WriteToLog(string aline)
        {
            SW.WriteLine(aline);
        }

        public void CloseLogFile()
        {
            SW.Close();
        }
    }
}
