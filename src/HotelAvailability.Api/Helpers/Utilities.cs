/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Api helpers utilities
/// </summary>

namespace HotelAvailability.Api.Helpers
{
    #region import namespaces

    using System;
    using System.IO;
    using Microsoft.Extensions.Logging;

    #endregion

    public static class Utilities
    {
        static ILoggerFactory _loggerFactory;

        public static void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static ILogger CreateLogger<T>()
        {
            if (_loggerFactory == null)
            {
                throw new InvalidOperationException($"{nameof(ILogger)} is not configured. {nameof(ConfigureLogger)} must be called before use");
            }

            return _loggerFactory.CreateLogger<T>();
        }

        public static void QuickLog(string text, string filename)
        {
            string dirPath = Path.GetDirectoryName(filename);

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine($"{DateTime.Now} - {text}");
            }
        }
    }
}
