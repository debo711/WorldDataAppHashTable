using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharedClasses;

namespace SetUpProg
{
    public class SetUpProg
    {
        public static void Main(string[] args)
        {

            StreamReader SR;
            UserInterface UI = new UserInterface();
            ActualData Outfile = new ActualData();
            CodeIndex CI = new CodeIndex();
            short Ncounter = 0;

            SR = File.OpenText(@"C:\Users\obeezy\Desktop\CS 3310 File Structures\Assign3\As3CountryData.csv");

            string lineread = SR.ReadLine();

            char[] code;
            int RRN;

            UI.WriteToLog("### LOG FILE just opened (in previous instruction)");
            UI.WriteToLog("### SETUP PROG started using As3CountryData.csv");

            while (lineread != null)
            {
               Outfile.Insert(lineread , out code, out RRN);
               CI.Insert(code, RRN);

               Ncounter++;
               lineread = SR.ReadLine();
            }
            
            Outfile.WriteHeaderRec(Ncounter);

            
            CI.WriteToBackUp();
            UI.WriteToLog("### SETUP PROG has finished writing to As3ActualData.bin");
            UI.WriteToLog("### SETUP PROG dumped index to As3SavedCodeIndex.bin");
            UI.WriteToLog("### SETUP PROG finished - " + Ncounter + " input records processed");
            UI.WriteToLog("### LOG FILE closed (in next instruction)\r\n");

            UI.CloseLogFile();
            CI.WriteToBackUp();
            Outfile.FinishUp();
            SR.Close();

        }
    }
}
