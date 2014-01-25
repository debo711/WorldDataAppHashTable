using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharedClasses;

namespace UserProg
{
    public class UserProg
    {
        public static void Main(string[] args)
        {
            UserInterface UI = new UserInterface();
            CodeIndex Index = CodeIndexBackup.Load();
            ActualData DataFile = new ActualData();
            StreamReader SR;

            int transcounter = 0;

            SR = File.OpenText(@"C:\Users\obeezy\Desktop\CS 3310 File Structures\Assign3\As3Trans.txt");

            UI.WriteToLog("### LOG FILE just opened (in previous instruction)");
            UI.WriteToLog("### USER PROG started using As3Trans.txt");
            UI.WriteToLog("### USER PROG loaded index from As3SavedCodeIndex.bin\r\n");
            string lineread = SR.ReadLine();

            while (lineread != null)
            {
                string[] LineRet = UI.GetTrans(lineread);
                string Trans = LineRet[0];

                if (Trans == "QI")
                {
                    int RecsVisited;
                    int DataFileAddress;
                    int id = int.Parse(LineRet[1]);
                    bool found = DataFile.Query(id, out RecsVisited, out DataFileAddress);

                    if (found)
                    {
                        UI.WriteToLog(Trans + " " + id);
                        DataFile.GetData(DataFileAddress, UI);
                        UI.WriteToLog("\t\t"+ RecsVisited + " data records read\r\n");
                    }
                    else
                    {
                        UI.WriteToLog(Trans + " " + id);
                        UI.WriteToLog("ERROR - NO COUNTRY WITH ID: " + id);
                        UI.WriteToLog("\t\t"+ RecsVisited + " data records read\r\n");
                    }

                }
                else if (Trans == "QC")
                {
                    int DRP;
                    int NodesVisited;

                    char[] Code = LineRet[1].ToCharArray();
                    bool found = Index.Query(Code,out DRP, out NodesVisited);

                    if (found)
                    {
                        UI.WriteToLog(string.Format("{0} {1}", Trans, new string(Code))); 
                        DataFile.GetData(DRP, UI);
                        UI.WriteToLog("\t\t"+ NodesVisited + " index nodes visited\r\n");
                    }
                    else
                    {
                        UI.WriteToLog(string.Format("{0} {1}", Trans, new string(Code)));
                        UI.WriteToLog(string.Format("ERROR - NO COUNTRY WITH CODE: {0}",new string (Code)));
                        UI.WriteToLog("\t\t"+ NodesVisited + " index nodes visited\r\n");
                    }
                }
                else if (Trans == "IN")
                {
                    char[] countrycode;
                    int rrn;

                    DataFile.Insert(LineRet[1], out countrycode, out rrn);
                    Index.Insert(countrycode, rrn);

                    UI.WriteToLog(Trans + " " + LineRet[1]);
                    UI.WriteToLog("OK, record inserted\r\n");
                }
                else if (Trans == "DE")
                {
                    UI.WriteToLog(Trans + " " + LineRet[1]);
                    DataFile.Delete(int.Parse(LineRet[1]), UI);
                }
                transcounter++;
                lineread = SR.ReadLine();
            }

            UI.WriteToLog("### USER PROG finished - " + transcounter + " transactions processed");
            UI.WriteToLog("### LOG FILE just closed (in next instruction)\n");

            SR.Close();
            UI.CloseLogFile();
            DataFile.FinishUp();
        }
    }
}
