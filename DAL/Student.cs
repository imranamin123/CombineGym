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
    
    public partial class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string FaherName { get; set; }
        public Nullable<short> GenderId { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public Nullable<System.DateTime> FeeDueDate { get; set; }
        public Nullable<decimal> FeeAmount { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Address { get; set; }
        public Nullable<short> BloodGroupId { get; set; }
        public string Picture { get; set; }
        public string Remarks { get; set; }
        public Nullable<short> StatusId { get; set; }
        public Nullable<short> GymShiftId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> BranchId { get; set; }
    }
}
