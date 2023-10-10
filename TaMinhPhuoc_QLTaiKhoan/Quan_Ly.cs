using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TaMinhPhuoc_QLTaiKhoan
{
    public partial class frmQuanLy : Form
    {
        QLTaiKhoanEntities db;
        Account account = new Account();
        Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,6})$");
        string temp;
        public frmQuanLy(string txtTK)
        {
            InitializeComponent();
            temp = txtTK;
        }
        void LoadData()
        {
            db = new QLTaiKhoanEntities();
            var getRole = from s in db.Accounts
                          where s.userName == temp
                          select s.roleID.ToString();
            if (getRole != null)
            {
                int roleID = int.Parse(getRole.FirstOrDefault());
                if (roleID == 1)
                {
                    LoadDataAdmin();
                }
                else
                {
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    LoadDataEmploy();
                }
            }
            txtUsername.Text = "";
            txtOwnername.Text = "";
            txtpassWord.Text = "";
            txtEmail.Text = "";
            cmbRole.SelectedIndex = -1;
        }
        void LoadDataAdmin()
        {
            var result = (from p in db.Accounts
                          join c in db.Roles on p.roleID equals c.roleID
                          select new
                          {
                              p.userName,
                              p.ownerName,
                              p.email,
                              p.passWord,
                              VaiTro = c.roleName
                          }).ToList();
            dvTaiKhoan.DataSource = result;
        }
        void LoadDataEmploy()
        {
            var result = (from p in db.Accounts
                          join c in db.Roles on p.roleID equals c.roleID
                          select new
                          {
                              p.userName,
                              p.ownerName,
                          }).ToList();
            dvTaiKhoan.DataSource = result;
        }
        private void frmQuanLy_Load(object sender, EventArgs e)
        {
            LoadData();
            cmbRole.DataSource = db.Roles.ToList();
            cmbRole.ValueMember = "roleID";
            cmbRole.DisplayMember = "roleName";
            cmbRole.SelectedIndex = -1;
            cmbRole.Invalidate();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var checkusername = db.Accounts.Where( x => x.userName == txtUsername.Text).FirstOrDefault();
            var checkEmail = db.Accounts.Where(x => x.email == txtEmail.Text).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(txtUsername.Text)
                        || string.IsNullOrWhiteSpace(txtOwnername.Text)
                        || string.IsNullOrWhiteSpace(txtEmail.Text)
                        || string.IsNullOrWhiteSpace(txtpassWord.Text)
                        || cmbRole.SelectedIndex == -1)
            {
                MessageBox.Show("Bạn nhập dữ liệu thiếu");
            }
            else if (checkusername != null)
            {
                MessageBox.Show("Tên tài khoản đã tồn tại");
            }
            else if (checkEmail != null)
            {
                MessageBox.Show("Email đã tồn tại");
            }
            else if (!regex.IsMatch(txtEmail.Text))
            {
                MessageBox.Show("Email của bạn không hợp lệ vui lòng đổi email!");
            }
            else
            {
                account.userName = txtUsername.Text;
                account.ownerName = txtOwnername.Text;
                account.email = txtEmail.Text;
                account.passWord = txtpassWord.Text;
                account.roleID = (int)cmbRole.SelectedIndex + 1;
                db.Accounts.Add(account);
                int result = db.SaveChanges();
                if (result > 0)
                {

                    MessageBox.Show("Thêm Thành công");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Thêm Thất bại");
                }
            }
        }

        private void dvTaiKhoan_DoubleClick(object sender, EventArgs e)
        {
            db = new QLTaiKhoanEntities();
            var getRole = from s in db.Accounts
                          where s.userName == temp
                          select s.roleID.ToString();
            if (getRole != null)
            {
                int roleID = int.Parse(getRole.FirstOrDefault());
                if (roleID == 1)
                {
                    int lst = dvTaiKhoan.CurrentRow.Index;
                    txtUsername.Text = dvTaiKhoan.Rows[lst].Cells[0].Value.ToString();
                    txtOwnername.Text = dvTaiKhoan.Rows[lst].Cells[1].Value.ToString();
                    txtEmail.Text = dvTaiKhoan.Rows[lst].Cells[2].Value.ToString();
                    txtpassWord.Text = dvTaiKhoan.Rows[lst].Cells[3].Value.ToString();
                    cmbRole.Text = dvTaiKhoan.Rows[lst].Cells[4].Value.ToString();
                }
                else
                {
                    MessageBox.Show("Bạn không phải là admin");                    
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            var soLuongTK = db.Accounts.ToList();
            var checkusername = db.Accounts.Where(x => x.userName == txtUsername.Text).FirstOrDefault();
            var checkEmail = db.Accounts.Where(x => x.email == txtEmail.Text).FirstOrDefault();
            if (soLuongTK != null)
            {
                var QuerySua = db.Accounts.Where(x => x.userName == txtUsername.Text).ToList().FirstOrDefault();
                if (QuerySua != null)
                {
                    if (string.IsNullOrWhiteSpace(txtUsername.Text)
                        || string.IsNullOrWhiteSpace(txtOwnername.Text)
                        || string.IsNullOrWhiteSpace(txtEmail.Text)
                        || string.IsNullOrWhiteSpace(txtpassWord.Text)
                        || cmbRole.SelectedIndex == -1)
                    {

                        MessageBox.Show("Bạn nhập dữ liệu thiếu hay chưa chọn tài khoản muốn sửa");
                    }
                    else if (!regex.IsMatch(txtEmail.Text))
                    {
                        MessageBox.Show("Email của bạn không hợp lệ vui lòng đổi email!");
                    }
                    else
                    {
                        QuerySua.userName = txtUsername.Text;
                        QuerySua.ownerName = txtOwnername.Text;
                        QuerySua.email = txtEmail.Text;
                        QuerySua.passWord = txtpassWord.Text;
                        QuerySua.roleID = (int)cmbRole.SelectedIndex + 1;
                        int result = db.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show("Sửa Thành công");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Sửa Thất bại");
                            LoadData();
                        }
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var QueryXoa = db.Accounts.Where(x => x.userName == txtUsername.Text).ToList().FirstOrDefault();
            DialogResult result = MessageBox.Show("Bạn thật sự muốn xóa tài khoản này không?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (QueryXoa != null)
            {
                if (result == DialogResult.Yes)
                {
                    db.Accounts.Remove(QueryXoa);
                    int kq = db.SaveChanges();
                    if (kq > 0)
                    {
                        MessageBox.Show("Đã xóa sách thành công");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại");
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có sản phẩm này!!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
