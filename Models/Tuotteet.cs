//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MateriaaliVarasto.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    public partial class Tuotteet
    {
        public int TuoteID { get; set; }
        public string Tuotenimi { get; set; }
        public Nullable<bool> Pesty { get; set; }
        public string Määrä { get; set; }
        public Nullable<int> RyhmäID { get; set; }
        public Nullable<int> MateriaaliID { get; set; }
        public Nullable<int> ValmistajaID { get; set; }
        public string ImageLink { get; set; }
        public Nullable<int> LoginId { get; set; }
        public byte[] Kuva { get; set; }
    
        public virtual Materiaalit Materiaalit { get; set; }
        public virtual Ryhmät Ryhmät { get; set; }
        public virtual Valmistajat Valmistajat { get; set; }
        public virtual Logins Logins { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
