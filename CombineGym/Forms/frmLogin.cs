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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(!Login())
            {
                MessageBox.Show("Login Failed. Username or Password is incorrect!");
                return;
            }
            
            frmMDI mdi = new frmMDI();
            mdi.objlogin = this;
            mdi.Show();
            this.Hide();
        }

        private bool Login()
        {
            secUser user = new DAL.DALSetup().UserGet(1, txtUsername.Text, txtPassword.Text);

            if(user == null )
            {
                return false;
            }
            else
            {
                Utilities.LoginUser = user;
                return true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
