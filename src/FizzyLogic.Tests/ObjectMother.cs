namespace FizzyLogic.Tests
{
    using FizzyLogic.Services;
    using NSubstitute;
    using System;

    /// <summary>
    /// Provides mocks and other useful tidbits of data for the tests.
    /// </summary>
    public class ObjectMother
    {
        /// <summary>
        /// Creates a clock set to a fixed date/time
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IClock StaticClock(DateTime date)
        {
            var staticClock = Substitute.For<IClock>();
            _ = staticClock.UtcNow.Returns(date);

            return staticClock;
        }
    }
}
