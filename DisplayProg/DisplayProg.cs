using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DisplayProg
{
    public class DisplayProg
    {
        public static void Main(string[] args)
        {
            // > > > >  FIX PATH FOR YOUR PROJECT (if needed)  < < < <
            string path = ".//..//..//..//";       // i.e., top-level project directory
            string fileNamePrefix;
            string binFileName = "ActualData.bin";

            if (args.Length == 0)               // DEVELOPER IS RUNNING THIS PROGRAM
            {
                fileNamePrefix = "As3";
            }
            else
                fileNamePrefix = args[0];       // DRIVER IS RUNNING THIS PROGRAM

            StreamWriter log = new StreamWriter(path + "Log.txt", true);    // append

            FileStream fileStream = new FileStream(path + fileNamePrefix + binFileName,
                                                    FileMode.Open, FileAccess.Read);
            BinaryReader binFile = new BinaryReader(fileStream);

            log.WriteLine("\r\n--------------------------------------------------------");
            log.WriteLine("THE ACTUAL DATA FILE (including empty locations)\r\n");

            short n = binFile.ReadInt16();
            short MAX_N_LOC = binFile.ReadInt16();
            log.WriteLine("N is {0}, MAX_N_LOC is {1}\r\n", n, MAX_N_LOC);

            if (n != 0)
                ShowActualDataRecs(fileStream, binFile, log, n, MAX_N_LOC);
            // this method needs fileStream so it can do random access
            log.WriteLine("\r\n--------------------------------------------------------\r\n");

            binFile.Close();
            log.Close();
            ///////////////////////////////////////
            //Print out SavedCodeIndex
            PrintSavedCodeIndex();
            //End Print
            ///////////////////////////////////////
        }

        //*******************************************************************************
        // PREDICTION:  There will be MAX_N_LOC number of records in the file (not including
        //      the header record) - N of these will be GOOD records, the rest will be
        //      empty locations ("Holes").
        //-------------------------------------------------------------------------------
        private static void ShowActualDataRecs(FileStream fileStr, BinaryReader binFile,
                StreamWriter log, short n, short MAX_N_LOC)
        {
            // USED TO CALCULATE OFFSET FOR RANDOM ACCESS
            //const int sizeOfHeaderRec = 2 + 2;
            //const int sizeOfDataRec = 2 + 3 + 17 + 11 + 10 + 4 + 2 + 8 + 4 + 4;

            // DISPLAY ALL RECORD LOCATIONS 
            log.WriteLine("RRN>> ID  CDE NAME............. CONTINENT.. REGION.... "
                + "......AREA .YEAR ...POPULATION LIFE ......GNP");

            for (int i = 1; i <= MAX_N_LOC; i++)
                ReadPrintADataRec(i, binFile, log);
        }
        //-------------------------------------------------------------------------------
        private static void ReadPrintADataRec(int i, BinaryReader f, StreamWriter log)
        {
            short id;
            char[] code = new char[3];
            char[] name = new char[17];
            char[] continent = new char[11];
            char[] region = new char[10];
            int surfaceArea;
            short yearOfIndep;
            long population;
            float lifeExp;
            int gnp;

            id = f.ReadInt16();
            code = f.ReadChars(3);
            name = f.ReadChars(17);
            continent = f.ReadChars(11);
            region = f.ReadChars(10);
            surfaceArea = f.ReadInt32();
            yearOfIndep = f.ReadInt16();
            population = f.ReadInt64();
            lifeExp = f.ReadSingle();
            gnp = f.ReadInt32();
            if (id == 0)
                log.WriteLine("{0:000}>> ---EMPTY LOCATION---", i);
            else
                log.WriteLine("{0:000}>> {1:000} {2} {3} {4} {5} " +
                    "{6,10:N0} {7,5} {8,13:N0} {9,4:N1} {10,9:N0} ",
                    i, id, new string(code), new string(name),
                    new string(continent), new string(region),
                    surfaceArea, yearOfIndep, population, lifeExp, gnp);
        }
        /* Program:    DisplayProg.cs CS3310 //FOR As3SavedCodeIndex.bin
         * Date:       February/28/2012 
         * Programmer: Michael Wickey
         * About:      Reads in data from SavedCodeIndex binary file, formats it and writes it to log file.
         * !!Notice!!: Only works with the SavedCodeIndex format discussed in class
         * Header Record: short, short, short, Regular Record: char[3], short, short.
         *                          
         * Output sample for As3SavedCodeIndex: 
        --------------------------------------------------------
        THE CODE INDEX FILE (including empty locations)

        MAX_N_HOME_LOC is 20, nHome is 11, nColl is 15
        [LOC]  CODE | DRP | LINK |
        [000]   JPN |  013 | 034 |
        [001]   ATA |  021 | 033 |
        [002]   TCA |  029 | 030 |
        [003]   QAT |  005 | 027 |
         **/
        private static void PrintSavedCodeIndex()
        {
            // > > > >  FIX PATH FOR YOUR PROJECT (if needed)  < < < <
            string path = ".//..//..//..//";       // i.e., top-level project directory
            string binFileName = "As3SavedCodeIndex.bin";

            ////////////////////
            //Record 
            char[] code = new char[3];//code – a 3-char array (fixed-length “string”)
            short drp = 0;//drp – a short
            short link = 0;//link – a short
            //End Record
            ////////////////////

            StreamWriter log = new StreamWriter(path + "Log.txt", true);    // append

            FileStream fileStream = new FileStream(path + binFileName,
                                                    FileMode.Open, FileAccess.Read);
            BinaryReader binFile = new BinaryReader(fileStream);

            log.WriteLine("\r\n--------------------------------------------------------");
            log.WriteLine("THE CODE INDEX FILE \r\n");

            ////////////////////
            //Header One header record containing 3 shorts: MAX_N_HOME_LOC, nHome, nColl
            short MAX_N_HOME_LOC = binFile.ReadInt16();
            short nHome = binFile.ReadInt16();
            short nColl = binFile.ReadInt16();
            //End Header
            ////////////////////
            log.WriteLine("MAX_N_HOME_LOC is {0}, nHome is {1}, nColl is {2} \r\n", MAX_N_HOME_LOC, nHome, nColl);
            log.WriteLine("[LOC]  CODE |  DRP | LINK|");//Write Header to Log file
            for (int i = 0; i <= (MAX_N_HOME_LOC + nColl) - 1; i++)   // FIXED LINE 2/22/12
            {
                    code = binFile.ReadChars(3);
                    drp = binFile.ReadInt16();
                    link = binFile.ReadInt16();
                    if (!char.IsLetter(code[0]))
                    { log.WriteLine("[{0:D3}] ---EMPTY LOCATION---", i); }
                    else
                    {
                        log.Write("[{0:D3}] ", i);
                        log.Write("  {0}{1}{2} | ", code[0], code[1], code[2]);
                        log.Write(" {0:D3} |", drp);
                        if (link == -1)
                        {
                            log.WriteLine(" {0:D2} |", link);
                        }
                        else
                        {
                            log.WriteLine(" {0:D3} |", link);
                        }
                    }
                
            }
            log.WriteLine("\r\n--------------------------------------------------------\r\n");

            log.Close();
            fileStream.Close();
            binFile.Close();

        }
    }
}
