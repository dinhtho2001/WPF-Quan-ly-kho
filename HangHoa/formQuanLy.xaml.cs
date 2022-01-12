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
using System.Windows.Navigation;
using System.Data.Linq;

namespace HangHoa
{
    public partial class formQuanLy : Window 
    {
        public formQuanLy()
        {
            InitializeComponent();
        }        
        DataHangHoaDataContext db = new DataHangHoaDataContext();
        Table<HoaDonNhapHang> hoaDonNhapHangs;
        Table<HoaDonXuatHang> hoaDonXuatHangs;
        Table<QuanLyTaiKhoan> quanLyTaiKhoans;
        Table<NhaPhanPhoi> nhaPhanPhois;
        Table<KhachHang> khachHangs;
        Table<NhanVien> nhanViens;
        Table<SanPham> sanPhams;
        //
        //                      Load data
        //
        private void Sanpham_Loaded(object sender, RoutedEventArgs e) //windows load
        {
            loadData();
            loadSP();
            loadDataHoaDonNhap();
            loadDataHoaDonXuat();
            load_NhaPhanPhoiCombo();
            load_NhaPhanPhoiData();
            load_DataNhanVien();
            load_DataKhachHang();
            load_DataThongKeNhap();
            load_DataThongKeXuat();
        }               
        public void loadData() // load Sản Phẩm
        {           
            sanPhams = db.GetTable<SanPham>();
            var data = from u in sanPhams
                       select u;
            Data.ItemsSource = data;
        }
        public void loadDataHoaDonNhap() // Load hóa đơn nhập 
        {   
            hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();
            nhanViens = db.GetTable<NhanVien>();
            var data2 = from a in hoaDonNhapHangs
                        from b in nhanViens
                        where a.MaNhanVien == b.MaNhanVien
                        select new {MaHoaDon = a.MaHoaDon, MaNhanVien =a.MaNhanVien, TenNhanVien = b.TenNhanVien,
                                    MaSanPham = a.MaSanPham, TenSanPham = a.TenSanPham, SoLuong = a.SoLuong,
                                    DonGia = a.DonGia, MaCongTy = a.MaCongTy, NgayNhap = a.Ngaynhap, ThanhTien = a.ThanhTien};
            DataHoaDonNhap.ItemsSource = data2;            
        }
        public void loadDataHoaDonXuat() // Load hóa đơn xuất 
        {
            hoaDonXuatHangs = db.GetTable<HoaDonXuatHang>();
            var data3 = from b in hoaDonXuatHangs
                        join c in nhanViens
                        on b.MaNhanVien equals c.MaNhanVien
                        select new
                        {
                            MaHoaDon    = b.MaHoaDon,
                            MaNhanVien  = b.MaNhanVien,
                            TenNhanVien = c.TenNhanVien,
                            MaSanPham   = b.MaSanPham,
                            TenSanPham  = b.TenSanPham,
                            NgayNhap    = b.Ngaynhap,
                            SoLuong     = b.SoLuong,
                            DonGia      = b.DonGia,
                            MaKhachHang = b.MaKhachHang,
                            ThanhTien   = b.TongTien
                        };
            DataHoaDonXuat.ItemsSource = data3;                       
        }
        public void load_DataNhanVien() // Load Nhân viên 
        {
            nhanViens = db.GetTable<NhanVien>();
            var data4 = from nv in nhanViens
                        select new { MaNhanVien = nv.MaNhanVien, TenNhanVien = nv.TenNhanVien, TenDangNhap = nv.TenDangNhap };
            DataNhanVien.ItemsSource = data4;

            cboTenNVXuat_TimKiem.ItemsSource = data4; // load combobox HD Xuất
            cboTenNVXuat_TimKiem.DisplayMemberPath = "TenNhanVien";

            cboTenNVNhap_Timkiem.ItemsSource = data4; // load combobox HD Nhập
            cboTenNVNhap_Timkiem.DisplayMemberPath = "TenNhanVien";
        }
        public void load_DataKhachHang() // Load Khách hàng 
        {
            khachHangs = db.GetTable<KhachHang>();
            var query = from a in khachHangs
                        select a;
            DataKhachHang.ItemsSource = query;
        }
        public void load_DataThongKeNhap()
        {
            hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();            
            var data = from a in hoaDonNhapHangs
                       select new
                        {
                            MaSanPham = a.MaSanPham,
                            TenSanPham = a.TenSanPham,
                            SoLuong = a.SoLuong,
                            DonGia = a.DonGia,
                            NgayNhap = a.Ngaynhap,
                            ThanhTien = a.ThanhTien
                        };
            Data_ThongKeHangNhap.ItemsSource = data;
        } // LOAD THỐNG KÊ NHẬP HÀNG
        public void load_DataThongKeXuat()
        {
            hoaDonXuatHangs = db.GetTable<HoaDonXuatHang>();
            var data = from a in hoaDonXuatHangs
                       select new
                       {
                           MaSanPham = a.MaSanPham,
                           TenSanPham = a.TenSanPham,
                           SoLuong = a.SoLuong,
                           DonGia = a.DonGia,
                           NgayNhap = a.Ngaynhap,
                           ThanhTien = a.TongTien
                       };
            Data_ThongKeHangXuat.ItemsSource = data;
        } // LOAD THỐNG KÊ Xuất HÀNG
        public void loadSP() // load Tên Phẩm lên combobox
        {
            sanPhams = db.GetTable<SanPham>();
            var data = from u in sanPhams
                       select new { tenSP = u.TenSanPham };
            cboTenSanPhamTimkiem.ItemsSource = data;
            cboTenSanPhamTimkiem.DisplayMemberPath = "tenSP";
        }
        public void load_NhaPhanPhoiCombo() //Load nhà phân phối lên combobox
        {
            nhaPhanPhois = db.GetTable<NhaPhanPhoi>();
            var data1 = from ct in nhaPhanPhois
                        select new { maCT = ct.MaCongTy, tenCT = ct.TenCongTy };
            cboMaCongTy.ItemsSource = data1;
            cboMaCongTy.DisplayMemberPath = "maCT";
            cboMaCongTy.SelectedValuePath = "maCT";
            cboMaCongTyTimkiem.ItemsSource = data1;
            cboMaCongTyTimkiem.DisplayMemberPath = "maCT";
            cboMaCongTyTimkiem.SelectedValuePath = "maCT";
        }
        public void load_NhaPhanPhoiData() // Load nhà phân phối lên DataGrid
        {
            nhaPhanPhois = db.GetTable<NhaPhanPhoi>();
            var data2 = from ct in nhaPhanPhois
                        select ct;
            DataDoiTac.ItemsSource = data2;
        }
        //
        //                 Sản phẩm
        //
        private void btnTim_Click(object sender, RoutedEventArgs e)// TÌM KIẾM
        {
            sanPhams = db.GetTable<SanPham>();
            bool kt = false;

            if (cboTenSanPhamTimkiem.SelectedItem != null && cboMaCongTyTimkiem.SelectedItem == null && dateNgaynhapTimkiem.SelectedDate == null) // Tên SẢN PHẨM 
            {
                var query = from a in sanPhams
                             where a.TenSanPham == cboTenSanPhamTimkiem.Text
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;

                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            else if (cboMaCongTyTimkiem.SelectedItem != null && cboTenSanPhamTimkiem.SelectedItem == null && dateNgaynhapTimkiem.SelectedDate == null) // mã Cong Ty
            {
                var query = from a in sanPhams
                             where a.MaCongTy == cboMaCongTyTimkiem.Text
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            else if (dateNgaynhapTimkiem.SelectedDate != null && cboMaCongTyTimkiem.Text.Length == 0 && cboTenSanPhamTimkiem.SelectedItem == null) // Date
            {
                var query = from a in sanPhams
                             where a.NgayNhap == Convert.ToDateTime(dateNgaynhapTimkiem.Text)
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
            else if (cboTenSanPhamTimkiem.SelectedItem != null && cboMaCongTyTimkiem.SelectedItem != null && dateNgaynhapTimkiem.SelectedDate == null) // Tên SP + MaCongTy
            {
                var query = from a in sanPhams
                             where a.TenSanPham == cboTenSanPhamTimkiem.Text && a.MaCongTy == cboMaCongTyTimkiem.Text
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                {
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
            }
            else if (cboTenSanPhamTimkiem.SelectedItem != null && dateNgaynhapTimkiem.SelectedDate != null && cboMaCongTyTimkiem.SelectedItem == null) // Tên Sp + Ngay Nhập
            {
                var query = from a in sanPhams
                             where a.TenSanPham == cboTenSanPhamTimkiem.Text && a.NgayNhap == Convert.ToDateTime(dateNgaynhapTimkiem.Text)
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                {
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
            }
            else if (dateNgaynhapTimkiem.SelectedDate != null && cboMaCongTyTimkiem.SelectedItem != null && cboTenSanPhamTimkiem.SelectedItem == null) // Date + MaCongTy
            {
                var query = from a in sanPhams
                                where a.NgayNhap == Convert.ToDateTime(dateNgaynhapTimkiem.Text) && a.MaCongTy == cboMaCongTyTimkiem.Text
                                select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                {
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
            }
            else if (dateNgaynhapTimkiem.SelectedDate != null && cboMaCongTyTimkiem.SelectedItem != null && cboTenSanPhamTimkiem.SelectedItem != null) //Tên SP + MaCongTy + Date 
            {
                var query = from a in sanPhams
                             where a.NgayNhap == Convert.ToDateTime(dateNgaynhapTimkiem.Text) && a.MaCongTy == cboMaCongTyTimkiem.Text && a.TenSanPham == cboTenSanPhamTimkiem.Text
                             select a;
                foreach (var i in query)
                {
                    if (i != null)
                        kt = true;
                }
                if (kt == true)
                    Data.ItemsSource = query;
                else
                {
                    MessageBox.Show("Không có sản phẩm", "Thông Báo", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Nhập đúng thông tin và thử lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }                                 
        }
        private void btnSua_Click_1(object sender, RoutedEventArgs e) //sửa
        {
            if (txtMasanpham.Text != "")
            {
                if (MessageBox.Show("Xác nhận sửa", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    SanPham sp = new SanPham();
                    sp = db.SanPhams.Where(a => a.MaSanPham == txtMaSP_Sữa.Text).SingleOrDefault();  
                    if(sp.MaSanPham != txtMasanpham.Text)
                    {
                        MessageBox.Show("Không thể thay đổi mã sản phẩm\n Hãy thêm lại sản phẩm và xóa sản phẩm cũ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        sp.TenSanPham = txtTensanpham.Text;
                        sp.SoLuong = Convert.ToInt32(txtSoLuong.Text);
                        sp.DonGia = Convert.ToDouble(txtDonGia.Text);
                        sp.NgayNhap = Convert.ToDateTime(dateNgaynhap.Text);
                        sp.MaCongTy = cboMaCongTy.SelectedValue.ToString();
                        db.SubmitChanges();
                        loadData();
                        MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK);
                    }                    
                }
            }else
            {
                MessageBox.Show("Chọn và thử lại", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
        private void btnLammoi_Click(object sender, RoutedEventArgs e)//Làm mới
        {
            txtMasanpham.Clear();
            txtTensanpham.Clear();
            cboMaCongTy.SelectedItem = null;
            txtDonGia.Clear();
            txtSoLuong.Clear();
            dateNgaynhap.SelectedDate = null;
            cboTenSanPhamTimkiem.SelectedItem = null;
            cboMaCongTyTimkiem.SelectedItem = null;
            dateNgaynhapTimkiem.SelectedDate = null;
            Data.SelectedValue = null;
            loadData();
            loadSP();
        }    
        private void btnXoa_Click(object sender, RoutedEventArgs e) // XÓA
        {
            if(txtMasanpham.Text.Length != 0)
            {
                if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    sanPhams = db.GetTable<SanPham>();
                    var sp = db.SanPhams.Single(a => a.MaSanPham == txtMasanpham.Text);
                    db.SanPhams.DeleteOnSubmit(sp);
                    db.SubmitChanges();
                    loadData();
                    MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);
                }
            }
            else
            {
                MessageBox.Show("Chọn sản phẩm cần xóa", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void item_NhapHang_Click(object sender, RoutedEventArgs e) // NHẬP HÀNG
        {
            formThem th = new formThem();
            th.Show();
        }
        private void item_XuatHang_Click(object sender, RoutedEventArgs e)// Xuất HÀNG
        {
            formXuatHang xh = new formXuatHang();
            xh.Show();
        }
        private void menuItem_DangXuat_Click(object sender, RoutedEventArgs e) // ĐĂNG XUẤT
        {            
            formDangNhap dn = new formDangNhap();
            dn.Show();
            this.Close();
        }
        //
        //                    HÓA ĐƠN
        //
        private void btnLoadNhap_Click(object sender, RoutedEventArgs e)// LÀM MỚI NHẬP HÀNG
        {
            cboTenNVNhap_Timkiem.SelectedItem = null;
            date_tim_NhapHang.SelectedDate = null;
            loadDataHoaDonNhap();
        }
        private void btnTim_HoaDonNhap_Click(object sender, RoutedEventArgs e)// Tìm HD NHẬP
        {
            hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();
            nhanViens = db.GetTable<NhanVien>();
            bool kt = false;
            if (cboTenNVNhap_Timkiem.SelectedItem != null && date_tim_NhapHang.SelectedDate == null)// NV
            {
                var query1 = from b in hoaDonNhapHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien
                             where c.TenNhanVien == cboTenNVNhap_Timkiem.Text
                             select new
                             {
                                 MaHoaDon = b.MaHoaDon,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaCongTy = b.MaCongTy,
                                 ThanhTien = b.ThanhTien
                             };
                foreach (var i in query1)
                    if (i != null)
                    {
                        kt = true;
                    }                        
                if (kt == true)
                {
                    DataHoaDonNhap.ItemsSource = query1;
                    kt = false;
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!","Thông báo", MessageBoxButton.OK);
                    loadDataHoaDonNhap();
                }    
            }
            else if (cboTenNVNhap_Timkiem.SelectedItem == null && date_tim_NhapHang.SelectedDate != null)//Date
            {
                var query2 = from b in hoaDonNhapHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien
                             where b.Ngaynhap == date_tim_NhapHang.SelectedDate
                             select new
                             {
                                 MaHoaDon = b.MaHoaDon,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaCongTy = b.MaCongTy,
                                 ThanhTien = b.ThanhTien
                             };
                foreach (var i2 in query2)
                    if (i2 != null)
                        kt = true;
                if (kt == true)
                {
                    DataHoaDonNhap.ItemsSource = query2;
                    kt = false;
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);
                    loadDataHoaDonXuat();
                }
            }
            else if (cboTenNVNhap_Timkiem.SelectedItem != null && date_tim_NhapHang.SelectedDate != null)//NV +Date
            {
                var query3 = from b in hoaDonNhapHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien
                             where c.TenNhanVien == cboTenNVNhap_Timkiem.Text && b.Ngaynhap == date_tim_NhapHang.SelectedDate
                             select new
                             {
                                 MaHoaDon = b.MaHoaDon,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaCongTy = b.MaCongTy,
                                 ThanhTien = b.ThanhTien
                             };
                foreach (var i in query3)
                    if (i != null)
                        kt = true;
                if (kt == true)
                {
                    DataHoaDonNhap.ItemsSource = query3;
                    kt = false;
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);
                    loadDataHoaDonXuat();
                }
            }
            else
                MessageBox.Show("Chọn thông tin cần tìm", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
        private void btnXoa_HoaDonNhap_Click(object sender, RoutedEventArgs e)// XÓA HD NHẬP
        {
            if(txtmaHD_Nhap.Text.Length == 0)
            {
                MessageBox.Show("Chọn thông tin cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();
                var hd = db.HoaDonNhapHangs.Single(a => a.MaHoaDon == txtmaHD_Nhap.Text);
                db.HoaDonNhapHangs.DeleteOnSubmit(hd);
                db.SubmitChanges();                
                loadDataHoaDonNhap();
                MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void btnLoadXuat_Click(object sender, RoutedEventArgs e) // LÀM MỚI XUẤT HÀNG
        {
            cboTenNVXuat_TimKiem.SelectedItem = null;
            date_tim_Xuathang.SelectedDate = null;
            loadDataHoaDonXuat();
        }
        private void btnTim_HoaDonXuat_Click(object sender, RoutedEventArgs e)// TÌM HD XUẤT
        {
            hoaDonXuatHangs = db.GetTable <HoaDonXuatHang>();
            nhanViens = db.GetTable<NhanVien>();
            bool kt = false;
            if (cboTenNVXuat_TimKiem.SelectedItem != null && date_tim_Xuathang.SelectedDate == null)// NV
            {
                var query1 = from b in hoaDonXuatHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien                             
                             where c.TenNhanVien == cboTenNVXuat_TimKiem.Text
                             select new {
                                 MaHoaDon = b.MaHoaDon,
                                 MaNhanVien = b.MaNhanVien,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaKhachHang = b.MaKhachHang,
                                 ThanhTien = b.TongTien
                             };
                foreach (var i in query1)
                if (i != null)
                        kt = true;                
                if (kt == true)
                {
                    DataHoaDonXuat.ItemsSource = query1;
                    kt = false;
                }    
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);
                    loadDataHoaDonXuat();
                }                    
            }
            else if (cboTenNVXuat_TimKiem.SelectedItem == null && date_tim_Xuathang.SelectedDate != null)// DATE
            {
                var query2 = from b in hoaDonXuatHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien
                             where b.Ngaynhap == date_tim_Xuathang.SelectedDate
                             select new
                             {
                                 MaHoaDon = b.MaHoaDon,
                                 MaNhanVien = b.MaNhanVien,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaKhachHang = b.MaKhachHang,
                                 ThanhTien = b.TongTien
                             };
                foreach (var i2 in query2)
                    if (i2 != null)
                        kt = true;
                if (kt == true)
                {
                    DataHoaDonXuat.ItemsSource = query2;
                    kt = false;
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);
                    loadDataHoaDonXuat();
                }   
            }
            else if(cboTenNVXuat_TimKiem.SelectedItem != null && date_tim_Xuathang.SelectedDate != null) // NV +Date
            {
                var query3 = from b in hoaDonXuatHangs
                             join c in nhanViens
                             on b.MaNhanVien equals c.MaNhanVien
                             where c.TenNhanVien == cboTenNVXuat_TimKiem.Text && b.Ngaynhap == date_tim_Xuathang.SelectedDate
                             select new
                             {
                                 MaHoaDon = b.MaHoaDon,
                                 MaNhanVien = b.MaNhanVien,
                                 TenNhanVien = c.TenNhanVien,
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 MaKhachHang = b.MaKhachHang,
                                 ThanhTien = b.SoLuong * b.DonGia
                             };
                foreach (var i in query3)
                    if (i != null)
                        kt = true;
                if (kt == true)
                {
                    DataHoaDonXuat.ItemsSource = query3;
                    kt = false;
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);                    
                }
            }
            else
                MessageBox.Show("Chọn thông tin cần tìm", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
        private void btnXoa_HoaDonXuat_Click(object sender, RoutedEventArgs e)// XÓA HD XUẤT
        {
            if (txtmaHD_xuat.Text.Length == 0)
            {
                MessageBox.Show("Chọn thông tin cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show("Xác nhận xóa", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                hoaDonXuatHangs = db.GetTable<HoaDonXuatHang>();
                var hd = db.HoaDonXuatHangs.Single(a => a.MaHoaDon == txtmaHD_xuat.Text);
                db.HoaDonXuatHangs.DeleteOnSubmit(hd);
                db.SubmitChanges();                
                loadDataHoaDonXuat();
                MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);
            }
        }

        //
        //                    Đối tác
        //
        private void btnLoadDoiTac_Click(object sender, RoutedEventArgs e) // LÀM MỚI CÔNG TY
        {
            txtMaCongTy.Clear();
            txtTenCongTy.Clear();
            txtDiaChi.Clear();
            DataDoiTac.SelectedValue = null;
            load_NhaPhanPhoiData();
        }
        private void btnThemCongTy_Click(object sender, RoutedEventArgs e) // THÊM CÔNG TY
        {
            bool kt = false;            
            if (this.txtMaCongTy.Text.Length != 0 && this.txtTenCongTy.Text.Length != 0 && this.txtDiaChi.Text.Length != 0)
            {
                nhaPhanPhois = db.GetTable<NhaPhanPhoi>();
                var query = from u in nhaPhanPhois
                            where u.MaCongTy == txtMaCongTy.Text
                            select u;
                foreach (var i in query)
                {
                    if (i != null)
                    {
                        kt = true;
                        MessageBox.Show("Trùng mã công ty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (kt == false)
                {
                    NhaPhanPhoi a = new NhaPhanPhoi();
                    a.MaCongTy = txtMaCongTy.Text;
                    a.TenCongTy = txtTenCongTy.Text;
                    a.DiaChi = txtDiaChi.Text;
                    nhaPhanPhois.InsertOnSubmit(a);
                    db.SubmitChanges();
                    load_NhaPhanPhoiData();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                    txtMaCongTy.Clear();
                    txtTenCongTy.Clear();
                    txtDiaChi.Clear();                    
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnXoaDoiTac_Click(object sender, RoutedEventArgs e) // XÓA CÔNG TY
        {
            if(txtMaCongTy.Text =="")
            {
                MessageBox.Show("Chọn Đối tác để xóa", "lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show("Xác nhận xóa", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                nhaPhanPhois = db.GetTable<NhaPhanPhoi>();
                var a = db.NhaPhanPhois.Single(b => b.MaCongTy == txtMaCongTy.Text);
                db.NhaPhanPhois.DeleteOnSubmit(a);
                db.SubmitChanges();                
                load_NhaPhanPhoiData();
                txtDiaChi.Clear();
                MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);
            }
        }

        private void btnLoad_KhachHang_Click(object sender, RoutedEventArgs e) // load KHÁCH HÀNG
        {
            txtMaKhachHang.Clear();
            txtTenKhachHang.Clear();
            txtDiaChi_KhachHang.Clear();
            DataKhachHang.SelectedValue = null;
            load_DataKhachHang();
        }
        private void btnThemKhachHang_Click(object sender, RoutedEventArgs e) // THÊM KHÁCH HÀNG
        {
            bool kt = false;
            if (test_de_them.Text.Length != 0)
            {
                MessageBox.Show("Làm mới và thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (this.txtMaKhachHang.Text.Length != 0 && this.txtTenKhachHang.Text.Length != 0 && this.txtDiaChi_KhachHang.Text.Length != 0)
                {
                    khachHangs = db.GetTable<KhachHang>();
                    var query = from u in khachHangs
                                where u.MaKhachHang == txtMaKhachHang.Text
                                select u;
                    foreach (var i in query)
                    {
                        if (i != null)
                        {
                            kt = true;
                            MessageBox.Show("Trùng mã khách hàng", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    if (kt == false)
                    {
                        KhachHang a = new KhachHang();
                        a.MaKhachHang = txtMaKhachHang.Text;
                        a.TenKhachHang = txtTenKhachHang.Text;
                        a.DiaChi = txtDiaChi_KhachHang.Text;
                        khachHangs.InsertOnSubmit(a);
                        db.SubmitChanges();
                        load_DataKhachHang();
                        MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                        txtMaKhachHang.Clear();
                        txtTenKhachHang.Clear();
                        txtDiaChi_KhachHang.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đủ thông tin", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }                        
           
        }
        private void btnXoa_KhachHang_Click(object sender, RoutedEventArgs e) // XÓA KHÁCH HÀNG
        {
            if (txtMaKhachHang.Text == "")
            {
                MessageBox.Show("Chọn Khách hàng để xóa", "lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show("Xác nhận xóa", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                khachHangs = db.GetTable<KhachHang>();
                var a = db.KhachHangs.Single(b => b.MaKhachHang == txtMaKhachHang.Text);
                db.KhachHangs.DeleteOnSubmit(a);
                db.SubmitChanges();
                load_DataKhachHang();
                MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);                
            }
        }
        //
        //                      Nhân viên
        //
        private void btnLoadNhanVien_Click(object sender, RoutedEventArgs e) // LÀM MỚI NHÂN VIÊN
        {
            txtMaNhanVien.Clear();
            txtTenNhanVien.Clear();
            DataNhanVien.SelectedValue = null;
            load_DataNhanVien();
        }        
        private void btnThemNhanVien_Click(object sender, RoutedEventArgs e) //THÊM NHÂN VIÊN
        {
            bool kt = false, kt2 = false;         

            if (this.txtMaNhanVien.Text.Length != 0 && this.txtTenNhanVien.Text.Length != 0 && this.txtTenDangNhap.Text.Length != 0)
            {
                nhanViens = db.GetTable<NhanVien>();
                var query1 = from u in nhanViens
                             where u.MaNhanVien == txtMaNhanVien.Text
                             select u;
                foreach (var t in query1)
                {
                    if (t != null)
                    {
                        kt = true;
                    }
                }

                quanLyTaiKhoans = db.GetTable<QuanLyTaiKhoan>();
                var query = from nv in quanLyTaiKhoans
                            where nv.TenDangNhap == txtTenDangNhap.Text
                            select nv;
                foreach (var h in query)
                {
                    if (h != null)
                    {
                        kt2 = true;
                    }
                }
                if (kt == true)
                {
                    MessageBox.Show("Trùng mã nhân viên", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);                    
                }
                else if (kt2 == false)
                {
                    MessageBox.Show("Nhập đúng tên đăng nhập và thử lại", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    NhanVien a = new NhanVien();
                    a.MaNhanVien = txtMaNhanVien.Text;
                    a.TenNhanVien = txtTenNhanVien.Text;
                    a.TenDangNhap = txtTenDangNhap.Text;
                    nhanViens.InsertOnSubmit(a);
                    db.SubmitChanges();
                    load_DataNhanVien();
                    txtMaNhanVien.Clear();
                    txtTenNhanVien.Clear();
                    txtTenDangNhap.Clear();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButton.OK);
                }              
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnXoaNhanVien_Click(object sender, RoutedEventArgs e)// XÓA NHÂN VIÊN
        {
            if (txtMaNhanVien.Text == "")
            {
                MessageBox.Show("Chọn nhân viên cần xóa", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (MessageBox.Show("Xác nhận xóa", "Cảnh báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                    nhanViens = db.GetTable<NhanVien>();
                    var a = db.NhanViens.Single(b => b.MaNhanVien == txtMaNhanVien.Text);
                    db.NhanViens.DeleteOnSubmit(a);
                    db.SubmitChanges();
                    load_DataNhanVien();
                    MessageBox.Show("Đã xóa", "Thông báo", MessageBoxButton.OK);                                            
            }
        }
        private void btnDangKy_tendangnhap_Click(object sender, RoutedEventArgs e)// ĐĂNG KÝ TÀI KHOẢN ĐĂNG NHẬP
        {
            formDangKy a = new formDangKy();
            a.Show();            
        }
        private void MenuThongTinTK_Click(object sender, RoutedEventArgs e) // tài khoản
        {
            MessageBox.Show("Đang bảo trì","Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btn_LamMoi_thongKe_Click(object sender, RoutedEventArgs e)
        {
            Date_ThongKe1.SelectedDate = null;
            Date_ThongKe2.SelectedDate = null;
            load_DataThongKeNhap();
        }

        private void btn_Loc_ThongKe_Click(object sender, RoutedEventArgs e)
        {
            hoaDonNhapHangs = db.GetTable<HoaDonNhapHang>();
            bool kt = false;
            if (Date_ThongKe1.SelectedDate != null || Date_ThongKe2.SelectedDate != null)// Lọc
            {
                var query1 = from b in hoaDonNhapHangs
                             where b.Ngaynhap == Date_ThongKe1.SelectedDate || b.Ngaynhap == Date_ThongKe2.SelectedDate
                             select new
                             {                                                           
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 ThanhTien = b.ThanhTien
                             };
                foreach (var i in query1)
                    if (i != null)
                    {
                        kt = true;
                    }
                if (kt == true)
                {
                    Data_ThongKeHangNhap.ItemsSource = query1;                                      
                    kt = false;
                }
                
                var query2 = from b in hoaDonXuatHangs
                             where b.Ngaynhap == Date_ThongKe1.SelectedDate || b.Ngaynhap == Date_ThongKe2.SelectedDate
                             select new
                             {                                 
                                 MaSanPham = b.MaSanPham,
                                 TenSanPham = b.TenSanPham,
                                 NgayNhap = b.Ngaynhap,
                                 SoLuong = b.SoLuong,
                                 DonGia = b.DonGia,
                                 ThanhTien = b.TongTien
                             };
                foreach (var i2 in query2)
                    if (i2 != null)
                    {
                        kt = true;
                    }
                if (kt == true)
                {
                    Data_ThongKeHangXuat.ItemsSource = query2;                  
                }
                else
                {
                    MessageBox.Show("Không có hóa đơn nào!", "Thông báo", MessageBoxButton.OK);                   
                    loadDataHoaDonXuat();
                }
            }
            else
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin", "Thông báo", MessageBoxButton.OK);
        }
        private void btn_Tim_Thongke_Nhaphang_Click(object sender, RoutedEventArgs e)
        {
            var sv = (from a in db.HoaDonNhapHangs where a.TenSanPham.Contains(txt_ThongKeNhapHang.Text) select a);
            Data_ThongKeHangNhap.ItemsSource = sv;
        }
        private void btn_Tim_Thongke_Xuathang_Click(object sender, RoutedEventArgs e)
        {
            var sv = (from a in db.HoaDonXuatHangs where a.TenSanPham.Contains(txt_ThongKeXuatHang.Text) select a);
            Data_ThongKeHangXuat.ItemsSource = sv;
        }

        private void btn_LamMoi_thongKe_XuatHang_Click(object sender, RoutedEventArgs e)
        {
            loadDataHoaDonXuat();
            txt_ThongKeXuatHang.Text = "0";
            Date_ThongKe1.SelectedDate = null;
            Date_ThongKe2.SelectedDate = null;
        }
    }
}