using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedClasses
{
    public class ActualData
    {
        private static FileStream BinaryFile;
        private static BinaryWriter BinWriter;
        private static BinaryReader BinReader;

        short MAX_N_LOC = 30;
        private int sizeOfHeaderRec = 2 + 2;
        private int sizeOfDataRec = 2 + 3 + 17 + 11 + 10 + 4 + 2 + 8 + 4 + 4;

        public short N { get; set; }

        private short ID;
        private char[] Code = new char[3];
        private char[] Name = new char[17];
        private char[] Continent = new char[11];
        private char[] Region = new char[10];
        private int SurfaceArea;
        private short YearOfIndep;
        private long Population;
        private float LifeExp;
        private int GNP;

        public ActualData()
        {
            BinaryFile = new FileStream(".\\..\\..\\..\\As3ActualData.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinReader = new BinaryReader(BinaryFile);
            BinWriter = new BinaryWriter(BinaryFile);
        }
        
        /******************************************************************************************************************/
        public void WriteHeaderRec(short n)
        {
            N = n;

            BinWriter.Seek(0, SeekOrigin.Begin);

            BinWriter.Write(N);
            BinWriter.Write(MAX_N_LOC);
        }

        /********************************************************************************************************************/
        public void Insert(string lineread, out char[] code, out int RRN)
        {
            char[] separator = { ',' };
            string[] splitline = lineread.Split(separator, StringSplitOptions.None);

            short CountryId = Convert.ToInt16(splitline[0]);

            int HomeAddress = HashFunction(CountryId);

            ReadARec(HomeAddress);               //read to see if box is empty
            while (ID != 0)                       //if it isnt, loop till there is an empty box
            {
                if (HomeAddress == 30)                    //if its at the end, start at beginning of data file
                    HomeAddress = 1;
                else
                    HomeAddress++;                        //else check box in front of current box
                ReadARec(HomeAddress);
            }
                       
            char[] CountryCode = splitline[1].ToCharArray();
            code = CountryCode;

            char[] CountryName = new char[17];
            if (splitline[2].Length < 17)
            {
                char[] NameBuffer = splitline[2].ToCharArray();
                for (int i = 0; i < NameBuffer.Length; i++)
                {
                    CountryName[i] = NameBuffer[i];
                }
            }
            else
            {
                char[] Namebuffer = splitline[2].Substring(0, 11).ToCharArray();
                for (int i = 0; i < Namebuffer.Length; i++)
                {
                    CountryName[i] = Namebuffer[i];
                }
            }

            char[] continent = new char[11];
            if (splitline[3].Length < 11)
            {
                char[] ConBuffer = splitline[3].ToCharArray();
                for (int i = 0; i < ConBuffer.Length; i++)
                {
                    continent[i] = ConBuffer[i];
                }
            }
            else
            {
                char[] ConBuffer = splitline[3].Substring(0, 11).ToCharArray();
                for (int i = 0; i < ConBuffer.Length; i++)
                {
                    continent[i] = ConBuffer[i];
                }
            }

            char[] region = new char[10];
            if (splitline[4].Length < 10)
            {
                char[] RegBuffer = splitline[4].ToCharArray();
                for (int i = 0; i < RegBuffer.Length; i++)
                {
                    region[i] = RegBuffer[i];
                }
            }
            else
            {
                char[] RegBuffer = splitline[4].Substring(0, 10).ToCharArray();
                for (int i = 0; i < RegBuffer.Length; i++)
                {
                    region[i] = RegBuffer[i];
                }
            }

            int surfacearea = Convert.ToInt32(splitline[5]);

            short yearofindep;
            if (splitline[6] == "NULL")
                yearofindep = 0;
            else
                yearofindep = Convert.ToInt16(splitline[6]);

            long population = Convert.ToInt64(splitline[7]);

            float lifeexp;
            if (splitline[8] == "NULL")
                lifeexp = 0;
            else
                lifeexp = Convert.ToSingle(splitline[8]);

            int gnp = Convert.ToInt32(splitline[9]);

            ID = CountryId;
            Code = CountryCode;
            Name = CountryName;
            Continent = continent;
            Region = region;
            SurfaceArea = surfacearea;
            YearOfIndep = yearofindep;
            Population = population;
            LifeExp = lifeexp;
            GNP = gnp;

            WriteARec(HomeAddress);
            RRN = HomeAddress;
        }

        /*************************************************************************************************************/
        public void Delete(int id, UserInterface UI)
        {
            UI.WriteToLog("SORRY, DELETE method not yet coded.\r\n");
        }

        /**************************************************************************************************************/
        public bool Query(int CountryId, out int RecVisited, out int Address)
        {
            RecVisited = 0;
            Address = HashFunction(CountryId);
            bool hit = false;
            int counter = 0;

            ReadARec(Address);               //read to see if box is empty
            while (counter < MAX_N_LOC && ID !=0)                       //if it isnt, loop till there is an empty box
            {
                counter++;
                RecVisited++;
                if (CountryId == ID)
                {
                    hit = true;
                    break;
                }
                if (Address == 30)                    //if its at the end, start at beginning of data file
                    Address = 1;
                else
                    Address++;                        //else check box in front of current box
                ReadARec(Address);
            }

            return hit;
        }

        /****************************************************************************************************************/
        public void GetData(int drp, UserInterface UI)
        {
            ReadARec(drp);
            MakeNiceRecToPrint(UI);
        }

        /****************************************************************************************************************/
        private void ReadARec(int rrn)
        {
            int byteOffset = sizeOfHeaderRec + ((rrn - 1) * sizeOfDataRec);
            BinReader.BaseStream.Seek(byteOffset, SeekOrigin.Begin);

            try
             {
                ID = BinReader.ReadInt16();
                Code = BinReader.ReadChars(3);
                Name = BinReader.ReadChars(17);
                Continent = BinReader.ReadChars(11);
                Region = BinReader.ReadChars(10);
                SurfaceArea = BinReader.ReadInt32();
                YearOfIndep = BinReader.ReadInt16();
                Population = BinReader.ReadInt64();
                LifeExp = BinReader.ReadSingle();
                GNP = BinReader.ReadInt32();
            }
            catch
            {
                ID = 0;
                Code = null;
                Name = null;
                Continent = null;
                Region = null;
                SurfaceArea = 0;
                YearOfIndep = 0;
                Population = 0;
                LifeExp = 0;
                GNP = 0;
            }
        }

        /*****************************************************************************************************************/
        private void WriteARec(int rrn)
        {
            int byteOffset = sizeOfHeaderRec + ((rrn - 1) * sizeOfDataRec);
            BinWriter.Seek(byteOffset, SeekOrigin.Begin);

            BinWriter.Write(ID);
            BinWriter.Write(Code);
            BinWriter.Write(Name);
            BinWriter.Write(Continent);
            BinWriter.Write(Region);
            BinWriter.Write(SurfaceArea);
            BinWriter.Write(YearOfIndep);
            BinWriter.Write(Population);
            BinWriter.Write(LifeExp);
            BinWriter.Write(GNP);
        }

        /*****************************************************************************************************************/
        private void MakeNiceRecToPrint(UserInterface UI)
        {
            UI.WriteToLog(string.Format("{0:000} {1} {2} |{3} |{4}| " +
                           "{5,10:N0}| {6,5}| {7,13:N0}| {8,4:N1}| {9,9:N0} ",
                           ID, new string(Code), new string(Name),
                           new string(Continent), new string(Region),
                           SurfaceArea, YearOfIndep, Population, LifeExp, GNP));
        }

        /********************************************************************************************************/
        private int HashFunction(int id)
        {
            int homeAddress = id % MAX_N_LOC;
            if (homeAddress == 0)
                homeAddress = MAX_N_LOC;
            return homeAddress;
           }

        /**********************************************************************************************************/
        public void FinishUp()
        {
            BinaryFile.Close();
        }
    }
}
