//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class secUserRight
    {
        public int UserRightsId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<int> PageNo { get; set; }
        public Nullable<bool> IsView { get; set; }
        public Nullable<bool> IsAdd { get; set; }
        public Nullable<bool> IsEdit { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}