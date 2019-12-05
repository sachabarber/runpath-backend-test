using System;

namespace Web.Api.Core.Interfaces
{
    ///<inheritdoc/>
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
