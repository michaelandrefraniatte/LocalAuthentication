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
using System.Security.Cryptography;
namespace TextCryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string contents;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                EncryptFile(op.FileName, "outputFile.txt", "tybtrybrtyertu50727885");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files(*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                DecryptFile(op.FileName, "inputFile.txt", "tybtrybrtyertu50727885");
            }
        }
        public static void EncryptFile(string inputFile, string outputFile, string password)
        {
            string contents = File.ReadAllText(inputFile);
            byte[] salt = new byte[8];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(salt);
            using (var encryptedStream = new MemoryStream())
            {
                StreamWriter sw = new StreamWriter(encryptedStream);
                sw.Write(contents);
                sw.Flush();
                encryptedStream.Seek(0, SeekOrigin.Begin);
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var encryptor = aes.CreateEncryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var output = File.Create(outputFile))
                {
                    output.Write(salt, 0, salt.Length);
                    using (var cs = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
                        encryptedStream.CopyTo(cs);
                    encryptedStream.Flush();
                }
            }
        }
        public static void DecryptFile(string inputFile, string outputFile, string password)
        {
            using (var input = File.OpenRead(inputFile))
            {
                byte[] salt = new byte[8];
                input.Read(salt, 0, salt.Length);
                using (var decryptedStream = new MemoryStream())
                using (var pbkdf = new Rfc2898DeriveBytes(password, salt))
                using (var aes = new RijndaelManaged())
                using (var decryptor = aes.CreateDecryptor(pbkdf.GetBytes(aes.KeySize / 8), pbkdf.GetBytes(aes.BlockSize / 8)))
                using (var cs = new CryptoStream(input, decryptor, CryptoStreamMode.Read))
                {
                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        decryptedStream.WriteByte((byte)data);
                    decryptedStream.Position = 0;
                    using (StreamReader sr = new StreamReader(decryptedStream))
                        contents = sr.ReadToEnd();
                    decryptedStream.Flush();
                    File.WriteAllText(outputFile, contents);
                }
            }
        }
    }
}
