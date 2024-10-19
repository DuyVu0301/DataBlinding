using DataBlinding.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataBlinding
{
    public partial class Form1 : Form
    {
        private BindingSource studentBindingSource = new BindingSource();
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadData()
        {
            using (var context = new Model1())
            {
                var students = context.Students.ToList();
                studentBindingSource.DataSource = students;
                dataGridView1.DataSource = studentBindingSource;
            }
        }
        private void BindControls()
        {
            txtFullName.DataBindings.Add("Text", studentBindingSource, "FullName");
            txtAge.DataBindings.Add("Text", studentBindingSource, "Age");
            cmbMajor.DataBindings.Add("Text", studentBindingSource, "Major");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            BindControls();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (var context = new Model1())
            {
                var student = new Student
                {
                    FullName = txtFullName.Text,
                    Age = int.Parse(txtAge.Text),
                    Major = cmbMajor.Text
                };
                context.Students.Add(student);
                context.SaveChanges();
                LoadData();
                ClearFields();
            }
        }
        private void ClearFields()
        {
            txtFullName.Text = "";
            txtAge.Text = "";
            cmbMajor.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có dòng nào được chọn không
            if (studentBindingSource.Current is Student student)
            {
                // Cập nhật thông tin sinh viên từ các TextBox và ComboBox
                student.FullName = txtFullName.Text;
                student.Age = int.Parse(txtAge.Text);
                student.Major = cmbMajor.Text;

                using (var context = new Model1())
                {
                    // Đính kèm đối tượng sinh viên vào ngữ cảnh
                    context.Students.Attach(student);
                    context.Entry(student).State = EntityState.Modified; // Đánh dấu sinh viên là đã sửa
                    context.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                }

                // Tải lại dữ liệu trong DataGridView
                LoadData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (studentBindingSource.Current is Student student)
            {
                using (var context = new Model1())
                {
                    context.Students.Attach(student);
                    context.Students.Remove(student);
                    context.SaveChanges();
                    LoadData();
                }
            }
        }

       

        private void button4_Click(object sender, EventArgs e)
        {
            studentBindingSource.MovePrevious();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            studentBindingSource.MoveNext();
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection(); // Bỏ chọn tất cả dòng
        }
    }
}
