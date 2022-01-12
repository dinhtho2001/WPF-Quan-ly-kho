using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace HangHoa
{
    public partial class formDangKy : Window
    {
        public formDangKy()
        {
            InitializeComponent();
        }

        DataHangHoaDataContext db = new DataHangHoaDataContext();
        Table<QuanLyTaiKhoan>  quanLyTaiKhoans;
       
        private void btnDangKy_Click(object sender, RoutedEventArgs e)
        {
            bool kt = false;
            if (this.txtUserDangKy.Text.Length != 0 && 
                this.txtPasswordDangKy.Text.Length != 0)
            {
                quanLyTaiKhoans = db.GetTable<QuanLyTaiKhoan>();
                var query = from u in quanLyTaiKhoans
                            where u.TenDangNhap ==
                                    txtUserDangKy.Text
                            select u;
                foreach (var i in query)
                {
                    if (i != null)
                    {
                        kt = true;
                        MessageBox.Show("Tên người dùng đã tồn tại", "Error", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        txtUserDangKy.Focus();
                    }
                }
                if(kt == false)
                {
                    QuanLyTaiKhoan tk = new QuanLyTaiKhoan();
                    tk.TenDangNhap = txtUserDangKy.Text;
                    tk.MatKhau = txtPasswordDangKy.Text;
                    quanLyTaiKhoans.InsertOnSubmit(tk);
                    db.SubmitChanges();
                    MessageBox.Show("Tạo tài khoản thành công", "Thông báo",
                        MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập thông tin", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                if (txtUserDangKy.Text.Length != 0)
                {
                    txtPasswordDangKy.Focus();
                }
                else
                {
                    txtUserDangKy.Focus();
                }
            } 
        }
    }
}
