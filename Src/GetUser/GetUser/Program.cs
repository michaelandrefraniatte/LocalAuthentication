using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetUser
{
    internal class Program
    {
        private static void OnKeyDown(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                const string message = "• Author: Michaël André Franiatte.\n\r\n\r• Contact: michael.franiatte@gmail.com.\n\r\n\r• Publisher: https://github.com/michaelandrefraniatte.\n\r\n\r• Copyrights: All rights reserved, no permissions granted.\n\r\n\r• License: Not open source, not free of charge to use.";
                const string caption = "About";
                MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        [DllImport("advapi32.dll")]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        static void Main(string[] args)
        {
            Console.WriteLine("\tEnter a username");
            string username = Console.ReadLine();
            Console.WriteLine("\tEnter a password");
            string password = Console.ReadLine();
            IntPtr token;
            try
            {
                if (LogonUser(username, null, password, 3, 0, out token))
                {
                    Console.WriteLine(token);
                }
            }
            catch { }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}