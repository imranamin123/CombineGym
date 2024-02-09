using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CombineGym.Forms
{
    public partial class frmStudentAttendance : Form
    {
        int BranchId = 1;
        private DataTable dtAttendance = new DataTable();
        public frmStudentAttendance()
        {
            InitializeComponent();
        }

        private void txtStudentId_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(e.KeyChar) == 13)
                {
                    if(txtStudentId.Text != string.Empty)
                    {
                        searchStudent();
                    }
                }
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FillComboBoxes()
        {
            DALSetup dal = new DALSetup();
            dal.fillGenderComboBox(ref cboGender);
            dal.fillStatusComboBox(ref cboStatus);
            //cboFeeMonth.SelectedValue = DateTime.Now.Month;
            dal.fillShiftComboBox(ref cboShift);
        }

        private void clearStudentDetails()
        {
            
            txtStudentId.Text = string.Empty;
            dtAttendanceDate.Value = DateTime.Now;
            txtStudentName.Text = string.Empty;
            cboGender.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dtAdmissionDate.Value = DateTime.Now;
            cboShift.SelectedIndex = 0;
            dtTimeIn.Value = DateTime.Now;
            dtTimeOut.Value = DateTime.Now;

            removePicture();
        }

        private void removePicture()
        {
            if (pictureBox1.Image != null)
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDir = System.IO.Path.GetDirectoryName(exePath);
                DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                string appFolder = appFolderDirInfo.ToString();
                string ExistingFilename = appFolder + "\\Images\\NoImage.jpg";
                //pictureBox1.Image.Dispose();
                pictureBox1.ImageLocation = ExistingFilename;

            }
        }
        private void searchStudent()
        {
            int studentId = Convert.ToInt32(txtStudentId.Text);
            clearStudentDetails();
            txtStudentId.Text = studentId.ToString();
            DALSetup dal = new DALSetup();
            if (txtStudentId.Text == string.Empty)
            {
                MessageBox.Show("Please enter student id");
                txtStudentId.Focus();
                return;
            }
            else
            {

                Student student = dal.StudentGet(studentId);
                if (student == null)
                {
                    MessageBox.Show("Student not found for this id");
                    txtStudentId.Focus();
                    return;
                }
                else
                {
                    txtStudentName.Text = student.Name;
                    cboGender.SelectedValue = student.GenderId;
                    cboStatus.SelectedValue = student.StatusId;
                    cboShift.SelectedValue = student.GymShiftId;
                    dtAdmissionDate.Value = student.AdmissionDate.Value;



                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string exeDir = System.IO.Path.GetDirectoryName(exePath);
                    DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                    DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                    string appFolder = appFolderDirInfo.ToString();

                    string imageLocation = student.Picture;
                    pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;

                    DateTime attendanceDate = DateTime.Now;
                    MarkAttendance(BranchId, student.StudentId, attendanceDate);
                    
                }
            }

        }


        private void MarkAttendance(int branchId, int studentId, DateTime studentAttendanceDate)
        {
            DALAttendance dal = new DALAttendance();

            StudentAttendance studentAttendance = dal.GetStudentAttendance(branchId, studentId, studentAttendanceDate.Date);

            if(studentAttendance == null || studentAttendance.StudentAttendanceId == 0)
            {
                studentAttendance = new StudentAttendance();
                studentAttendance.StudentId = studentId;
                studentAttendance.BranchId = branchId;
                studentAttendance.TimeIn = DateTime.Now;
                studentAttendance.AttendanceDate = DateTime.Now;
                dal.MarkAttendance(studentAttendance);
                LoadAttendaceByDateList();
                //MessageBox.Show("Student checked in", "Attendance");
            }
            else if(studentAttendance != null && studentAttendance.TimeOut == null)
            {
                studentAttendance.TimeOut = DateTime.Now;
                dal.MarkAttendance(studentAttendance);
                LoadAttendaceByDateList();
                //MessageBox.Show("Student checked out", "Attendance");
            }
            else
            {
                MessageBox.Show("Student is already checked out","Attendance");
            }

            
        }

        private void frmStudentAttendance_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBoxes();
                setAttendanceGridColumns();
                LoadAttendaceByDateList();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void ClearDetailForm()
        {
            txtStudentName.Text = string.Empty;
            cboGender.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            cboShift.SelectedIndex = 0;
            dtAdmissionDate.Text = string.Empty;
            dtTimeIn.Text = string.Empty;
            dtTimeOut.Text = string.Empty;
            //dtFeeHistory.Clear();
            removePicture();

        }

        private void setAttendanceGridColumns()
        {
            dtAttendance.Columns.Add(new DataColumn("StudentId", typeof(int)));
            dtAttendance.Columns.Add(new DataColumn("Name", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeIn", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeOut", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeSpent", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("FeeDueDate", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("FeeDue", typeof(string)));

            grdAttendance.DataSource = dtAttendance;

        }

        private void LoadAttendaceByDateList()
        {
            DALAttendance dal = new DALAttendance();
            dtAttendance.Clear();
            List<spStudentAttendanceListByDate_Result> StudentAttendanceListByDateResults = dal.GetStudentAttendanceByDateResult(BranchId,dtAttendanceDate.Value,dtAttendanceDate.Value);
            //grd.DataSource = StudentListResult;
            if (StudentAttendanceListByDateResults != null && StudentAttendanceListByDateResults.Count > 0)
            {
                foreach (spStudentAttendanceListByDate_Result StudentAttendanceListByDateResult in StudentAttendanceListByDateResults)
                {
                    var row = dtAttendance.NewRow();
                    row["StudentId"] = StudentAttendanceListByDateResult.StudentId;
                    row["Name"] = StudentAttendanceListByDateResult.Name;
                    row["TimeIn"] = StudentAttendanceListByDateResult.TimeIn;
                    row["TimeOut"] = StudentAttendanceListByDateResult.TimeOut;
                    row["TimeSpent"] = StudentAttendanceListByDateResult.TimeSpend;
                    row["FeeDueDate"] = StudentAttendanceListByDateResult.FeeDueDate;
                    if(StudentAttendanceListByDateResult.IsPending == true)
                    {
                        row["FeeDue"] = "Fee Due";
                        
                        dtAttendance.Rows.Add(row);
                        int index = dtAttendance.Rows.IndexOf(row);
                        grdAttendance.Rows[index].DefaultCellStyle.BackColor = Color.Pink;
                        grdAttendance.Refresh();
                        //grdAttendance.Rows[grdAttendance.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Pink;
                    }
                    else
                    {
                        row["FeeDue"] = string.Empty;
                        dtAttendance.Rows.Add(row);
                    }

                }
            }

        }

        //private void LoadAttendanceList(int? branchId, int? studentId)
        //{
        //    DALFeeReceipt dal = new DALFeeReceipt();
        //    //dt = new DataTable();
        //    dtFeeHistory.Clear();
        //    List<spFeeReceiptHistory_Result> FeeHistoryResults = dal.FeeReceiptHistoryResult(branchId, studentId);
        //    //grd.DataSource = StudentListResult;
        //    if (FeeHistoryResults != null && FeeHistoryResults.Count > 0)
        //    {
        //        foreach (spFeeReceiptHistory_Result feeHistoryResult in FeeHistoryResults)
        //        {
        //            var row = dtFeeHistory.NewRow();
        //            row["Year"] = feeHistoryResult.FeeYear;
        //            row["Month"] = feeHistoryResult.FeeMonth;
        //            row["Date"] = feeHistoryResult.ReceiptDate.Value.ToString("dd/MM/yyyy");
        //            row["Previous"] = feeHistoryResult.PreviousBalance;
        //            row["Received"] = feeHistoryResult.ReceiptAmount;
        //            row["Balance"] = feeHistoryResult.BalanceAmount;
        //            // row["ShiftTiming"] = StudentResult.shi;
        //            dtFeeHistory.Rows.Add(row);
        //        }
        //    }

        //}



    }
}
