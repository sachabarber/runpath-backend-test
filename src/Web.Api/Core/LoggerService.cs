using System;
using Web.Api.Core.Interfaces;

namespace Web.Api.Core
{
    ///<inheritdoc/>
    public class LoggerService : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
