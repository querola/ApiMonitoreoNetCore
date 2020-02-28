using System;
using System.IO;

namespace monitoreoApiNetCore.Interfaces{
    public interface ILog
    {
        public void WriteLog(Exception exception, string method, string logFile);
        public void WriteLog(string info, string logFile);
    }
}