using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MateriaaliVarasto.ViewModel
{
    public class ProductEdit
    {
        public int TuoteID { get; set; }
        public string Tuotenimi { get; set; }
        public Nullable<bool> Pesty { get; set; }
        public string Määrä { get; set; }
        public Nullable<int> RyhmäID { get; set; }
        public Nullable<int> MateriaaliID { get; set; }

        public string Materiaali {  get; set; }
        public Nullable<int> ValmistajaID { get; set; }
        public string Valmistaja { get; set; }
        public string ImageLink { get; set; }
    }
}