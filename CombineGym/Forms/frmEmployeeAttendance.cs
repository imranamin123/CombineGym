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
    public partial class frmEmployeeAttendance : Form
    {
        public frmEmployeeAttendance()
        {
            InitializeComponent();
        }

        int BranchId = 1;
        private DataTable dtAttendance = new DataTable();

        private void txtEmployeeId_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(e.KeyChar) == 13)
                {
                    if (txtEmployeeId.Text != string.Empty)
                    {
                        searchEmployee();
                    }
                }
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
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

        private void clearEmployeeDetails()
        {

            txtEmployeeId.Text = string.Empty;
            dtAttendanceDate.Value = DateTime.Now;
            txtEmployeeName.Text = string.Empty;
            cboGender.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            dtJoiningDate.Value = DateTime.Now;
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
        private void searchEmployee()
        {
            try
            {
                int employeeId = Convert.ToInt32(txtEmployeeId.Text);
                clearEmployeeDetails();
                txtEmployeeId.Text = employeeId.ToString();
                DALSetup dal = new DALSetup();
                if (txtEmployeeId.Text == string.Empty)
                {
                    MessageBox.Show("Please enter employee id");
                    txtEmployeeId.Focus();
                    return;
                }
                else
                {

                    Employee employee = dal.EmployeeGet(employeeId);
                    if (employee == null)
                    {
                        MessageBox.Show("Employee not found for this id");
                        txtEmployeeId.Focus();
                        return;
                    }
                    else
                    {
                        txtEmployeeName.Text = employee.Name;
                        cboGender.SelectedValue = employee.GenderId;
                        cboStatus.SelectedValue = employee.StatusId;
                        cboShift.SelectedValue = employee.GymShiftId;
                        dtJoiningDate.Value = employee.DateOfJoining.Value;



                        string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                        string exeDir = System.IO.Path.GetDirectoryName(exePath);
                        DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                        DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                        string appFolder = appFolderDirInfo.ToString();

                        string imageLocation = employee.Picture;
                        pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;

                        DateTime attendanceDate = DateTime.Now;
                        MarkAttendance(BranchId, employee.EmployeeId, attendanceDate);

                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        private void MarkAttendance(int branchId, int employeeId, DateTime employeeAttendanceDate)
        {
            DALAttendance dal = new DALAttendance();

            EmployeeAttendance employeeAttendance = dal.GetEmployeeAttendance(branchId, employeeId, employeeAttendanceDate.Date);

            if (employeeAttendance == null || employeeAttendance.EmployeeAttendanceId == 0)
            {
                employeeAttendance = new EmployeeAttendance();
                employeeAttendance.EmployeeId = employeeId;
                employeeAttendance.BranchId = branchId;
                employeeAttendance.TimeIn = DateTime.Now;
                employeeAttendance.AttendanceDate = DateTime.Now;
                dal.MarkAttendance(employeeAttendance);
                LoadAttendaceList();
                //MessageBox.Show("Employee checked in", "Attendance");
            }
            else if (employeeAttendance != null && employeeAttendance.TimeOut == null)
            {
                employeeAttendance.TimeOut = DateTime.Now;
                dal.MarkAttendance(employeeAttendance);
                LoadAttendaceList();
                //MessageBox.Show("Employee checked out", "Attendance");
            }
            else
            {
                MessageBox.Show("Employee is already checked out", "Attendance");
            }


        }

        private void frmEmployeeAttendance_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBoxes();
                setAttendanceGridColumns();
                //LoadAttendaceByDateList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        public void ClearDetailForm()
        {
            txtEmployeeName.Text = string.Empty;
            cboGender.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            cboShift.SelectedIndex = 0;
            dtJoiningDate.Text = string.Empty;
            dtTimeIn.Text = string.Empty;
            dtTimeOut.Text = string.Empty;
            //dtFeeHistory.Clear();
            removePicture();

        }

        private void setAttendanceGridColumns()
        {
            dtAttendance.Columns.Add(new DataColumn("EmployeeId", typeof(int)));
            dtAttendance.Columns.Add(new DataColumn("Name", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeIn", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeOut", typeof(string)));
            dtAttendance.Columns.Add(new DataColumn("TimeSpent", typeof(string)));

            grdAttendance.DataSource = dtAttendance;

        }

        private void LoadAttendaceList()
        {
            DALAttendance dal = new DALAttendance();
            dtAttendance.Clear();
            List<spEmployeeAttendanceList_Result> EmployeeAttendanceListResults = dal.GetEmployeeAttendanceListResult(BranchId, dtAttendanceDate.Value, dtAttendanceDate.Value);
            //grd.DataSource = EmployeeListResult;
            if (EmployeeAttendanceListResults != null && EmployeeAttendanceListResults.Count > 0)
            {
                foreach (spEmployeeAttendanceList_Result EmployeeAttendanceListResult in EmployeeAttendanceListResults)
                {
                    var row = dtAttendance.NewRow();
                    row["EmployeeId"] = EmployeeAttendanceListResult.EmployeeId;
                    row["Name"] = EmployeeAttendanceListResult.Name;
                    row["TimeIn"] = EmployeeAttendanceListResult.TimeIn;
                    row["TimeOut"] = EmployeeAttendanceListResult.TimeOut;
                    row["TimeSpent"] = EmployeeAttendanceListResult.TimeSpend;
                    dtAttendance.Rows.Add(row);
                }
            }

        }

       
    }
}
