//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NovelHub.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NovelCategory
    {
        public int NovelID { get; set; }
        public int CategoryID { get; set; }
        public string Note { get; set; }
    
        public virtual Novel Novel { get; set; }
        public virtual Category Category { get; set; }
    }
}