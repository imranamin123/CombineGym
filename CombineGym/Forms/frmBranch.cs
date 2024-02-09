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

namespace CombineGym.Forms
{
    public partial class frmBranch : Form
    {
        public frmBranch()
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
                    MessageBox.Show("Record saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmBranch_Load(object sender, EventArgs e)
        {
            try
            {
                LoadBranch(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Save()
        {
            Branch branch = new Branch();
            DALSetup dal = new DALSetup();

            branch.BranchId = txtBranchId.Text == string.Empty ? 0 : Convert.ToInt32(txtBranchId.Text);
            branch.BranchName = txtBranchName.Text;
            branch.Phone1 = txtPhone1.Text;
            branch.Phone2 = txtPhone2.Text;
            branch.Address = txtAddress.Text;
            branch = dal.BranchSave(branch);
            if (branch != null)
            {
                txtBranchId.Text = Convert.ToString(branch.BranchId);
            }

        }

        private bool ValidateForm()
        {
            bool IsValid = true;
            if (txtBranchName.Text == string.Empty)
            {
                MessageBox.Show("Please enter branch name");
                IsValid = false;
                txtBranchName.Focus();
            }
            else if (txtPhone1.Text == string.Empty)
            {
                MessageBox.Show("Please enter at least one branc phone");
                IsValid = false;
                txtPhone1.Focus();
            }
            else if (txtAddress.Text == string.Empty)
            {
                MessageBox.Show("Please enter branch address");
                IsValid = false;
                txtBranchName.Focus();
            }

            return IsValid;
        }

        private void LoadBranch(int id)
        {
            DALSetup dal = new DALSetup();
            Branch branch = dal.BranchGet(id);
            if (branch != null)
            {
                txtBranchId.Text = Convert.ToString( branch.BranchId);
                txtBranchName.Text = branch.BranchName;
                txtPhone1.Text = branch.Phone1;
                txtPhone2.Text = branch.Phone2;
                txtAddress.Text = branch.Address;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
    }
}
