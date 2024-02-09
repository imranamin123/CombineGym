﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class CombineGymEntities : DbContext
    {
        public CombineGymEntities()
            : base("name=CombineGymEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<secUser> secUsers { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<GymShift> GymShifts { get; set; }
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<FeeMonth> FeeMonths { get; set; }
        public virtual DbSet<FeeYear> FeeYears { get; set; }
        public virtual DbSet<FeeReceipt> FeeReceipts { get; set; }
        public virtual DbSet<StudentAttendance> StudentAttendances { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<secMenu> secMenus { get; set; }
        public virtual DbSet<secPage> secPages { get; set; }
        public virtual DbSet<secUserRight> secUserRights { get; set; }
    
        public virtual ObjectResult<spEmployeeList_Result> spEmployeeList(Nullable<int> employeeId, string name, Nullable<int> branchId)
        {
            var employeeIdParameter = employeeId.HasValue ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spEmployeeList_Result>("spEmployeeList", employeeIdParameter, nameParameter, branchIdParameter);
        }
    
        public virtual ObjectResult<spStudentList_Result> spStudentList(Nullable<int> branchId, Nullable<int> studentId, string name, string phone, Nullable<short> status, Nullable<short> gender, Nullable<short> shift)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("branchId", branchId) :
                new ObjectParameter("branchId", typeof(int));
    
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("studentId", studentId) :
                new ObjectParameter("studentId", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("phone", phone) :
                new ObjectParameter("phone", typeof(string));
    
            var statusParameter = status.HasValue ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(short));
    
            var genderParameter = gender.HasValue ?
                new ObjectParameter("gender", gender) :
                new ObjectParameter("gender", typeof(short));
    
            var shiftParameter = shift.HasValue ?
                new ObjectParameter("shift", shift) :
                new ObjectParameter("shift", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spStudentList_Result>("spStudentList", branchIdParameter, studentIdParameter, nameParameter, phoneParameter, statusParameter, genderParameter, shiftParameter);
        }
    
        public virtual ObjectResult<spFeeReceiptHistorySearch_Result> spFeeReceiptHistorySearch(Nullable<int> branchId, Nullable<int> studentId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spFeeReceiptHistorySearch_Result>("spFeeReceiptHistorySearch", branchIdParameter, studentIdParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<Nullable<decimal>> spGetStdentPrevBal(Nullable<int> branchId, Nullable<int> studentId, Nullable<int> feeMonth, Nullable<int> feeYear)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("branchId", branchId) :
                new ObjectParameter("branchId", typeof(int));
    
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("studentId", studentId) :
                new ObjectParameter("studentId", typeof(int));
    
            var feeMonthParameter = feeMonth.HasValue ?
                new ObjectParameter("feeMonth", feeMonth) :
                new ObjectParameter("feeMonth", typeof(int));
    
            var feeYearParameter = feeYear.HasValue ?
                new ObjectParameter("feeYear", feeYear) :
                new ObjectParameter("feeYear", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("spGetStdentPrevBal", branchIdParameter, studentIdParameter, feeMonthParameter, feeYearParameter);
        }
    
        public virtual ObjectResult<spFeeReceiptHistory_Result> spFeeReceiptHistory(Nullable<int> branchId, Nullable<int> studentId)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var studentIdParameter = studentId.HasValue ?
                new ObjectParameter("StudentId", studentId) :
                new ObjectParameter("StudentId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spFeeReceiptHistory_Result>("spFeeReceiptHistory", branchIdParameter, studentIdParameter);
        }
    
        public virtual ObjectResult<spStudentAttendanceListByDate_Result> spStudentAttendanceListByDate(Nullable<int> branchId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spStudentAttendanceListByDate_Result>("spStudentAttendanceListByDate", branchIdParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<spEmployeeAttendanceList_Result> spEmployeeAttendanceList(Nullable<int> branchId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spEmployeeAttendanceList_Result>("spEmployeeAttendanceList", branchIdParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual ObjectResult<spUserList_Result> spUserList(Nullable<int> userId, string name, Nullable<int> branchId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spUserList_Result>("spUserList", userIdParameter, nameParameter, branchIdParameter);
        }
    }
}