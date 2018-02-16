using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            SqlConnection connection =
                new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30");
            SqlDataAdapter sqlDataAdapter =
                new SqlDataAdapter("Select count(*) from users where username ='" + textBox1.Text + "' and password ='" + GetHashString(textBox2.Text) + "'", connection);

            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            if (dataTable.Rows[0][0].ToString() == "1") {
                this.Hide();
                Form2 form2 = new Form2();
                form2.Show();
            } else {
                MessageBox.Show("Input correct username and password");
            }
            //поменять все на EntityFramework
        }

        string GetHashString(string s)
        {
            //строку в байт-массив
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //объект для шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //хеш в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //строка из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

    }
}
