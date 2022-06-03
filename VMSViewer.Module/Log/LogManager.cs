using System;
using System.IO;

namespace VMSViewer
{
    public class LogManager
    {
        private static LogManager _shared;

        public static LogManager Shared
        {
            get
            {
                if (_shared == null)
                {
                    _shared = new LogManager();
                }

                return _shared;
            }
        }

        public void AddLog(string Log)
        {
            /* 로그파일 저장경로 */
            if (Directory.Exists(GetSaveDirectory()) == false)
                Directory.CreateDirectory(GetSaveDirectory());

            var log = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} ### {Log} ###";
            var savePath = $"{GetSaveDirectory()}\\{GetFilename()}";

            Console.WriteLine(log);

            if (File.Exists(savePath))
            {
                using (StreamWriter streamWriter = File.AppendText(savePath))
                {
                    streamWriter.WriteLine(log);
                    streamWriter.Close();
                }
            }
            else
            {
                using (StreamWriter streamWriter = new StreamWriter(savePath))
                {
                    streamWriter.WriteLine(log);
                    streamWriter.Close();
                }
            }
        }

        private string GetSaveDirectory() { return $"{AppDomain.CurrentDomain.BaseDirectory}\\Log"; }
        private string GetFilename() { return $"log{DateTime.Now.ToString("yyyyMMdd")}.txt"; }
    }
}
