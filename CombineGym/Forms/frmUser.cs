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
    public partial class frmUser : Form
    {
        int branchId = 1;
        bool isLoading = false;

        private DataTable dt = new DataTable();

        public frmUser()
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
                    LoadUserList(0, string.Empty, branchId);
                    MessageBox.Show("Record saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,"Error");
            }
        }

        private void setGridColumns()
        {
            dt.Columns.Add(new DataColumn("UserId", typeof(int)));
            dt.Columns.Add(new DataColumn("Employee", typeof(string)));
            dt.Columns.Add(new DataColumn("username", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Branch", typeof(string)));

            grd.DataSource = dt;

        }

        private void LoadUserList(int employeeId = 0, string name = "", int branchId = 1)
        {
            DALSetup dal = new DALSetup();
            //dt = new DataTable();
            dt.Clear();
            List<spUserList_Result> userListResult = dal.UserListResult(employeeId, name, branchId);
            //grd.DataSource = employeeListResult;
            if (userListResult.Count > 0)
            {
                foreach (spUserList_Result userResultList in userListResult)
                {
                    var row = dt.NewRow();
                    row["UserId"] = userResultList.UserId;
                    row["Employee"] = userResultList.Name;
                    row["username"] = userResultList.username;
                    row["Status"] = userResultList.Status;
                    row["Branch"] = userResultList.BranchName;
                    dt.Rows.Add(row);
                }
            }

        }
        private void Save()
        {
            secUser user = null;
            DALSetup dal = new DALSetup();

            int userId = txtUserId.Text == string.Empty ? 0 : Convert.ToInt32(txtUserId.Text);
            user = dal.UserGet(userId, branchId);
            if (user == null)
            {
                user = new secUser();
            }
            user.UserId = userId;
            user.username = txtUserName.Text;
            user.password= txtPassword.Text;
            user.EmployeeId = Convert.ToInt32(cboEmployee.SelectedValue);
            user.StatusId = Convert.ToInt16(cboStatus.SelectedValue);
            user.Remarks = txtRemarks.Text;
            user.BranchId = Convert.ToInt32(cboBranch.SelectedValue);

            user = dal.userSave(user);
            if (user != null)
            {
                txtUserId.Text = Convert.ToString(user.UserId);
            }

        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            if (txtUserName.Text == string.Empty)
            {
                MessageBox.Show("Please enter username","Information");
                IsValid = false;
                txtUserName.Focus();
            }
            else if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("Please enter password");
                IsValid = false;
                txtPassword.Focus();
            }
            else if (cboEmployee.SelectedIndex == -1)
            {
                MessageBox.Show("Please select employee");
                IsValid = false;
                cboEmployee.Focus();
            }

            else if (cboStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Please select status");
                IsValid = false;
                cboStatus.Focus();
            }

            else if (cboBranch.SelectedIndex == -1)
            {
                MessageBox.Show("Please select branch");
                IsValid = false;
                cboBranch.Focus();
            }
            return IsValid;
        }

        private void FillComboBoxes()
        {
            DALSetup dal = new DALSetup();
            dal.fillStatusComboBox(ref cboStatus);
            dal.fillBranchComboBox(ref cboBranch);
            isLoading = true;
            dal.fillEmployeeComboBox(branchId, ref cboEmployee);
            isLoading = false;
        }


        private void LoadUser(int userId, int branchId)
        {
            DALSetup dal = new DALSetup();
            secUser user = dal.UserGet(userId,branchId);
            if (user != null)
            {
                txtUserId.Text = Convert.ToString(user.UserId);
                txtUserName.Text = user.username;
                txtPassword.Text = user.password;
                cboEmployee.SelectedValue = user.EmployeeId;
                cboBranch.SelectedValue = user.BranchId;
                cboStatus.SelectedValue = user.StatusId;
                txtRemarks.Text = user.Remarks;
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDir = System.IO.Path.GetDirectoryName(exePath);
                DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                string appFolder = appFolderDirInfo.ToString();
                string imageLocation = new DALSetup().EmployeeGet(user.EmployeeId.Value).Picture;
                pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int branchId = 1;
            int EmployeeId = txtSearchEmpId.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSearchEmpId.Text.Trim());
            string Name = txtSearchName.Text.Trim();

            LoadUserList(EmployeeId, Name, branchId);
        }

        private void grd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                int userId = Convert.ToInt32(grd.Rows[e.RowIndex].Cells[0].Value);
                LoadUser(userId,branchId);
                tabTab.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDetailForm();
                ClearListForm();
                LoadUserList();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void ClearListForm()
        {
            dt.Clear();
            txtSearchEmpId.Text = string.Empty;
            txtSearchName.Text = string.Empty;
            LoadUserList(0, string.Empty, 1);
        }
        public void ClearDetailForm()
        {
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            //txtConfirmPassword.Text = string.Empty;
            cboEmployee.Text = string.Empty;
            cboBranch.SelectedIndex = -1;
            cboStatus.SelectedValue = 0;
            //dtCreatedDate.Text = string.Empty;
            //dtCreatedDate.Text = string.Empty;

            removePicture();
        }

       
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            try
            {
                ClearDetailForm();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
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
                pictureBox1.ImageLocation = ExistingFilename;

            }
        }

        private void frmUser_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBoxes();
                setGridColumns();

                LoadUserList(0, string.Empty, 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,"Error Message");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearDetailForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
        }

        private void cboEmployee_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isLoading == true) return;
            DALSetup dal = new DALSetup();
            int employeeId = Convert.ToInt32(cboEmployee.SelectedValue);
            Employee employee = new DALSetup().EmployeeGet(employeeId);

            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string exeDir = System.IO.Path.GetDirectoryName(exePath);
            DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
            DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
            string appFolder = appFolderDirInfo.ToString();
            string imageLocation = employee.Picture;
            pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;
        }
    }
}
