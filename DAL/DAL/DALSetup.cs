using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DAL
{
    public class DALSetup
    {
        private CombineGymEntities db = new CombineGymEntities();

        #region Branch
        public List<Branch> BranchGetList()
        {
            try
            {
                var BranchList = db.Branches.ToList();
                return BranchList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Branch BranchGet(Int64 BranchId)
        {
            try
            {
                var Branch = db.Branches.Where(x => x.BranchId == BranchId).FirstOrDefault();
                return Branch;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool BranchDelete(Int64 BranchId)
        {
            try
            {
                var Branch = db.Branches.Where(x => x.BranchId == BranchId).FirstOrDefault();
                db.Branches.Remove(Branch);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Branch BranchSave(Branch Branch)
        {
            try
            {
                if (Branch != null)
                {
                    //db.Branches.AddOrUpdate(Branch);
                    if (Branch.BranchId == 0)
                    {
                        db.Entry(Branch).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry(Branch).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    return Branch;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Employee
        public List<spEmployeeList_Result> EmployeeGetListResult(int employeeId, string name, int branchId)
        {
            try
            {
                var EmployeeListResult = db.spEmployeeList(employeeId, name, branchId).ToList();
                return EmployeeListResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Employee EmployeeGet(Int64 EmployeeId)
        {
            try
            {
                var Employee = db.Employees.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                return Employee;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool EmployeeDelete(Int64 EmployeeId)
        {
            try
            {
                var Employee = db.Employees.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                db.Employees.Remove(Employee);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Employee EmployeeSave(Employee Employee)
        {
            try
            {
                if (Employee != null)
                {
                    //db.Employeees.AddOrUpdate(Employee);
                    if (Employee.EmployeeId == 0)
                    {
                        db.Entry(Employee).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry(Employee).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    return Employee;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Lists
        public List<Gender> GetGenderList()
        {
            var genderList = db.Genders.ToList();
            return genderList;
        }
        public List<Status> GetStatusList()
        {
            var statusList = db.Status.ToList();
            return statusList;
        }
        public List<GymShift> GetShiftList()
        {
            var shiftList = db.GymShifts.ToList();
            return shiftList;
        }

        public List<Branch> GetBranchList()
        {
            var branchList = db.Branches.ToList();
            return branchList;
        }


        public List<BloodGroup> GetBloodGroupList()
        {
            var BllodGroupist = db.BloodGroups.ToList();
            return BllodGroupist;
        }

        public List<FeeYear> GetFeeYearList()
        {
            var FeeYearList = db.FeeYears.ToList();
            return FeeYearList;
        }

        public List<FeeMonth> GetFeeMonthList()
        {
            var FeeMonthList = db.FeeMonths.ToList();
            return FeeMonthList;
        }

        #endregion

        #region Dropdowns
        public  void fillGenderComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetGenderList();
            cmb.DisplayMember = "Description";
            cmb.ValueMember = "GenderId";
        }

        public  void fillStatusComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetStatusList();
            cmb.DisplayMember = "Description";
            cmb.ValueMember = "StatusId";
        }

        public  void fillShiftComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetShiftList();
            cmb.DisplayMember = "Description";
            cmb.ValueMember = "GymShiftId";
        }

        public  void fillBranchComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetBranchList();
            cmb.DisplayMember = "BranchName";
            cmb.ValueMember = "BranchId";
        }

        public void fillEmployeeComboBox(int branchId, ref ComboBox cmb)
        {
            List<spEmployeeList_Result> EmployeeListResult = new DALSetup().EmployeeGetListResult(0, string.Empty, branchId);
            var empDropdownList = EmployeeListResult.Select(x => new { EmployeeId = x.EmployeeId, EmployeeName = x.Name }).ToList();

            cmb.DataSource = empDropdownList;
            cmb.DisplayMember = "EmployeeName";
            cmb.ValueMember = "EmployeeId";
            cmb.SelectedIndex = -1;
        }
        public void fillFeeYearComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetFeeYearList().OrderByDescending(x => x.FeeYear1).ToList();
            cmb.DisplayMember = "FeeYear1";
            cmb.ValueMember = "FeeYearId";
        }
        public  void fillFeeMonthComboBox(ref ComboBox cmb)
        {
            cmb.DataSource = new DALSetup().GetFeeMonthList();
            cmb.DisplayMember = "FeeMonth1";
            cmb.ValueMember = "FeeMonthId";
        }
        #endregion

        #region Student

        public List<spStudentList_Result> StudentListResult(Nullable <int> branchId, Nullable<int> studentId, string name, string phone, Nullable<Int16> status, Nullable<Int16> gender, Nullable<Int16> shift)
        {
            try
            {
                var EmployeeListResult = db.spStudentList(branchId, studentId, name, phone, status, gender,shift).ToList();
                return EmployeeListResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Student StudentGet(Int32 StudentId)
        {
            try
            {
                var Student = db.Students.Where(x => x.StudentId == StudentId).FirstOrDefault();
                return Student;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool StudentDelete(Int64 StudentId)
        {
            try
            {
                var Student = db.Students.Where(x => x.StudentId == StudentId).FirstOrDefault();
                db.Students.Remove(Student);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Student StudentSave(Student Student)
        {
            try
            {
                if (Student != null)
                {
                    //db.Studentes.AddOrUpdate(Student);
                    if (Student.StudentId == 0)
                    {
                        db.Entry(Student).State = System.Data.Entity.EntityState.Added;
                    }
                    else
                    {
                        db.Entry(Student).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    return Student;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region User
        public List<spUserList_Result> UserListResult(int employeeId, string name, int branchId)
        {
            try
            {
                var UserListResult = db.spUserList(employeeId, name, branchId).ToList();
                return UserListResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public secUser UserGet(Int64 UserId, Int32 branchId)
        {
            try
            {
                var user = db.secUsers.Where(x => x.UserId == UserId && x.BranchId == branchId).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public secUser UserGet(Int32 branchId, string username, string password)
        {
            try
            {
                var user = db.secUsers.Where(x => x.BranchId == branchId && x.username == username && x.password == password).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool userDelete(Int32 userId, Int32 branchId)
        {
            try
            {
                var user = db.secUsers.Where(x => x.UserId == userId && x.BranchId == branchId).FirstOrDefault();
                db.secUsers.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public secUser userSave(secUser user)
        {
            try
            {
                if (user != null)
                {
                    //db.Employeees.AddOrUpdate(Employee);
                    if (user.UserId == 0)
                    {
                        user.CreatedDate = DateTime.Now;
                        db.Entry(user).State = System.Data.Entity.EntityState.Added;

                    }
                    else
                    {
                        user.ModifiedDate = DateTime.Now;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    }

                    db.SaveChanges();
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
