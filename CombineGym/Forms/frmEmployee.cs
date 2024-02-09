using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using System.IO;

namespace CombineGym.Forms
{
    public partial class frmEmployee : Form
    {
        Image file;

        private DataTable dt = new DataTable();

        public frmEmployee()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm() == true)
                {
                    Save();
                    Utilities.saveImages(Convert.ToInt32(txtEmployeeId.Text), pictureBox1, "Employee");
                    LoadEmployeeList(0, string.Empty, 1);
                    MessageBox.Show("Record saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setGridColumns()
        {
            dt.Columns.Add(new DataColumn("EmployeeId", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Shift", typeof(string)));
            dt.Columns.Add(new DataColumn("ShiftTiming", typeof(string)));

            grd.DataSource = dt;

        }

        private void LoadEmployeeList(int employeeId=0, string name= "", int branchId=1)
        {
            DALSetup dal = new DALSetup();
            //dt = new DataTable();
            dt.Clear();
            List<spEmployeeList_Result> employeeListResult = dal.EmployeeGetListResult(employeeId, name, branchId);
            //grd.DataSource = employeeListResult;
            if (employeeListResult.Count > 0)
            {
                foreach (spEmployeeList_Result employeeResultList in employeeListResult)
                {
                    var row = dt.NewRow();
                    row["EmployeeId"] = employeeResultList.EmployeeId;
                    row["Name"] = employeeResultList.Name;
                    row["Shift"] = employeeResultList.ShiftDescription;
                    row["ShiftTiming"] = employeeResultList.ShiftTiming;
                    dt.Rows.Add(row);
                }
            }

        }
        private void frmEmployee_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBoxes();
                //LoadEmployee(1);
                setGridColumns();
                // List
                LoadEmployeeList(0, string.Empty, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Save()
        {
            Employee employee = null;
            DALSetup dal = new DALSetup();

            int employeeId = txtEmployeeId.Text == string.Empty ? 0 : Convert.ToInt32(txtEmployeeId.Text);
            employee = dal.EmployeeGet(employeeId);
            if (employee == null)
            {
                employee = new Employee();
            }


            employee.EmployeeId = txtEmployeeId.Text == string.Empty ? 0 : Convert.ToInt32(txtEmployeeId.Text);
            employee.Name = txtEmployeeName.Text;
            employee.FatherName = txtFather.Text;
            employee.Phone1 = txtFather.Text;
            employee.Phone2 = txtPhone1.Text;
            employee.GenderId = Convert.ToInt16(cboGender.SelectedValue);
            employee.StatusId = Convert.ToInt16(cboStatus.SelectedValue);
            employee.DateOfJoining = dtDateOfJoining.Value;
            employee.DateOfLeaving = dtDateOfLeaving.Value;
            employee.Remarks = txtRemarks.Text;
            employee.Address = txtAddress.Text;

            employee.DateOfJoining = dtDateOfJoining.Value;
            employee.DateOfLeaving = dtDateOfLeaving.Value;
            employee.StatusId = Convert.ToInt16(cboStatus.SelectedValue);
            employee.GenderId = Convert.ToInt16(cboGender.SelectedValue);
            employee.GymShiftId = Convert.ToInt16(cboShift.SelectedValue);
            employee.BranchId = 1;

            employee = dal.EmployeeSave(employee);
            if (employee != null)
            {
                txtEmployeeId.Text = Convert.ToString(employee.EmployeeId);
            }
            
        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            if (txtEmployeeName.Text == string.Empty)
            {
                MessageBox.Show("Please enter employee name");
                IsValid = false;
                txtEmployeeName.Focus();
            }
            else if (txtFather.Text == string.Empty)
            {
                MessageBox.Show("Please enter father name");
                IsValid = false;
                txtFather.Focus();
            }
            else if (txtPhone1.Text == string.Empty)
            {
                MessageBox.Show("Please enter at lease one phone no");
                IsValid = false;
                txtPhone1.Focus();
            }

            else if (txtAddress.Text == string.Empty)
            {
                MessageBox.Show("Please enter Employee address");
                IsValid = false;
                txtEmployeeName.Focus();
            }

            return IsValid;
        }

        private void FillComboBoxes()
        {
            DALSetup dal = new DALSetup();
            dal.fillGenderComboBox(ref cboGender);
            dal.fillStatusComboBox(ref cboStatus);
            dal.fillBranchComboBox(ref cboBranch);
            dal.fillShiftComboBox(ref cboShift);

        }

        private void fillBranchComboBox()
        {
            cboBranch.DataSource = new DALSetup().GetBranchList();
            cboBranch.DisplayMember = "BranchName";
            cboBranch.ValueMember = "BranchId";
        }

        private void LoadEmployee(int id)
        {
            DALSetup dal = new DALSetup();
            Employee employee = dal.EmployeeGet(id);
            if (employee != null)
            {
                txtEmployeeId.Text = Convert.ToString(employee.EmployeeId);
                txtEmployeeName.Text = employee.Name;
                txtFather.Text = employee.FatherName;
                txtPhone1.Text = employee.Phone1;
                txtPhone2.Text = employee.Phone2;
                cboGender.SelectedValue = employee.GenderId;
                cboStatus.SelectedValue = employee.StatusId;
                cboBranch.SelectedValue = employee.BranchId;

                if (employee.DateOfJoining == null)
                {
                    dtDateOfJoining.Text = string.Empty;
                }
                else
                {
                    dtDateOfJoining.Value = Convert.ToDateTime(employee.DateOfJoining);
                }
                if (employee.DateOfLeaving == null)
                {
                    dtDateOfLeaving.Text = string.Empty;
                }
                else
                {
                    dtDateOfLeaving.Value = Convert.ToDateTime(employee.DateOfLeaving);
                }

                txtAddress.Text = employee.Address;


                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDir = System.IO.Path.GetDirectoryName(exePath);
                DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                string appFolder = appFolderDirInfo.ToString();
                string imageLocation = employee.Picture;
                pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int branchId = 1;
            int EmployeeId = txtSearchEmpId.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSearchEmpId.Text.Trim());
            string Name = txtSearchName.Text.Trim();

            LoadEmployeeList(EmployeeId, Name, branchId);
        }

       

        private void grd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //txtClassID.Text = grd.Rows[e.RowIndex].Cells[0].Value.ToString();
                //txtName.Text = grd.Rows[e.RowIndex].Cells[1].Value.ToString();
                //btnDelete.Enabled = true;

                int EmployeeId = Convert.ToInt32(grd.Rows[e.RowIndex].Cells[0].Value);
                LoadEmployee(EmployeeId);
                tabTab.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDetailForm();
                ClearListForm();
                LoadEmployeeList();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearListForm()
        {
            dt.Clear();
            txtSearchEmpId.Text = string.Empty;
            txtSearchName.Text = string.Empty;
            LoadEmployeeList(0, string.Empty, 1);
        }
        public void ClearDetailForm()
        {
            txtEmployeeId.Text = string.Empty;
            txtEmployeeName.Text = string.Empty;
            txtFather.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            cboGender.SelectedValue = string.Empty;
            cboStatus.SelectedValue = string.Empty;
            cboShift.SelectedValue = string.Empty;
            dtDateOfJoining.Text = string.Empty;
            dtDateOfLeaving.Text = string.Empty;

            txtAddress.Text = string.Empty;

            removePicture();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            ClearDetailForm();
        }

        private void txtSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnPicSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "JPG (*.JPG)|*.jpg| png (*.png)|*.png"; //"Text files (*.txt)|*.txt|All files (*.*)|*.*"'
            //f.Filter = "JPG (*.JPG)|*.jpg|*.png";
            if (f.ShowDialog() == DialogResult.OK)
            {
                file = Image.FromFile(f.FileName);
                pictureBox1.Image = file;
            }
        }

        private void btnPicRemove_Click(object sender, EventArgs e)
        {
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
    }
}
