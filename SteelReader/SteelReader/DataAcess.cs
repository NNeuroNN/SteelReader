using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteelReader
{
    class DataAcess
    {



        public string getQuery(EzAnnotation annot)
        {
            string.Format("Insert into * Values({0},{1},{2})",annot.Author,annot.AContent,annot.ADate);


            return "";
        }

    }
}
