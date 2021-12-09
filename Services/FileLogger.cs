using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Karma.Services
{
    public class FileLogger : ILogger
    {
        protected readonly FileLoggerProvider _fileLoggerProvider;
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public FileLogger([NotNull] FileLoggerProvider fileLoggerProvider)
        {
            _fileLoggerProvider = fileLoggerProvider;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var fullFilePath = _fileLoggerProvider.options.FolderPath + "/" + _fileLoggerProvider.options.FilePath.Replace("{date}", DateTimeOffset.UtcNow.ToString("yyyy-MM-dd"));
            var logRecord = string.Format("{0} [{1}] {2} {3}", "[" + DateTimeOffset.UtcNow.ToString("yyyy-MM-dd HH:mm:ss+00:00") + "]", logLevel.ToString(), formatter(state, exception), exception == null ? "" : exception.StackTrace);

            _readWriteLock.EnterWriteLock();
            try
            {
                using (StreamWriter w = File.AppendText(fullFilePath))
                    {
                        w.WriteLine(logRecord);
                        w.Close();
                    }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}
