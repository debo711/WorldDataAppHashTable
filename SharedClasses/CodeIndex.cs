using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public class CodeIndex
    {
        static int MAX_N_HOME_LOC = 20;
        public Node[] HashTable = new Node[300];
        int nColl = 0;
        int nHome = 0; 
        int nextEmpty = MAX_N_HOME_LOC;

        /**********************************************************************************************************/
        public CodeIndex()
        {
            for (int i = 0; i < MAX_N_HOME_LOC; i++)
            {
                HashTable[i] = new Node();
            }
        }

        /***********************************************************************************************************/
        public void Insert(char[] Code, int RRN)
        {
            int TableLoc = HashFunction(Code);
            if (HashTable[TableLoc].KeyValue == null)
            {
                HashTable[TableLoc] = new Node(Code, RRN);
                nHome++;
            }
            else
            {
                HashTable[nextEmpty] = new Node(Code, RRN);
                HashTable[nextEmpty].HeadPtr = HashTable[TableLoc].HeadPtr;
                HashTable[TableLoc].HeadPtr = nextEmpty;
                nColl++;
                nextEmpty = MAX_N_HOME_LOC + nColl;
            }
        }
        
        /*************************************************************************************************************/
        public void Delete(char [] code, UserInterface UI)
        {
            UI.WriteToLog("SORRY, DELETE method not yet coded");
        }
        
        /**********************************************************************************************************/
        public bool Query(char [] CountryCode, out int DRP, out int NodeVisitor)
        {
            bool Hit = false;
            DRP = 0;
            NodeVisitor = 0;

            string cc = new string(CountryCode);
            string KV;

            for (int i = 0; i <= (MAX_N_HOME_LOC + CodeIndexBackup.CollCnt) - 1; i++)
            {
                KV = new string(HashTable[i].KeyValue);
                NodeVisitor++;
                if (cc == KV)
                {
                    Hit = true;
                    DRP = HashTable[i].DRP;
                    break;
                }
            }

            return Hit;
        }

        /******************************************************************************************************************/
        private int HashFunction(char[] code)
        {
            int letter1 = (int)code[0];
            int letter2 = (int)code[1];
            int letter3 = (int)code[2];

            int AsciiMulti = letter1 * letter2 * letter3;

            int homeAddress = AsciiMulti % MAX_N_HOME_LOC;
            return homeAddress;
        }

        /******************************************************************************************************************/
        public void WriteToBackUp()
        {
            CodeIndexBackup.Save(HashTable, MAX_N_HOME_LOC, nHome, nColl);
        }
    }
}
