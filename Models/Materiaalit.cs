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
    
    public partial class Materiaalit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materiaalit()
        {
            this.Tuotteet = new HashSet<Tuotteet>();
        }
    
        public int MateriaaliID { get; set; }
        public string Materiaali { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tuotteet> Tuotteet { get; set; }
        public string MateriaaliIDMateriaali { get; set; }
    }
}
