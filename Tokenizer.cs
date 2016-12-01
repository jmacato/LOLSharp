using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LOLpreter
{
    class Tokenizer
    {
        public Dictionary<int, string[]> ProgTokenTable = new Dictionary<int, string[]>();

        public void Tokenize(string raw)
        {
            ProgTokenTable.Clear();
            foreach(string curline in raw.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var linenum = ProgTokenTable.Count + 1;
                ProgTokenTable.Add(linenum, curline.Split(' '));
            }

            foreach(string[] x in ProgTokenTable.Values)
            {
                Debug.WriteLine("-->" + String.Join("|",x));
            }

        }
    }
}
