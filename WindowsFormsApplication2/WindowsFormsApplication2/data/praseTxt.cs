using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication2.data
{
    class praseTxt
    {
        FileStream p;
      
        StreamReader read;
        public praseTxt(string address)
        {
            p = File.Open(address, FileMode.Open, FileAccess.Read);
            p.Position = 0;
            read = new StreamReader(p);
        }


        public void close()
        {
            if (p.CanRead)
                p.Close();
            read.Close();
        }

        public List<string> getNextLine()
        {
            string str = read.ReadLine();
            if (str != null)
            {
                return str.Split('\t').ToList();
            }
            else
                throw new EndOfStreamException();
        }

    }

}
