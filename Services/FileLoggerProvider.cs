using Microsoft.Extensions.Logging;
using Karma.Models;
using Microsoft.Extensions.Options;
using System.IO;

namespace Karma.Services
{
    public class FileLoggerProvider : ILoggerProvider
    {
        public readonly FileLoggerOptions options;

        public FileLoggerProvider(IOptions<FileLoggerOptions> _options)
        {
            options = _options.Value;

            if (!Directory.Exists(options.FolderPath))
            {
                Directory.CreateDirectory(options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(this);
        }

        public void Dispose()
        {
        }
    }
}
