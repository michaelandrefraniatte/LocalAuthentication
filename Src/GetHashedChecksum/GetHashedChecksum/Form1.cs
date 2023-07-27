using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
namespace GetHashedChecksum
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static string username, hash, userchecksum;
        const string alphanumeric = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader("auth.txt"))
                {
                    username = file.ReadLine();
                    hash = file.ReadLine();
                    file.Close();
                }
                string exePath = op.FileName;
                SHA1 sha1 = SHA1.Create();
                FileStream fs = new FileStream(exePath, FileMode.Open, FileAccess.Read);
                string checksum = BitConverter.ToString(sha1.ComputeHash(fs)).Replace("-", "");
                fs.Close();
                textBox1.Text = username;
                textBox2.Text = checksum;
                textBox3.Text = hash;
                userchecksum = username + checksum;
                string salt = GetSalt(10); // 10 is the size of Salt 
                string hashedPass = HashPassword(salt, userchecksum);
                textBox4.Text = hashedPass;
                if (hash == hashedPass)
                    MessageBox.Show("It's all good.");
                else
                    MessageBox.Show("It's not good.");
            }
        }
        public static string GetSalt(int saltSize)
        {
            float key = 0.6f;
            StringBuilder strB = new StringBuilder("");
            while ((saltSize--) > 0)
                strB.Append(alphanumeric[(int)(key * alphanumeric.Length)]);
            return strB.ToString();
        }
        public static string HashPassword(string salt, string password)
        {
            string mergedPass = string.Concat(salt, password);
            return EncryptUsingMD5(mergedPass);
        }
        public static string EncryptUsingMD5(string inputStr)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(inputStr));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                    sBuilder.Append(data[i].ToString("x2"));

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
