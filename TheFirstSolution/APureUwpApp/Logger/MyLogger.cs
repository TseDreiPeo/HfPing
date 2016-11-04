using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

namespace APureUwpApp.Logger {
    public class MyLogger :ILogger {
        private const string LOGFILE_NAME = "MyLogFile.txt";
        private static StorageFile LogFile = StaticInitializer();

        private static StorageFile StaticInitializer() {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            var t = storageFolder.CreateFileAsync(LOGFILE_NAME, CreationCollisionOption.OpenIfExists).AsTask();
            t.Wait();
            return t.Result;
        }

        public void Log(string line) {
            //if(LogFile != null) {
            //    long ticks = DateTime.Now.Ticks;
            //    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            //    var t = storageFolder.CreateFileAsync(LOGFILE_NAME, CreationCollisionOption.OpenIfExists).AsTask();
            //    t.Wait();
            //    LogFile = t.Result;
            //    line = $"New Log File Referenz ({LogFile.Path}) created at {DateTime.Now.ToString("HH:mm:ss.fff")} (took: {(DateTime.Now.Ticks - ticks) / 10000} ms).{Environment.NewLine} --> Orig Line: {line}";
            //}
            if(LogFile != null) {
                // Wir starten das Logging ins file aber wir warten nicht aufs fertig schreiben ...
                var dontCare = LogAsyncAction(line);
            }
        }

        public ulong GetCurrentLogFileSize() {
            var t = LogFile.GetBasicPropertiesAsync().AsTask();
            t.Wait();
            Windows.Storage.FileProperties.BasicProperties basicProperties = t.Result;
            return basicProperties.Size;
        }

        public void LogFinalize(string line) {
            var letsWait = LogAsyncAction(line).AsTask();
            letsWait.Wait();
        }

        private IAsyncAction LogAsyncAction(string line) {
            return FileIO.AppendTextAsync(LogFile, $"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff")}: {line}{Environment.NewLine}");
        }
    }
}
