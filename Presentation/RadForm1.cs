using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using System.IO;
namespace Presentation
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        string path = "../../Hinh";
        Table<SINHVIEN> Bang_SINHVIEN;
        Table<LOP> Bang_LOP;
        QLSinhVienDBDataContext db;
        BindingManagerBase DSSV;

        public RadForm1()
        {
            InitializeComponent();
        }

       
        private void RadForm1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qLSINHVIEN4DataSet.SINHVIEN' table. You can move, or remove it, as needed.
            this.sINHVIENTableAdapter.Fill(this.qLSINHVIEN4DataSet.SINHVIEN);
            db = new QLSinhVienDBDataContext();
            Bang_SINHVIEN = db.SINHVIENs;
            Bang_LOP = db.LOPs;

            loadCBOLop();
            LoadDGVHocSinh();

            rtxt_MaSV.DataBindings.Add("text", Bang_SINHVIEN, "MaSV", true);
            rtxt_HoTen.DataBindings.Add("text", Bang_SINHVIEN, "HoTen", true);
            radDateTimePicker1.DataBindings.Add("text", Bang_SINHVIEN, "NgaySinh", true);
            rrad_Nam.DataBindings.Add("Ischecked", Bang_SINHVIEN, "GioiTinh", true);
            radcbo_Lop.DataBindings.Add("SelectedValue", Bang_SINHVIEN, "MaLop", true);
            rtxt_QueQuan.DataBindings.Add("text", Bang_SINHVIEN, "DiaChi", true);
            pictureBox1.DataBindings.Add("ImageLocation", Bang_SINHVIEN, "Hinh", true);

            DSSV = this.BindingContext[Bang_SINHVIEN];
            enableNutLenh(false);
        }
        private void enableNutLenh(bool isEnabled)
        {
            rbtn_Them.Enabled = !isEnabled;
            rbtn_Xoa.Enabled = !isEnabled;
            rbtn_Sua.Enabled = !isEnabled;
            rbtn_Thoat.Enabled = !isEnabled;
            rbtn_Lưu.Enabled = isEnabled;
            rbtn_Huy.Enabled = isEnabled;
        }

        private void LoadDGVHocSinh()
        {
            radGridView1.AutoGenerateColumns = false;
            radGridView1.DataSource =Bang_SINHVIEN;
        }

        private void loadCBOLop()
        {
            radcbo_Lop.DataSource = Bang_LOP;
            radcbo_Lop.DisplayMember = "TenLop";
            radcbo_Lop.ValueMember = "MaLop";
        }

        private void rrad_Nam_CheckStateChanged(object sender, EventArgs e)
        {
            rrad_Nu.IsChecked = !rrad_Nam.IsChecked;
        }

        private void rbtn_Them_Click(object sender, EventArgs e)
        {
            DSSV.AddNew();
            enableNutLenh(true);
        }

        private void rbtn_Lưu_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.EndCurrentEdit();
                db.SubmitChanges();
                enableNutLenh(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtn_Sua_Click(object sender, EventArgs e)
        {
            enableNutLenh(true);
        }


        private void rbtn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                DSSV.RemoveAt(DSSV.Position);
                db.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbtn_Huy_Click(object sender, EventArgs e)
        {
            DSSV.CancelCurrentEdit();
            ChangeSet cs = db.GetChangeSet();
            db.Refresh(RefreshMode.OverwriteCurrentValues, cs.Updates);
            enableNutLenh(false);
        }

        private void rbtn_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtxt_Timkiem_MouseDown(object sender, MouseEventArgs e)
        {
            rtxt_Timkiem.Text = "";
        }

        private void rbtn_ChonHinh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "JPG File|*.jpg|PNG File|*.png|All File|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.SafeFileName;
                string pathFile = path + "/" + fileName;
                if (!File.Exists(pathFile)) File.Copy(openFileDialog1.FileName, pathFile);
                pictureBox1.ImageLocation = pathFile;
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DSSV.Position = Bang_SINHVIEN.ToList().FindIndex(sv => sv.MaSV.Contains(rtxt_Timkiem.Text));
        }

        
    }
}
