using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CombineGymDAL.DAL
{
    public class DALSetup
    {
        //private CombineGymEntities db = new CombineGymEntities();

        //#region Branch
        //public List<Branch> BranchGetList()
        //{
        //    try
        //    {
        //        var BranchList = db.Branches.ToList();
        //        return BranchList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public Branch BranchGet(Int64 BranchId)
        //{
        //    try
        //    {
        //        var Branch = db.Branches.Where(x => x.BranchId == BranchId).FirstOrDefault();
        //        return Branch;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public bool BranchDelete(Int64 BranchId)
        //{
        //    try
        //    {
        //        var Branch = db.Branches.Where(x => x.BranchId == BranchId).FirstOrDefault();
        //        db.Branches.Remove(Branch);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public Branch BranchSave(Branch Branch)
        //{
        //    try
        //    {
        //        if (Branch != null)
        //        {
        //            //db.Branches.AddOrUpdate(Branch);
        //            if(Branch.BranchId == 0)
        //            {
        //                db.Entry(Branch).State = System.Data.Entity.EntityState.Added;
        //            }
        //            else
        //            {
        //                db.Entry(Branch).State = System.Data.Entity.EntityState.Modified;
        //            }
                    
        //            db.SaveChanges();
        //            return Branch;
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region Employee
        ////public List<sp_EmployeeGetEmployeeList2_Result> EmployeeGetListResult(int employeeId, string name, int branchId)
        ////{
        ////    try
        ////    {
        ////        var EmployeeListResult = db.sp_EmployeeGetEmployeeList2(employeeId,name,branchId).ToList();
        ////        return EmployeeListResult;
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex;
        ////    }
        ////}
        //public Employee EmployeeGet(Int64 EmployeeId)
        //{
        //    try
        //    {
        //        var Employee = db.Employees.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
        //        return Employee;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public bool EmployeeDelete(Int64 EmployeeId)
        //{
        //    try
        //    {
        //        var Employee = db.Employees.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
        //        db.Employees.Remove(Employee);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public Employee EmployeeSave(Employee Employee)
        //{
        //    try
        //    {
        //        if (Employee != null)
        //        {
        //            //db.Employeees.AddOrUpdate(Employee);
        //            if (Employee.EmployeeId == 0)
        //            {
        //                db.Entry(Employee).State = System.Data.Entity.EntityState.Added;
        //            }
        //            else
        //            {
        //                db.Entry(Employee).State = System.Data.Entity.EntityState.Modified;
        //            }

        //            db.SaveChanges();
        //            return Employee;
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region Lists
        //public List<Gender> GetGenderList()
        //{
        //    var genderList = db.Genders.ToList();
        //    return genderList;
        //}
        //public List<Status> GetStatusList()
        //{
        //    var statusList = db.Status.ToList();
        //    return statusList;
        //}
        #endregion
    }
}
