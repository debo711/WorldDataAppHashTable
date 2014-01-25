using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public class Node
    {
        public char[] KeyValue { get; set; }
        public int DRP { get; set; }
        public int HeadPtr { get; set; }

        public Node()
        {
            KeyValue = null;
            DRP = 0;
            HeadPtr = -1;
        }

        public Node(char[] countrycode, int rrn)
        {
            KeyValue = countrycode;
            DRP = rrn;
            HeadPtr = -1;
        }

        public Node(char[] countrycode, int rrn, int link)
        {
            KeyValue = countrycode;
            DRP = rrn;
            HeadPtr = link;
        }
    }
}
