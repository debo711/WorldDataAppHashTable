using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharedClasses
{
    public class CodeIndexBackup
    {
        public static short Max_N_Home_Loc;
        public static short HomeCnt;
        public static short CollCnt { get; set; }

        public static void Save(Node[] countryData, int max_n_home_loc, int nhome, int ncount)
        {
            Max_N_Home_Loc = (short)max_n_home_loc;
            HomeCnt = (short)nhome;
            CollCnt = (short)ncount;

            FileStream BinFile = new FileStream(".\\..\\..\\..\\As3SavedCodeIndex.bin", FileMode.Create, FileAccess.Write);
            BinaryWriter BinWriter = new BinaryWriter(BinFile);

            BinWriter.Write(Max_N_Home_Loc);              //header
            BinWriter.Write(HomeCnt);
            BinWriter.Write(CollCnt);
            int i = 0;
            char[] subarray = { '0', '0', '0' };

            while (i <= (Max_N_Home_Loc + CollCnt) - 1) // write out index
            {
                if (countryData[i].DRP != 0)
                {
                    BinWriter.Write(countryData[i].KeyValue);
                    BinWriter.Write(Convert.ToInt16(countryData[i].DRP));
                    BinWriter.Write(Convert.ToInt16(countryData[i].HeadPtr));
                }
                else
                {
                    BinWriter.Write(subarray);
                    BinWriter.Write(Convert.ToInt16(0));
                    BinWriter.Write(Convert.ToInt16(0));
                }
                i++;
            }

            BinFile.Close();
        }

        public static CodeIndex Load()
        {
            FileStream BinFile = new FileStream(".\\..\\..\\..\\As3SavedCodeIndex.bin", FileMode.Open, FileAccess.Read);
            BinaryReader BinReader = new BinaryReader(BinFile);

            CodeIndex Index = new CodeIndex();

            Max_N_Home_Loc = BinReader.ReadInt16();
            HomeCnt = BinReader.ReadInt16();
            CollCnt = BinReader.ReadInt16();

            char [] keyvalue;
            int drp;
            int link;

            for (int i = 0; i <= (Max_N_Home_Loc + CollCnt - 1); i++)
            {
                keyvalue = BinReader.ReadChars(3);
                drp = BinReader.ReadInt16();
                link = BinReader.ReadInt16();

                Index.HashTable[i] = new Node(keyvalue, drp, link);
            }

            BinFile.Close();

            return Index;
        }

    }
}
