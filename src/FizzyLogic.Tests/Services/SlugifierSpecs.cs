namespace FizzyLogic.Tests.Services
{
    using Chill;
    using FizzyLogic.Services;
    using FluentAssertions;
    using Xunit;

    public class SlugifierWhenProcessingATitleWithSpaces : GivenSubject<Slugifier, string>
    {
        public SlugifierWhenProcessingATitleWithSpaces()
        {
            WithSubject(resolver => new Slugifier());
            When(() => Subject.Process("sample title"));
        }

        [Fact]
        public void ThenSpacesAreReplacedForDashes()
        {
            _ = Result.Should().Be("sample-title");
        }
    }

    public class SlugifierWhenProcessingATitleWithCapitals : GivenSubject<Slugifier, string>
    {
        public SlugifierWhenProcessingATitleWithCapitals()
        {
            WithSubject(resolver => new Slugifier());
            When(() => Subject.Process("Sample"));
        }

        [Fact]
        public void ThenCapitalsAreLowerCased()
        {
            _ = Result.Should().Be("sample");
        }
    }

    public class SlugifierWhenProcessingATitleWithSpecialCharacters : GivenSubject<Slugifier, string>
    {
        public SlugifierWhenProcessingATitleWithSpecialCharacters()
        {
            WithSubject(resolver => new Slugifier());
            When(() => Subject.Process("hellö"));
        }

        [Fact]
        public void ThenSpecialCharactersAreNormalized()
        {
            _ = Result.Should().Be("hello");
        }
    }

    public class SlugifierWhenProcessingATitleWithDashesAtTheEnd : GivenSubject<Slugifier, string>
    {
        public SlugifierWhenProcessingATitleWithDashesAtTheEnd()
        {
            WithSubject(resolver => new Slugifier());
            When(() => Subject.Process("title-"));
        }

        [Fact]
        public void ThenDashesAreRemovedFromTheEnd()
        {
            _ = Result.Should().Be("title");
        }
    }

    public class SlugifierWhenProcessingATitleWithMultipleDashes : GivenSubject<Slugifier, string>
    {
        public SlugifierWhenProcessingATitleWithMultipleDashes()
        {
            WithSubject(resolver => new Slugifier());
            When(() => Subject.Process("test--title"));
        }

        [Fact]
        public void ThenSingleDashesRemain()
        {
            _ = Result.Should().Be("test-title");
        }
    }
}
