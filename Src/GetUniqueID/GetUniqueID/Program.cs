using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
namespace GetUniqueID
{
    class Program
    {
        static void Main(string[] args)
        {
            string getuniqueid = getUniqueId();
            using (System.IO.StreamWriter createdfile = System.IO.File.AppendText("la.txt"))
            {
                createdfile.WriteLine(getuniqueid);
                createdfile.Close();
            }
            Console.WriteLine("UniqueId : " + getuniqueid);
            Console.ReadLine();
        }
        public static string getUniqueId()
        {
            try
            {
                string cpuInfo = string.Empty;
                ManagementClass mc = new ManagementClass("win32_processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
                string drive = "C";
                ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
                dsk.Get();
                string volumeSerial = dsk["VolumeSerialNumber"].ToString();
                string uuidInfo = string.Empty;
                ManagementClass mcu = new ManagementClass("Win32_ComputerSystemProduct");
                ManagementObjectCollection mocu = mcu.GetInstances();
                foreach (ManagementObject mou in mocu)
                {
                    uuidInfo = mou.Properties["UUID"].Value.ToString();
                    break;
                }
                if (volumeSerial != null & volumeSerial != "" & cpuInfo != null & cpuInfo != "" & uuidInfo != null & uuidInfo != "")
                    return volumeSerial + "-" + cpuInfo + "-" + uuidInfo;
                else
                    return null;
            }
            catch
            {
                Application.Exit();
                return null;
            }
        }
    }
}
