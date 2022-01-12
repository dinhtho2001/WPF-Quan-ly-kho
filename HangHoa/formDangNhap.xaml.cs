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
using System.Data.SqlClient;
using System.Data.Linq;

namespace HangHoa
{
    /// <summary>
    /// Interaction logic for formDangNhap.xaml
    /// </summary>
    public partial class formDangNhap : Window
    {
        public formDangNhap()
        {
            InitializeComponent();
        }        
        DataHangHoaDataContext db = new DataHangHoaDataContext();
        Table<QuanLyTaiKhoan> quanLyTaiKhoans;      
        private void btnThoatDangNhap_Click(object sender, RoutedEventArgs e) // Thoát ĐĂNG NHẬP
        {
            this.Close();
        }
        private void btnDangKy_Click(object sender, RoutedEventArgs e) // cLICK ĐĂNG KÝ
        {            
            formDangKy a = new formDangKy();
            a.Show();            
        }     
        private void btnDangNhap_Click(object sender, RoutedEventArgs e) // ĐĂNG NHẬP
        {
            bool kiemtra = false;
            if (this.txtUsername.Text.Length != 0 &&
                this.txtPassword.Password.Length != 0)
            {
                quanLyTaiKhoans = db.GetTable<QuanLyTaiKhoan>();
                var query = from a in quanLyTaiKhoans
                            where a.TenDangNhap == txtUsername.Text &&
                                  a.MatKhau == txtPassword.Password
                            select a;
                foreach (var dn in query)
                {
                    if (dn != null)
                    {
                        kiemtra = true;
                    }
                }
                if (kiemtra == true)
                {
                    formQuanLy ql = new formQuanLy();
                    ql.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng", "Thông báo", 
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    txtUsername.Focus();
                }
            }
            else
            {
                if (this.txtUsername.Text.Length == 0 &&
                    this.txtPassword.Password.Length == 0)
                {
                    MessageBox.Show("Hãy nhập tài khoản và mật khẩu", "Thông báo",
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    txtUsername.Focus();
                }
                else if (this.txtUsername.Text.Length == 0 ||
                        this.txtPassword.Password.Length == 0)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng", "Thông báo", 
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                    if(txtUsername.Text.Length != 0)
                    {
                        txtPassword.Focus();
                    }
                    else
                        txtUsername.Focus();
                }                
            }
        }
        //
        // SHOW PASSWORD
        //
        private void ShowPassword_PreviewMouseDown(object sender, MouseButtonEventArgs e) => ShowPassword();
        private void ShowPassword_PreviewMouseUp(object sender, MouseButtonEventArgs e) => ShowPassword();
        private void ShowPassword_MouseLeave(object sender, MouseEventArgs e) => AnPassword();
        private void ShowPassword()
        {
            txtShowPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            txtShowPassword.Text = txtPassword.Password;
        }
        private void AnPassword()
        {
            txtShowPassword.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
        }
    }
}
