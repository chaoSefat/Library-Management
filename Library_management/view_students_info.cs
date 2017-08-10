using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace Library_management
{
    public partial class view_students_info : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=.\sqlexpress;Initial Catalog=library_management;Integrated Security=True;Pooling=False");
        string pwd = Class1.GetRandomPassword(20);
        string wanted_path;
        DialogResult result;
        public view_students_info()
        {
            InitializeComponent();
        }

        private void view_students_info_Load(object sender, EventArgs e)
        {
            int i = 0;
            if(con.State==ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            fill_grid();

            
        }
        public void fill_grid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
            int i = 0;
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from students_info where student_name like('%" + textBox1.Text + "%')";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            Bitmap img;
            DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
            imageCol.HeaderText = "student image";
            imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imageCol.Width = 100;
            dataGridView1.Columns.Add(imageCol);
            foreach (DataRow dr in dt.Rows)
            {
                img = new Bitmap(@"..\..\" + dr["student_image"].ToString());
                dataGridView1.Rows[i].Cells[8].Value = img;
                dataGridView1.Rows[i].Height = 100;
                i = i + 1;

            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Refresh();
                try
                {
                    int i = 0;
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * from students_info where student_name like('%" + textBox1.Text + "%')";
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    Bitmap img;
                    DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                    imageCol.HeaderText = "student image";
                    imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    imageCol.Width = 100;
                    dataGridView1.Columns.Add(imageCol);
                    foreach (DataRow dr in dt.Rows)
                    {
                        img = new Bitmap(@"..\..\" + dr["student_image"].ToString());
                        dataGridView1.Rows[i].Cells[8].Value = img;
                        dataGridView1.Rows[i].Height = 100;
                        i = i + 1;

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wanted_path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            result = openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select * from students_info where id=" + i + "";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    studentname.Text = dr["student_name"].ToString();
                    studentenroll.Text = dr["student_enrollment_no"].ToString();
                    studentdept.Text = dr["student_department"].ToString();
                    studentsem.Text = dr["student_sem"].ToString();
                    studentcontact.Text = dr["student_contact"].ToString();
                    studentemail.Text = dr["student_email"].ToString();


                }
            
            }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (result == DialogResult.OK)
            {
                int i;
                i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
                string img_path;
                File.Copy(openFileDialog1.FileName, wanted_path + "\\student_images\\" + pwd + ".jpg");
                img_path = "student_images\\" + pwd + ".jpg";

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update students_info set student_name='" + studentname.Text + "',student_image='" + img_path.ToString() + "',student_enrollment_no='" + studentenroll.Text + "',student_department='" + studentdept.Text + "',student_sem='" + studentsem.Text + "',student_contact='" + studentcontact.Text + "',student_email='" + studentemail.Text + "' where id=" + i + " ";
                cmd.ExecuteNonQuery();
                fill_grid();
                MessageBox.Show("records updated successfully");
                

            }
            else if ( result==DialogResult.Cancel)
            {
                int i;
                i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "update students_info set student_name='" + studentname.Text + "',student_enrollment_no='" + studentenroll.Text + "',student_department='" + studentdept.Text + "',student_sem='" + studentsem.Text + "',student_contact='" + studentcontact.Text + "',student_email='" + studentemail.Text + "' where id=" + i + " ";
                cmd.ExecuteNonQuery();
                fill_grid();
                MessageBox.Show("records updated successfully");

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
