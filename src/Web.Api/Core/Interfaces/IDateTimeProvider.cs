using System;

namespace Web.Api.Core.Interfaces
{
    /// <summary>
    /// Simple interface to allow mocking of Dates
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}
