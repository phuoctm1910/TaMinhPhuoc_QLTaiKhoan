using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaMinhPhuoc_QLTaiKhoan
{
    public partial class frmLogin : Form
    {
        QLTaiKhoanEntities db = new QLTaiKhoanEntities();

        public frmLogin()
        {
            InitializeComponent();
        }
  
         
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK);
            }
            else if (txtPassword.TextLength <= 5)
            {
                MessageBox.Show("Mật khẩu phải điền đủ 6 kí tự", "Lỗi", MessageBoxButtons.OK);
            }
            else
            {
                var result = db.Accounts.Where(x => x.userName == txtUsername.Text && x.passWord == txtPassword.Text).ToList().FirstOrDefault();
                if (result != null)
                {
                    var getRole = from s in db.Accounts
                                  where s.userName == txtUsername.Text && s.passWord == txtPassword.Text
                                  select s.roleID.ToString();
                    int roleID = int.Parse(getRole.FirstOrDefault());
                    if (getRole != null && roleID == 1 || roleID == 2 ) 
                    {
                        MessageBox.Show("Đăng Nhập Thành Công");
                        Hide();
                        frmQuanLy fr = new frmQuanLy(txtUsername.Text);
                        fr.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Chỉ có những quản lý hệ thống mới đăng nhập được!!");
                    }

                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu vui lòng nhập lại", "Lỗi", MessageBoxButtons.OK);
                }
            }            
        }
    }
}
