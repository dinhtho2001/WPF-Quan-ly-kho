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
using System.Windows.Shapes;
using System.Data.Linq;

namespace HangHoa
{
    /// <summary>
    /// Interaction logic for formXuatHang.xaml
    /// </summary>
    public partial class formXuatHang : Window
    {
        public formXuatHang()
        {
            InitializeComponent();
        }
        DataHangHoaDataContext db = new DataHangHoaDataContext();
        Table<KhachHang> khachHangs;
        Table<NhanVien> nhanViens;
        Table<HoaDonXuatHang> hoaDonXuatHangs;
        Table<SanPham> sanPhams;
        private void btnLamMoi_Click(object sender, RoutedEventArgs e)
        {
            txtMaHoaDon.Clear();
            cboTenNhanVien.SelectedItem = null;
            txtDonGia.Clear();
            cboMaSanPham.SelectedItem = null;
            txtSoLuong.Clear();
            txtTenSanPham.Clear();
            cboTenKhachHang.SelectedItem = null;
        }
        public int LaySL()
        {            
            int k;
            sanPhams = db.GetTable<SanPham>();
            var data1 = from u in sanPhams
                        where u.MaSanPham == cboMaSanPham.Text
                        select u.SoLuong;
            foreach (int p in data1)
            {                
                return k = p;                
            }
            return 0; 
        }
        
        private void btnXuat_Click(object sender, RoutedEventArgs e)
        {
            bool kt = false;
            if (this.txtMaHoaDon.Text.Length != 0 && 
                this.txtTenSanPham.Text.Length != 0 &&
                this.txtDonGia.Text.Length != 0 && 
                this.txtSoLuong.Text.Length != 0 && 
                this.cboMaSanPham.SelectedValue != null &&
                this.cboTenKhachHang.SelectedValue != null &&
                this.cboTenNhanVien.SelectedValue != null)
            {
                hoaDonXuatHangs = db.GetTable<HoaDonXuatHang>();
                var u = from c in hoaDonXuatHangs
                        where c.MaHoaDon == txtMaHoaDon.Text
                        select c;
                foreach (var item in u)
                {
                    if (item != null)
                        kt = true;
                }
                if (kt == false)
                {
                    if (LaySL() >= Convert.ToInt32(txtSoLuong.Text))
                    {
                        hoaDonXuatHangs = db.GetTable<HoaDonXuatHang>();
                        HoaDonXuatHang hd = new HoaDonXuatHang();
                        hd.MaHoaDon = txtMaHoaDon.Text;
                        hd.MaNhanVien = cboTenNhanVien.SelectedValue.ToString();
                        hd.MaSanPham = cboMaSanPham.SelectedValue.ToString();
                        hd.TenSanPham = txtTenSanPham.Text;
                        hd.Ngaynhap = Convert.ToDateTime(DateTime.Now.ToString());
                        hd.SoLuong = Convert.ToInt32(txtSoLuong.Text);
                        hd.DonGia = Convert.ToDouble(txtDonGia.Text);
                        hd.MaKhachHang = cboTenKhachHang.SelectedValue.ToString();
                        hd.TongTien = (Convert.ToInt32(txtSoLuong.Text) * 
                                        Convert.ToDouble(txtDonGia.Text));
                        hoaDonXuatHangs.InsertOnSubmit(hd);

                        SanPham sp = new SanPham();
                        sp = db.SanPhams.Where(a => a.MaSanPham == cboMaSanPham.Text).SingleOrDefault();
                        sp.MaSanPham = cboMaSanPham.SelectedValue.ToString();
                        sp.SoLuong = Convert.ToInt32(LaySL()) - Convert.ToInt32(txtSoLuong.Text);                        

                        db.SubmitChanges();                        
                        MessageBox.Show("Xuất thành công", "Thông báo", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Số lượng không đủ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Trùng mã hóa đơn\n Nhập lại mã hóa đơn và thử lại", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }                
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin và thử lại", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_TenKH();
            load_TenNV();
            load_MaSP();
        }
        public void load_TenKH()
        {
            khachHangs = db.GetTable<KhachHang>();
            var query = from a in khachHangs
                        select new { ma = a.MaKhachHang, ten = a.TenKhachHang };
            cboTenKhachHang.ItemsSource = query;
            cboTenKhachHang.DisplayMemberPath = "ten";
            cboTenKhachHang.SelectedValuePath = "ma";
        }
        public void load_TenNV()
        {
            nhanViens = db.GetTable<NhanVien>();
            var query = from a in nhanViens
                        select new { ma = a.MaNhanVien, ten = a.TenNhanVien };
            cboTenNhanVien.ItemsSource = query;
            cboTenNhanVien.DisplayMemberPath = "ten";
            cboTenNhanVien.SelectedValuePath = "ma";
        }
        public void load_MaSP()
        {
            sanPhams = db.GetTable<SanPham>();
            var query = from a in sanPhams
                        select new { ma = a.MaSanPham};
            cboMaSanPham.ItemsSource = query;
            cboMaSanPham.DisplayMemberPath = "ma";
            cboMaSanPham.SelectedValuePath = "ma";
        }
    }
}
