using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
namespace nuevoProyecto.Models
{
    public class Global
    {
        public string Id { get; set; }
        public string Tabla { get; set; }
        public Dictionary<string, string> Variables = new Dictionary<string, string>();
        public string Int1 { get; set; }
        public string Int2 { get; set; }
        public string Int3 { get; set; }
        public string VarChar1 { get; set; }
        public string VarChar2 { get; set; }
        public string VarChar3 { get; set; }
        public string DT1 { get; set; }
        public string DT2 { get; set; }
        public string DT3 { get; set; }

       public Global()
        {
            Id = "";
            Tabla = "";
            Int1 = "";
            Int2 ="";
            Int3 = "";
            VarChar1 ="";
            VarChar2= "";
            VarChar3 ="";
            DT1="";
            DT2 ="";
            DT3 ="";
            Variables = new Dictionary<string, string>();
        }

        public Dictionary<string,string> Nombre_Variable(Dictionary<string, string> DiccionarioArbol)
        {
            var DiccionarioDevuelve = new Dictionary<string, string>();

                int ints = 1;
                int varchar = 1;
                int DT= 1;
            foreach (var item in DiccionarioArbol)
            {
                if (item.Value=="INT")
                {
                   
                    DiccionarioDevuelve.Add(item.Key,("Int"+ ints.ToString()));
                    ints++;
                }
                else if (item.Value == "DATETIME")
                {
                    var c = DT.ToString();
                    DiccionarioDevuelve.Add(item.Key, ("DT" + DT.ToString()));
                    DT++;
                }
                else 
                {
                    var c = varchar.ToString();
                    DiccionarioDevuelve.Add(item.Key, ("VarChar" + varchar.ToString()));
                    varchar++;
                }
            }
            return DiccionarioDevuelve;
        }

        public int CompareTo(object obj)
        {
            var comparable = (Global)obj;
            return Id.CompareTo(comparable.Id);
        }


    }
}