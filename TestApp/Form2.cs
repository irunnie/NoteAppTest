using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form2 : Form
    {
        //поменять все на EntityFramework
        DataTable table;
        String connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30";
        static int id = 0;


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Messages", typeof(String));

            dataGridView1.DataSource = table;
            dataGridView1.Columns["Messages"].Visible = false;
            dataGridView1.Columns["Title"].Width = 150;
        }

        private void bttNew_Click(object sender, EventArgs e)
        {
            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void bttSave_Click(object sender, EventArgs e)
        {
            String sqlExpression = "OPEN SYMMETRIC KEY SSN_Key_01 DECRYPTION BY CERTIFICATE cert1; INSERT INTO usersdata(id, title, dataText) values(" + id++ + ", '" + txtTitle.Text + "', EncryptByKey(Key_GUID('SSN_Key_01'), '" + txtMessage.Text + "'))";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number);
                Console.WriteLine("Id: {0}", id);
            }

            table.Rows.Add(txtTitle.Text, txtMessage.Text);
            txtTitle.Clear();
            txtMessage.Clear();
        }

        private void bttRead_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sqlExpression = "OPEN SYMMETRIC KEY SSN_Key_01 DECRYPTION BY CERTIFICATE cert1; SELECT title, convert(char,DecryptByKey([dataText])) as p from usersData go";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlExpression, connection);
                sqlDataAdapter.Fill(dataTable);

                Console.WriteLine(dataGridView1.CurrentCell.RowIndex.ToString());
            }
            
            if (index > -1) {
                txtTitle.Text = dataTable.Rows[index].ItemArray[0].ToString();
                txtMessage.Text = dataTable.Rows[index].ItemArray[1].ToString();
            }
        }

        private void bttDelete_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            table.Rows[index].Delete();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                String sqlExpression = "DELETE from usersData WHERE id =" + index;
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("Удалено объектов: {0}", number);
            }
            }
    }
}
