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
using System.Data;
using System.Windows.Navigation;

namespace HangHoa
{
    public partial class formThem : Window
    {
        public formThem()
        {
            InitializeComponent();
        }
        //
        //    FORM THÊM
        //
        DataHangHoaDataContext db = new DataHangHoaDataContext();
        Table<SanPham> sanPhams;
        Table<NhaPhanPhoi> nhaPhanPhois;
        Table<HoaDonNhapHang> hoaDonNhapHangs;
        Table<NhanVien> nhanViens;
        private void them_Loaded(object sender, RoutedEventArgs e)
        {
            load_CongTyCombo();
            load_NhanNVienCombo();
        }
        
        public void load_CongTyCombo() //Load dữ liệu lên combobox
        {
            nhaPhanPhois = db.GetTable<NhaPhanPhoi>();
            var query1 = from ct in nhaPhanPhois
                         select new { ma = ct.MaCongTy,
                                    tenct = ct.TenCongTy};
            cboTenCongTy.ItemsSource = query1;
            cboTenCongTy.DisplayMemberPath = "tenct";
            cboTenCongTy.SelectedValuePath = "ma";
        }
        public void load_NhanNVienCombo() //Load Nhân viên lên combobox
        {
            nhanViens = db.GetTable<NhanVien>();
            var query2 = from nv in nhanViens
                         select new { ma = nv.MaNhanVien,
                                     ten = nv.TenNhanVien };
            cboTenNhanVien.ItemsSource = query2;
            cboTenNhanVien.DisplayMemberPath = "ten";
            cboTenNhanVien.SelectedValuePath = "ma";
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            bool kt = false;            
            if (this.txtMaSanPham.Text.Length != 0 && this.txtTenSanPham.Text.Length != 0
                && this.txtSoLuong.Text.Length != 0
                && this.txtDonGia.Text.Length != 0 && this.cboTenNhanVien.SelectedItem != null
                && this.cboTenCongTy.SelectedItem != null)
            {
                sanPhams = db.GetTable<SanPham>();
                hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();
                SanPham sp = new SanPham();
                HoaDonNhapHang hd = new HoaDonNhapHang();
                var query = from a in sanPhams
                            where a.MaSanPham == txtMaSanPham.Text
                            select a;
                foreach (var item in query)
                {
                    if (item != null)
                        kt = true;
                    MessageBox.Show("Trùng mã sản phẩm", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (kt == false)
                {                    
                    sp.MaSanPham = txtMaSanPham.Text;
                    sp.TenSanPham = txtTenSanPham.Text;
                    sp.SoLuong = Convert.ToInt32(txtSoLuong.Text);
                    sp.DonGia = Convert.ToDouble(txtDonGia.Text);
                    sp.NgayNhap = Convert.ToDateTime(DateTime.Now.ToString());
                    sp.MaCongTy = cboTenCongTy.SelectedValue.ToString();
                    sanPhams.InsertOnSubmit(sp);

                    hd.MaSanPham = txtMaSanPham.Text;
                    hd.TenSanPham = txtTenSanPham.Text;
                    hd.SoLuong = Convert.ToInt32(txtSoLuong.Text);
                    hd.DonGia = Convert.ToDouble(txtDonGia.Text);
                    hd.Ngaynhap = Convert.ToDateTime(DateTime.Now.ToString());
                    hd.MaHoaDon = txtMaSanPham.Text + Convert.ToString (txtSoLuong.Text);
                    hd.MaCongTy = cboTenCongTy.SelectedValue.ToString();
                    hd.MaNhanVien = cboTenNhanVien.SelectedValue.ToString();
                    hd.ThanhTien = Convert.ToInt32(txtSoLuong.Text) * Convert.ToDouble(txtDonGia.Text);
                    hoaDonNhapHangs.InsertOnSubmit(hd);

                    db.SubmitChanges();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                }               
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin và thử lại", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnLamMoi_Click(object sender, RoutedEventArgs e) // LÀM MỚI
        {
            txtMaSanPham.Clear();
            txtTenSanPham.Clear();
            txtSoLuong.Clear();
            txtDonGia.Clear();             
            cboTenCongTy.SelectedItem = null;
            cboTenNhanVien.SelectedItem = null;
        }

    }
}
