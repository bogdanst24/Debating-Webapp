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
    
    public partial class Argument
    {
        public int Id { get; set; }
        public sbyte Side { get; set; }
        public string Content { get; set; }
        public string Date_created { get; set; }
        public int Opinion_id { get; set; }
        public string User_email { get; set; }
    
        public virtual Opinion Opinion { get; set; }
        public virtual User User { get; set; }
    }
}
