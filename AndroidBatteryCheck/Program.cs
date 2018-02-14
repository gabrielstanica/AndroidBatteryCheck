using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidBatteryCheck
{
    class Program
    {
        public static List<string> devices = new List<string>();

        public static void WaitDevices(string deviceIds)
        {
            var list = deviceIds.Split(',');
            //List<Task> taskList = new List<Task>();


            foreach (var item in list)
            {
                //taskList.Add(new Task(
                //        () => StartProcess(item)
                //    ));
                StartProcess(item);
            }

            //taskList.ForEach(t => t.Start());
            //taskList.ForEach(t => t.Wait());
        }

        public static void StartProcess(string deviceId)
        {
            string[] output = new string[] { };

            //while (true)
            //{
            //    try
            //    {
            string command = string.Format("-s {0} shell dumpsys battery", deviceId);
            string line = String.Empty;

            //execute process and get output to process
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "adb.exe ";
            startInfo.Arguments = command;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            StreamReader reader = process.StandardOutput;
            process.WaitForExit();

            using (StreamReader read = reader)
            {
                while ((line = read.ReadLine()) != null)
                {
                    if (line.Contains("level"))
                    {
                        output = line.Split(' ');
                    }
                }
            }
            var infoMessage = String.Format("DeviceId {0} - battery level {1}", deviceId, output[3]);

            if (Convert.ToInt32(output[3]) >= 40)
            {
                devices.Add(deviceId);
            }


            //    Console.WriteLine(infoMessage);
            //    Console.WriteLine("Waiting for 1 minute");
            //    Thread.Sleep(TimeSpan.FromMinutes(1));
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("Retrying ADB command");
            //    Console.WriteLine(ex.Message);
            //}
            //}
        }

        static void Main(string[] args)
        {
            WaitDevices(args[0]);
            string deviceList = String.Empty;
            foreach(var device in devices)
            {
                deviceList += device + ",";
            }

            deviceList = deviceList.Remove(deviceList.Length -1, 1);
            Console.WriteLine(deviceList);
        }
    }
}
