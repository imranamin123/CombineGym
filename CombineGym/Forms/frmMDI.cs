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
    public partial class frmMDI : Form
    {
        public CombineGym.Forms.frmLogin objlogin = null;
        public frmMDI()
        {
            InitializeComponent();
            applySecurity();
        }

        private void applySecurity()
        {
            foreach(System.Windows.Forms.ToolStripItem item in this.menuStrip.Items)
            {
                string name = item.Name;
            }
            
        }

        private void branchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBranch objBranch = new frmBranch();
            objBranch.MdiParent = this;
            objBranch.Show();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployee objEmployee = new frmEmployee();
            objEmployee.MdiParent = this;
            objEmployee.Show();
        }

        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudent objStudent = new frmStudent();
            objStudent.MdiParent = this;
            objStudent.Show();
        }

        private void feeReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmFeeReceipt objFeeReceipt = new frmFeeReceipt();
            objFeeReceipt.MdiParent = this;
            objFeeReceipt.Show();
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStudentAttendance objStudentAttendance = new frmStudentAttendance();
            objStudentAttendance.MdiParent = this;
            objStudentAttendance.Show();
        }

        private void staffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmployeeAttendance objEmployeeAttendance = new frmEmployeeAttendance();
            objEmployeeAttendance.MdiParent = this;
            objEmployeeAttendance.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUser objUser = new frmUser();
            objUser.MdiParent = this;
            objUser.Show();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objlogin.Close();
        }
    }
}
