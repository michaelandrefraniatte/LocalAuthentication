using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);
        private static string username, password;
        static void Main(string[] args)
        {
            Console.WriteLine("\tEnter a username");
            username = Console.ReadLine();
            Console.WriteLine("\tEnter a password");
            password = Console.ReadLine();
            try
            {
                if (GetUser())
                {
                    Console.WriteLine("ok");
                }
            }
            finally
            {
                Console.ReadKey();
            }
        }
        static bool GetUser()
        {
            try
            {
                IntPtr token;
                bool result = LogonUser(username, null, password, 3, 0, out token);
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine(error);
                return result | error == 1327;
            }
            catch
            {
                return false;
            }
        }
    }
}