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
using DAL;

namespace CombineGym.Forms
{
    public partial class frmFeeReceipt : Form
    {
        private DataTable dtFeeHistory = new DataTable();
        private DataTable dtFeeHistorySearch = new DataTable();
        public frmFeeReceipt()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearDetailForm();
            EnableControls("load");
            //makeFormEditable(true);
        }

        public void ClearDetailForm()
        {
            txtFeeReceiptId.Text = string.Empty;
            dtReceiptDate.Value = DateTime.Now;
            //cboFeeYear.SelectedIndex = 0;
            //cboFeeMonth.SelectedIndex = 0;
            txtStudentId.Text = string.Empty;
            txtStudentName.Text = string.Empty;
            cboGender.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            cboShift.SelectedIndex = 0;
            dtAdmissionDate.Text = string.Empty;
            txtMonthlyFee.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            txtPrevBalance.Text = string.Empty;
            txtPayableAmount.Text = string.Empty;
            txtReceivedAmount.Text = string.Empty;
            txtRemainingBalance.Text = string.Empty;

            dtFeeHistory.Clear();
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

        private void frmFeeReceipt_Load(object sender, EventArgs e)
        {
            int branchId = 1;
            FillComboBoxes();
            setHistoryGridColumns();
            setHistorySearchGridColumns();
            LoadHistorySearchList(branchId, null, dtFromDate.Value, dtToDate.Value);
            EnableControls("load");
            //LoadEmployee(1);
            //setGridColumns();
            // List
            //LoadEmployeeList(0, string.Empty, 0);
        }

        private void FillComboBoxes()
        {
            DALSetup dal = new DALSetup();
            dal.fillGenderComboBox(ref cboGender);
            dal.fillStatusComboBox(ref cboStatus);
            dal.fillFeeYearComboBox(ref cboFeeYear);
            int year = DateTime.Now.Year;
            cboFeeYear.SelectedIndex = 0;
            dal.fillFeeMonthComboBox(ref cboFeeMonth);
            cboFeeMonth.SelectedIndex = 0;
            //cboFeeMonth.SelectedValue = DateTime.Now.Month;
            dal.fillShiftComboBox(ref cboShift);
        }

        private void txtStudentId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSearchStudent_Click(object sender, EventArgs e)
        {
            int studentId = Convert.ToInt32(txtStudentId.Text);
            ClearDetailForm();
            txtStudentId.Text = Convert.ToString(studentId) ;
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
                    txtMonthlyFee.Text = Convert.ToString(student.FeeAmount);
                    txtPrevBalance.Text = Convert.ToString( new DALFeeReceipt().GetStdentPrevBal(student.BranchId,student.StudentId, Convert.ToInt32(cboFeeMonth.SelectedValue),Convert.ToInt32(cboFeeYear.SelectedValue)));
                    

                    txtPayableAmount.Text = Convert.ToString(Convert.ToDecimal(txtMonthlyFee.Text) + Convert.ToDecimal(txtPrevBalance.Text));
                    txtReceivedAmount.Text = txtPayableAmount.Text;
                    txtRemainingBalance.Text = Convert.ToString(Convert.ToDecimal(txtPayableAmount.Text) - Convert.ToDecimal(txtReceivedAmount.Text));

                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string exeDir = System.IO.Path.GetDirectoryName(exePath);
                    DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                    DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                    string appFolder = appFolderDirInfo.ToString();

                    string imageLocation = student.Picture;
                    pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;
                    EnableControls("load");
                    LoadHistoryList(student.BranchId, student.StudentId);
                }
            }
        }

        private void txtReceivedAmount_Leave(object sender, EventArgs e)
        {
            if (txtReceivedAmount.Text == string.Empty)
            {
                txtReceivedAmount.Text = "0";
            }
            if (Convert.ToDecimal(txtReceivedAmount.Text) < 0)
            {
                MessageBox.Show("Received amount can not be negative");
                txtReceivedAmount.Focus();
                return;
            }
            txtPayableAmount.Text = txtPayableAmount.Text == string.Empty ? "0" : txtPayableAmount.Text;
            txtReceivedAmount.Text = txtReceivedAmount.Text == string.Empty ? "0" : txtReceivedAmount.Text;

            txtRemainingBalance.Text = Convert.ToString(Convert.ToDecimal(txtPayableAmount.Text) - Convert.ToDecimal(txtReceivedAmount.Text));


        }

        private void txtReceivedAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValidateForm() == true)
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void Save()
        {
            FeeReceipt feeReceipt = null;
            DALFeeReceipt dal = new DALFeeReceipt();
            //int std = ;
            int? studentId = Convert.ToInt32(txtStudentId.Text);
            int? branchId = 1;
            int feeYear = Convert.ToInt32(cboFeeYear.SelectedValue);
            int feeMonth = Convert.ToInt32(cboFeeMonth.SelectedValue);
            int feeReceiptId = Convert.ToInt32(txtFeeReceiptId.Text == string.Empty ? "0" : txtFeeReceiptId.Text);

            int dbStatus = dal.IsDBStatus(feeReceiptId, branchId, studentId, feeYear, feeMonth);

            if(dbStatus == 2)
            {
                MessageBox.Show("Fee already exists for this year and month");
                return;
            }
           

            feeReceipt = dal.FeeReceiptGet(feeReceiptId);
            if (feeReceipt == null)
            {
                feeReceipt = new FeeReceipt();
            }

            feeReceipt.BalanceAmount = Convert.ToDecimal(txtRemainingBalance.Text);
            feeReceipt.BranchId = 1;
            feeReceipt.FeeMonth = Convert.ToInt16(cboFeeMonth.SelectedValue);
            feeReceipt.FeeReceiptId = feeReceiptId;
            feeReceipt.FeeYear = Convert.ToInt16(cboFeeYear.SelectedValue);
            if (feeReceiptId > 0)
            {
                feeReceipt.ModifiedBy = 1;
                feeReceipt.ModifiedDate = DateTime.Now;
            }
            else
            {
                feeReceipt.CreatedBy = 1;
                feeReceipt.CreatedDate = DateTime.Now;
            }
            feeReceipt.PreviousBalance = Convert.ToDecimal(txtPrevBalance.Text);
            feeReceipt.ReceiptAmount = Convert.ToDecimal(txtReceivedAmount.Text);
            feeReceipt.ReceiptDate = dtReceiptDate.Value;
            feeReceipt.ReferenceNo = lblRefNo.Text;
            feeReceipt.Remarks = txtRemarks.Text;
            feeReceipt.StudentId = Convert.ToInt32(txtStudentId.Text);

            //if(txtFeeReceiptId.Text == string.Empty)
            //{
            //    EnableControls("save");
            //}


            feeReceipt = dal.FeeReceiptSave(feeReceipt);
            if (feeReceipt != null)
            {
                txtFeeReceiptId.Text = Convert.ToString(feeReceipt.FeeReceiptId);
                EnableControls("save");
            }

            LoadHistoryList(feeReceipt.BranchId, feeReceipt.StudentId);
        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            if (txtStudentId.Text == string.Empty)
            {
                MessageBox.Show("Please enter Student");
                IsValid = false;
                txtStudentId.Focus();
            }
            return IsValid;
        }

        private void setHistoryGridColumns()
        {
            dtFeeHistory.Columns.Add(new DataColumn("Year", typeof(int)));
            dtFeeHistory.Columns.Add(new DataColumn("Month", typeof(string)));
            dtFeeHistory.Columns.Add(new DataColumn("Date", typeof(string)));
            dtFeeHistory.Columns.Add(new DataColumn("Previous", typeof(string)));
            dtFeeHistory.Columns.Add(new DataColumn("Received", typeof(string)));
            dtFeeHistory.Columns.Add(new DataColumn("Balance", typeof(string)));
            dtFeeHistory.Columns.Add(new DataColumn("IsPending", typeof(string)));

            grdFeeHistory.DataSource = dtFeeHistory;

        }

        private void setHistorySearchGridColumns()
        {
            dtFeeHistorySearch.Columns.Add(new DataColumn("ReceiptId", typeof(int)));
            dtFeeHistorySearch.Columns.Add(new DataColumn("Year", typeof(int)));
            dtFeeHistorySearch.Columns.Add(new DataColumn("Month", typeof(string)));
            dtFeeHistorySearch.Columns.Add(new DataColumn("Date", typeof(string)));
            dtFeeHistorySearch.Columns.Add(new DataColumn("Student", typeof(string)));

            grdSearch.DataSource = dtFeeHistorySearch;

        }

        private void LoadHistoryList(int? branchId, int? studentId)
        {
            DALFeeReceipt dal = new DALFeeReceipt();
            //dt = new DataTable();
            dtFeeHistory.Clear();
            List<spFeeReceiptHistory_Result> FeeHistoryResults = dal.FeeReceiptHistoryResult(branchId, studentId);
            //grd.DataSource = StudentListResult;
            if (FeeHistoryResults != null && FeeHistoryResults.Count > 0)
            {
                foreach (spFeeReceiptHistory_Result feeHistoryResult in FeeHistoryResults)
                {
                    var row = dtFeeHistory.NewRow();
                    row["Year"] = feeHistoryResult.FeeYear;
                    row["Month"] = feeHistoryResult.FeeMonth;
                    row["Date"] =  feeHistoryResult.ReceiptDate.Value.ToString("dd/MM/yyyy");
                    row["Previous"] = feeHistoryResult.PreviousBalance;
                    row["Received"] = feeHistoryResult.ReceiptAmount;
                    row["Balance"] = feeHistoryResult.BalanceAmount;
                    row["IsPending"] = feeHistoryResult.IsPending;
                    // row["ShiftTiming"] = StudentResult.shi;
                    dtFeeHistory.Rows.Add(row);
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int branchId = 1;
                int studentSearchId = Convert.ToInt32(txtSearchStudentId.Text == string.Empty ? "0" : txtSearchStudentId.Text);
                LoadHistorySearchList(branchId, studentSearchId, dtFromDate.Value,dtToDate.Value);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadHistorySearchList(int? branchId, int? studentId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            DALFeeReceipt dal = new DALFeeReceipt();
            //dt = new DataTable();
            dtFeeHistorySearch.Clear();
            List<spFeeReceiptHistorySearch_Result> FeeReceiptHistorySearchResults = dal.FeeReceiptHistorySearchResult(branchId, studentId, fromDate, toDate);
            //grd.DataSource = StudentListResult;
            if (FeeReceiptHistorySearchResults != null && FeeReceiptHistorySearchResults.Count > 0)
            {
                foreach (spFeeReceiptHistorySearch_Result feeHistorySearchResult in FeeReceiptHistorySearchResults)
                {
                    var row = dtFeeHistorySearch.NewRow();
                    row["ReceiptId"] = feeHistorySearchResult.FeeReceiptId;
                    row["Year"] = feeHistorySearchResult.FeeYear;
                    row["Month"] = feeHistorySearchResult.FeeMonth;
                    row["Date"] = feeHistorySearchResult.ReceiptDate.Value.ToString("dd/MM/yyyy");
                    row["Student"] = feeHistorySearchResult.Name;

                    dtFeeHistorySearch.Rows.Add(row);
                }
            }

        }

        private void LoadFeeReceipt(int feeReceiptId)
        {
            DALFeeReceipt dal = new DALFeeReceipt();
            FeeReceipt feeReceipt =  dal.FeeReceiptGet(feeReceiptId);
            if (feeReceipt != null)
            {
                txtFeeReceiptId.Text = Convert.ToString(feeReceipt.FeeReceiptId);
                txtStudentId.Text = Convert.ToString(feeReceipt.StudentId);
                cboFeeYear.SelectedValue = feeReceipt.FeeYear;
                cboFeeMonth.SelectedValue = feeReceipt.FeeMonth;
                txtPrevBalance.Text = Convert.ToString(feeReceipt.PreviousBalance);
                
                txtReceivedAmount.Text = Convert.ToString(feeReceipt.ReceiptAmount);
                txtRemainingBalance.Text = Convert.ToString(feeReceipt.BalanceAmount);
                txtRemarks.Text = feeReceipt.Remarks;



                Student student = new DALSetup().StudentGet(feeReceipt.StudentId.Value);
                if(student != null)
                {
                    txtStudentName.Text = student.Name;
                    cboGender.SelectedValue = student.GenderId;
                    cboStatus.SelectedValue = student.StatusId;
                    dtAdmissionDate.Value = student.AdmissionDate.Value;
                    cboShift.SelectedValue = student.GymShiftId;
                    txtMonthlyFee.Text = Convert.ToString(student.FeeAmount);
                    txtPayableAmount.Text = Convert.ToString(feeReceipt.PreviousBalance + student.FeeAmount);
                    string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string exeDir = System.IO.Path.GetDirectoryName(exePath);
                    DirectoryInfo binDir = System.IO.Directory.GetParent(exeDir);
                    DirectoryInfo appFolderDirInfo = System.IO.Directory.GetParent(binDir.ToString());
                    string appFolder = appFolderDirInfo.ToString();

                    string imageLocation = student.Picture;
                    pictureBox1.ImageLocation = appFolder + "\\images\\" + imageLocation;

                    LoadHistoryList(student.BranchId, student.StudentId); 
                }

                EditableByFeeReceipt(student.StudentId, feeReceiptId);
            }
        }

        private void EditableByFeeReceipt(int studentId, int feeReceiptId)
        {
            DALFeeReceipt dal = new DALFeeReceipt();
            bool isEditable = dal.IsFeeReceiptEditable(studentId,feeReceiptId);
            if(isEditable == false)
            {
                EnableControls("uneditable");
            }
            else
            {
                EnableControls("editable");
            }
            //makeFormEditable(isEditable);
        }

        private void EnableControls(string state )
        {
            if(state == "load")
            {
                dtReceiptDate.Enabled = true;
                txtStudentId.Enabled = true;
                btnSearchStudent.Enabled = true;
                txtRefNo.Enabled = true;
                cboFeeYear.Enabled = true;
                cboFeeMonth.Enabled = true;
                txtReceivedAmount.Enabled = true;
                txtRemarks.Enabled = true;
                btnDelete.Enabled = false;
                btnClose.Enabled = true;
                btnSave.Enabled = true;
                dtFeeHistory.Clear();
            }
            else if (state == "editable")
            {
                dtReceiptDate.Enabled = true;
                txtStudentId.Enabled = false;
                btnSearchStudent.Enabled = false;
                txtRefNo.Enabled = true;
                cboFeeYear.Enabled = false;
                cboFeeMonth.Enabled = false;
                txtReceivedAmount.Enabled = true;
                txtRemarks.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                //dtFeeHistory.Clear();
            }
            else if (state == "uneditable")
            {
                dtReceiptDate.Enabled = false;
                txtStudentId.Enabled = false;
                btnSearchStudent.Enabled = false;
                txtRefNo.Enabled = false;
                cboFeeYear.Enabled = false;
                cboFeeMonth.Enabled = false;
                txtReceivedAmount.Enabled = false;
                txtRemarks.Enabled = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
                //dtFeeHistory.Clear();
            }

            else if (state == "save")
            {
                dtReceiptDate.Enabled = false;
                txtStudentId.Enabled = false;
                btnSearchStudent.Enabled = false;
                txtRefNo.Enabled = false;
                cboFeeYear.Enabled = false;
                cboFeeMonth.Enabled = false;
                txtReceivedAmount.Enabled = true;
                txtRemarks.Enabled = true;
                btnDelete.Enabled = true;
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                btnClose.Enabled = true;

            }
            else if(state == "delete")
            {

            }
            else if(state == "clear")
            {

            }
            else if(state == "loadEdit")
            {

            }
            else if(state == "loadUnEdit")
            {

            }
        }

        private void grdSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int feeReceiptId = Convert.ToInt32(grdSearch.Rows[e.RowIndex].Cells[0].Value);
                LoadFeeReceipt(feeReceiptId);
                tabTab.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtFeeReceiptId.Text != string.Empty)
                {

                    DialogResult dr = MessageBox.Show("Are you sure you want to delete this record ?", "Delete Confirmation", MessageBoxButtons.YesNo);

                    if(dr == DialogResult.Yes)
                    {
                        int feeReceiptId = Convert.ToInt32(txtFeeReceiptId.Text);
                        new DALFeeReceipt().FeeReceiptDelete(feeReceiptId);
                        ClearDetailForm();
                        EnableControls("load");
                    }

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        private void delete()
        {

        }

       
    }
}
