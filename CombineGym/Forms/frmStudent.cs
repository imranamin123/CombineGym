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
using System.Reflection;

namespace CombineGym.Forms
{
    public partial class frmStudent : Form
    {
        Image file;

        private DataTable dt = new DataTable();
        //int branchId = 1;

        public frmStudent()
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
                    Utilities.saveImages(Convert.ToInt32(txtStudentId.Text),pictureBox1,"Student");
                    ClearListForm();
                    LoadStudentList(null, null, null, null, null, null,null);
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
            dt.Columns.Add(new DataColumn("StudentId", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Shift", typeof(string)));
            dt.Columns.Add(new DataColumn("Gender", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone", typeof(string)));
            //dt.Columns.Add(new DataColumn("ShiftTiming", typeof(string)));

            grd.DataSource = dt;

        }

        private void LoadStudentList(int? branchId, int? studentId , string name , string phone, Int16? status, Int16? gender,Int16? shift)
        {
            DALSetup dal = new DALSetup();
            //dt = new DataTable();
            dt.Clear();
            List<spStudentList_Result> StudentListResult = dal.StudentListResult( branchId, studentId, name,phone,status, gender,shift);
            //grd.DataSource = StudentListResult;
            if (StudentListResult.Count > 0)
            {
                foreach (spStudentList_Result StudentResult in StudentListResult)
                {
                    var row = dt.NewRow();
                    row["StudentId"] = StudentResult.StudentId;
                    row["Name"] = StudentResult.Name;
                    row["Shift"] = StudentResult.Shift;
                    row["Gender"] = StudentResult.Gender;
                    row["Status"] = StudentResult.Status;
                    row["Phone"] = StudentResult.Phone1;
                    // row["ShiftTiming"] = StudentResult.shi;
                    dt.Rows.Add(row);
                }
            }

        }
        private void frmStudent_Load(object sender, EventArgs e)
        {
            try
            {
                FillComboBoxes();
                setGridColumns();
                LoadStudentList(null,null,null,null,null,null,null);
                dtFeeDate.Value = DateTime.Now.AddDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month ));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Save()
        {
            Student student = null;
            DALSetup dal = new DALSetup();
            int studentId = txtStudentId.Text == string.Empty ? 0 : Convert.ToInt32(txtStudentId.Text);
            student = dal.StudentGet(studentId);
            if(student == null)
            {
                student = new Student();
            }
            student.Name = txtName.Text;
            student.FaherName = txtFather.Text;
            student.Phone1 = txtPhone1.Text;
            student.Phone2 = txtPhone1.Text;
            student.GenderId = Convert.ToInt16(cboGender.SelectedValue);
            student.StatusId = Convert.ToInt16(cboStatus.SelectedValue);
            student.BloodGroupId = Convert.ToInt16(cboBloodGroup.SelectedValue);
            student.AdmissionDate = dtAdmissionDate.Value;
            student.FeeDueDate = dtFeeDate.Value;
            student.FeeAmount = Convert.ToDecimal(txtFeeAmount.Text);
            student.Remarks = txtRemarks.Text;
            student.Address = txtAddress.Text;
            student.StatusId = Convert.ToInt16(cboStatus.SelectedValue);
            student.GenderId = Convert.ToInt16(cboGender.SelectedValue);
            student.GymShiftId = Convert.ToInt16(cboShift.SelectedValue);
            student.BranchId = 1;

            student = dal.StudentSave(student);
            if (student != null)
            {
                txtStudentId.Text = Convert.ToString(student.StudentId);
            }
        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            if (txtName.Text == string.Empty)
            {
                MessageBox.Show("Please enter Student name");
                IsValid = false;
                txtName.Focus();
            }
            else if (txtFeeAmount.Text == string.Empty || txtFeeAmount.Text=="0")
            {
                MessageBox.Show("Please enter student fee");
                IsValid = false;
                txtFeeAmount.Focus();
            }

            //else if (txtFather.Text == string.Empty)
            //{
            //    MessageBox.Show("Please enter father name");
            //    IsValid = false;
            //    txtFather.Focus();
            //}
            //else if (txtPhone1.Text == string.Empty)
            //{
            //    MessageBox.Show("Please enter at lease one phone no");
            //    IsValid = false;
            //    txtPhone1.Focus();
            //}

            //else if (txtAddress.Text == string.Empty)
            //{
            //    MessageBox.Show("Please enter Student address");
            //    IsValid = false;
            //    txtName.Focus();
            //}

            return IsValid;
        }

        private void FillComboBoxes()
        {
            fillGenderComboBox();
            fillStatusComboBox();
            fillBranchComboBox();
            fillShiftComboBox();
            fillBloodGroupComboBox();
        }
        private void fillGenderComboBox()
        {
            List<Gender> genderList = new DALSetup().GetGenderList();

            cboGender.DataSource = genderList;
            cboGender.DisplayMember = "Description";
            cboGender.ValueMember = "GenderId";

            cboSearchGender.DataSource = genderList;
            cboSearchGender.DisplayMember = "Description";
            cboSearchGender.ValueMember = "GenderId";
            cboSearchGender.SelectedIndex = -1;

        }

        private void fillStatusComboBox()
        {
            List<Status> statusList =  new DALSetup().GetStatusList();

            cboStatus.DataSource = statusList;
            cboStatus.DisplayMember = "Description";
            cboStatus.ValueMember = "StatusId";

            cboSearchStatus.DataSource = statusList;
            cboSearchStatus.DisplayMember = "Description";
            cboSearchStatus.ValueMember = "StatusId";
            cboSearchStatus.SelectedIndex = -1;
        }

        private void fillShiftComboBox()
        {
            List<GymShift> shiftList =  new DALSetup().GetShiftList();

            cboShift.DataSource = shiftList;
            cboShift.DisplayMember = "Description";
            cboShift.ValueMember = "GymShiftId";

            cboSearchShift.DataSource = shiftList;
            cboSearchShift.DisplayMember = "Description";
            cboSearchShift.ValueMember = "GymShiftId";
            cboSearchShift.SelectedIndex = -1;
            cboSearchShift.Text = string.Empty;
        }

        private void fillBranchComboBox()
        {
            List<Branch> branchList =  new DALSetup().GetBranchList();

            cboBranch.DataSource = branchList;
            cboBranch.DisplayMember = "BranchName";
            cboBranch.ValueMember = "BranchId";
            
            cboSearchBranch.DataSource = branchList;
            cboSearchBranch.DisplayMember = "BranchName";
            cboSearchBranch.ValueMember = "BranchId";
            cboSearchBranch.SelectedIndex = -1;
        }

        private void fillBloodGroupComboBox()
        {
            List<BloodGroup> BloodGroupList = new DALSetup().GetBloodGroupList();

            cboBloodGroup.DataSource = BloodGroupList;
            cboBloodGroup.DisplayMember = "BloodGroupName";
            cboBloodGroup.ValueMember = "BloodGroupId";

            //cboBloodGroup.DataSource = BloodGroupList;
            //cboBloodGroup.DisplayMember = "Description";
            //cboBloodGroup.ValueMember = "BloodGroupId";
            //cboBloodGroup.SelectedIndex = -1;
        }

        private void LoadStudent(int id)
        {
            DALSetup dal = new DALSetup();
            Student student = dal.StudentGet(id);
            if (student != null)
            {
                txtStudentId.Text = Convert.ToString(student.StudentId);
                txtName.Text = student.Name;
                txtFather.Text = student.FaherName;
                txtPhone1.Text = student.Phone1;
                txtPhone2.Text = student.Phone2;
                cboGender.SelectedValue = student.GenderId;
                cboStatus.SelectedValue = student.StatusId;
                cboShift.SelectedValue = student.GymShiftId;
                cboBranch.SelectedValue = student.BranchId;
                cboBloodGroup.SelectedValue = student.BloodGroupId;
                dtAdmissionDate.Value = student.AdmissionDate.Value;
                dtFeeDate.Value = student.FeeDueDate.Value;
                txtFeeAmount.Text = Convert.ToString(student.FeeAmount);
                txtRemarks.Text = student.Remarks;
                txtAddress.Text = student.Address;


                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeDir = System.IO.Path.GetDirectoryName(exePath);
                DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                string appFolder = appFolderDirInfo.ToString();

                string imageLocation = student.Picture;
                pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtStudentId.Text = string.Empty;
                txtName.Text = string.Empty;
                txtFather.Text = string.Empty;
                txtPhone1.Text = string.Empty;
                txtPhone2.Text = string.Empty;
                txtFeeAmount.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                txtAddress.Text = string.Empty;
                cboGender.SelectedIndex = 0;
                cboStatus.SelectedIndex = 0;
                cboBranch.SelectedIndex = 0;
                cboBloodGroup.SelectedIndex = 0;
                cboShift.SelectedIndex = 2;
                dtAdmissionDate.Value = DateTime.Now;
                dtFeeDate.Value = DateTime.Now;

                removePicture();

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
           try
            {
                ClearDetailForm();
                //ClearListForm();
                fillSearchList();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void fillSearchList()
        {
            //int branchId = 1;


            int studentId = txtSearchEmpId.Text.Trim() == "" ? 0 : Convert.ToInt32(txtSearchEmpId.Text.Trim());
            string name = txtSearchName.Text;
            string phone = txtSearchPhone.Text;
            Int16? gender = Convert.ToInt16( cboSearchGender.SelectedValue);
            Int16? status = Convert.ToInt16(cboSearchStatus.SelectedValue);
            int? branch = Convert.ToInt16(cboSearchBranch.SelectedValue);
            Int16? shift = Convert.ToInt16(cboSearchShift.SelectedValue);


            LoadStudentList(branch, studentId, name, phone, status, gender, shift);
        }

        private void grd_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //txtClassID.Text = grd.Rows[e.RowIndex].Cells[0].Value.ToString();
                //txtName.Text = grd.Rows[e.RowIndex].Cells[1].Value.ToString();
                //btnDelete.Enabled = true;

                int StudentId = Convert.ToInt32(grd.Rows[e.RowIndex].Cells[0].Value);
                LoadStudent(StudentId);
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
                fillSearchList();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ClearListForm()
        {
            dt.Clear();
            txtSearchEmpId.Text = string.Empty;
            txtSearchName.Text = string.Empty;
            txtSearchPhone.Text = string.Empty;
            cboSearchGender.SelectedIndex = -1;
            cboSearchBranch.SelectedIndex = -1;
            cboSearchStatus.SelectedIndex = -1;
            cboSearchShift.SelectedIndex = -1;
            //LoadStudentList(null,null,null,null,null,null,null);
        }
        public void ClearDetailForm()
        {
            txtStudentId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtFather.Text = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone2.Text = string.Empty;
            cboGender.SelectedValue = string.Empty;
            cboStatus.SelectedValue = string.Empty;
            cboShift.SelectedValue = string.Empty;
            dtAdmissionDate.Text = string.Empty;
            dtFeeDate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtAddress.Text = string.Empty;

            removePicture();

        }

       


        private void txtFeeAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void grd_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                int StudentId = Convert.ToInt32(grd.Rows[e.RowIndex].Cells[0].Value);
                LoadStudent(StudentId);
                tabTab.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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
