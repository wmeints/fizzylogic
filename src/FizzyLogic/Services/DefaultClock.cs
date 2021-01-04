using System;

namespace FizzyLogic.Services
{
    /// <summary>
    /// Default implementation of the application clock.
    /// </summary>
    public class DefaultClock : IClock
    {
        /// <inheritdoc/>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
