using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Projekat1TVP
{
    class Datoteka
    {
        public static void Upisi(Object o, string putanja)
        {
            FileStream fs = File.Create(putanja);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, o);
            fs.Flush();
            fs.Close();
        }

        public static Object Citaj(string putanja)
        {
            if (File.Exists(putanja))
            {
                FileStream fs = File.Open(putanja, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                Object o = bf.Deserialize(fs);
                fs.Flush();
                fs.Close();
                
                return o;
            }
            else { return null; }
                
        }

        public static bool PostojiLi(string putanja)
        {
            return File.Exists(putanja);
        }



    }
}
