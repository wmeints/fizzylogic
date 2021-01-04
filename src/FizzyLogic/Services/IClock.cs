namespace FizzyLogic.Services
{
    using System;

    /// <summary>
    /// Returns the current date and time.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Gets the current date/time in UTC format.
        /// </summary>
        DateTime UtcNow { get; }
    }
}
