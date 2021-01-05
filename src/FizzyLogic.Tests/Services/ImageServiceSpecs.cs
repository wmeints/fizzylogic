namespace FizzyLogic.Tests.Services
{
    using Chill;
    using FizzyLogic.Services;
    using FluentAssertions;
    using System;
    using System.IO;
    using Xunit;

    public class WhenStoringAnImage : GivenSubject<ImageService, string>
    {
        public WhenStoringAnImage()
        {
            _ = SetThe<IClock>().To(ObjectMother.StaticClock(new DateTime(2020, 1, 1)));

            WithSubject(resolver => new ImageService(resolver.Get<IClock>()));

            When(() => Subject.UploadImage("test.png", GenerateImageStream()));
        }

        protected override void Dispose(bool disposing)
        {
            if (Directory.Exists("wwwroot/content/images"))
            {
                Directory.Delete("wwwroot/content/images", true);
            }
        }

        [Fact]
        public void ThenTheRelativePathToTheImageIsReturned()
        {
            _ = Result.Should().Be("/content/images/2020/01/01/test.png");
        }

        [Fact]
        public void ThenTheFileIsStoredOnDisk()
        {
            _ = File.Exists("wwwroot/content/images/2020/01/01/test.png").Should().BeTrue();
        }

        private Stream GenerateImageStream()
        {
            return new MemoryStream(new byte[4096]);
        }
    }
}
