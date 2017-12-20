//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DatabaseAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class DebateInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DebateInfo()
        {
            this.CategoryDebates = new HashSet<CategoryDebate>();
            this.Votes = new HashSet<Vote>();
        }
    
        public int debate_id { get; set; }
        public string subject { get; set; }
        public string date_created { get; set; }
        public string state { get; set; }
        public string description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoryDebate> CategoryDebates { get; set; }
        public virtual Content Content { get; set; }
        public virtual RoundState RoundState { get; set; }
        public virtual UserDebate UserDebate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vote> Votes { get; set; }
    }
}