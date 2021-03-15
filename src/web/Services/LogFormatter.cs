using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CurrencyExchangeAPI.Services
{
    public interface ILogFormatter
    {
        public string FormatMessage(LogType logType, string details, [CallerMemberName] string caller = "");
    }
    public class LogFormatter : ILogFormatter
    {
        public LogFormatter(LogFormatterOptions options)
        {
            Subject = options.Subject;
        }

        public string Subject { get; set; }

        public string FormatMessage(LogType logType, string details, [CallerMemberName] string caller = "")
        {
            return $"{Subject} - {logType}: caller: {caller} - {details}";
        }
    }

    public enum LogType
    {
        Debug,
        Information,
        Warning,
        Error
    }

    public class LogFormatterOptions
    {
        public string Subject { get; set; }
    }
    
}
