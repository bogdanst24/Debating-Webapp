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
    
    public partial class CategoryOpinion
    {
        public int Id { get; set; }
        public int Opinion_id { get; set; }
        public int Categ_id { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Opinion Opinion { get; set; }
    }
}
