using NUnit.Framework;
using System;
namespace Markdig.BadHeaders.Tests
{
    [TestFixture]
    public class HeaderTests
    {
        private MarkdownPipeline _pipeline;

        private const string BAD_HEADER_MARKDOWN = 
            "#Title\nsome body text and *such*\n##Subtitle with other words\n";
        private const string BAD_HEADER_MARKDOWN_WRAPPED =
            "#Title#\nsome body text and *such*\n##Subtitle with other words##\n";

        private const string GOOD_HEADER_MARKDOWN = 
            "# Title\nsome body text and *such*\n## Subtitle with other words\n";
        private const string GOOD_HEADER_MARKDOWN_WRAPPED =
            "# Title #\nsome body text and *such*\n## Subtitle with other words ##\n";
        private const string MIXED_HEADER_MARKDOWN =
            "#Title\nsome body text and *such*\n## Subtitle with other words ##\n";

        [SetUp]
        public void SetUp()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder = MarkdownExtensions.Use<BadHeadersExtension>(pipelineBuilder);
            _pipeline = pipelineBuilder.Build();
        }

        [Test]
        public void TestWithBadHeaders()
        {
            var html1 = Markdown.ToHtml(BAD_HEADER_MARKDOWN, _pipeline);
            var html2 = Markdown.ToHtml(BAD_HEADER_MARKDOWN_WRAPPED, _pipeline);

            Assert.IsTrue(html1.Contains("<h1>"));
            Assert.IsTrue(html2.Contains("<h1>"));
            Assert.IsTrue(html1.Contains("<h2>"));
            Assert.IsTrue(html2.Contains("<h2>"));
        }

        [Test]
        public void TestWithGoodHeaders()
        {
            var html1 = Markdown.ToHtml(GOOD_HEADER_MARKDOWN, _pipeline);
            var html2 = Markdown.ToHtml(GOOD_HEADER_MARKDOWN_WRAPPED, _pipeline);

            Assert.IsTrue(html1.Contains("<h1>"));
            Assert.IsTrue(html2.Contains("<h1>"));
            Assert.IsTrue(html1.Contains("<h2>"));
            Assert.IsTrue(html2.Contains("<h2>"));
        }

        [Test]
        public void TestWithMixedHeaders()
        {
            var html1 = Markdown.ToHtml(MIXED_HEADER_MARKDOWN, _pipeline);

            Assert.IsTrue(html1.Contains("<h1>"));
            Assert.IsTrue(html1.Contains("<h2>"));
        }
    }
}
