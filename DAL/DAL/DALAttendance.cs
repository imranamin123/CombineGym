using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALAttendance
    {

        private CombineGymEntities db = new CombineGymEntities();

        #region Student
        public StudentAttendance GetStudentAttendance(int branchId, int studentId , DateTime attdDate)
        {
            try
            {
                //List<temp> t = db.temps.ToList();
                StudentAttendance atd = new StudentAttendance();
                foreach (var atnd in db.StudentAttendances)
                {
                    if(atnd.StudentId == studentId && atnd.BranchId == branchId && atnd.AttendanceDate.Value.Date.ToString("dd/MM/yyyy") == attdDate.ToString("dd/MM/yyyy"))
                    {
                        atd = atnd;
                        break;
                    }
                }

                return atd;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void MarkAttendance(StudentAttendance studentAttendance)
        {
            try
            {
                //int branchId = studentAttendance.BranchId.Value;
                //int studentId = studentAttendance.StudentId.Value;
                //DateTime attdDate = studentAttendance.AttendanceDate.Value.Date;

                //StudentAttendance atd = db.StudentAttendances.Where(x => x.BranchId == branchId && x.StudentId == studentId && x.AttendanceDate == attdDate).FirstOrDefault();

                if(studentAttendance.StudentAttendanceId == 0)
                {
                    db.Entry(studentAttendance).State = System.Data.Entity.EntityState.Added;
                }
                else 
                {
                    db.Entry(studentAttendance).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spStudentAttendanceListByDate_Result> GetStudentAttendanceByDateResult(Nullable<int> branchId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            try
            {
                var StudentAttendanceListByDateResult = db.spStudentAttendanceListByDate(branchId, fromDate, toDate).ToList();
                return StudentAttendanceListByDateResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Employee
        public EmployeeAttendance GetEmployeeAttendance(int branchId, int employeeId, DateTime attdDate)
        {
            try
            {

                EmployeeAttendance employeeAttendance = null;

                //List<temp> t = db.temps.ToList();

                //EmpAttendance atd = new EmpAttendance();
                //CombineGymEntities ab = new CombineGymEntities();
                //List<Emp> empAttd =  db.EmpAttendances.ToList();
                //if(empAttd == null || empAttd.Count <=0)
                //{
                //    return null;
                //}
                foreach (var atnd in db.EmployeeAttendances)
                {
                    if (atnd.EmployeeId == employeeId && atnd.BranchId == branchId && atnd.AttendanceDate.Value.Date.ToString("dd/MM/yyyy") == attdDate.ToString("dd/MM/yyyy"))
                    {
                        employeeAttendance = atnd;
                        break;
                    }
                }

                return employeeAttendance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MarkAttendance(EmployeeAttendance employeeAttendance)
        {
            try
            {

                if (employeeAttendance.EmployeeAttendanceId == 0)
                {
                    db.Entry(employeeAttendance).State = System.Data.Entity.EntityState.Added;
                }
                else
                {
                    db.Entry(employeeAttendance).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<spEmployeeAttendanceList_Result> GetEmployeeAttendanceListResult(Nullable<int> branchId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            try
            {
                var EmployeeAttendanceListResult = db.spEmployeeAttendanceList(branchId, fromDate, toDate).ToList();
                return EmployeeAttendanceListResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


    }
}
