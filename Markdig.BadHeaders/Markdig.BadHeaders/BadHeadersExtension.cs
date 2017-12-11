using System;
using Markdig.Renderers;

namespace Markdig.BadHeaders
{
    /// <summary>
    /// Markdig markdown extension for handling bad markdown for titles
    /// </summary>
    public class BadHeadersExtension : IMarkdownExtension
    {
        /// <summary>
        /// Sets up the extension to use the badheading block parser
        /// </summary>
        /// <returns>The setup.</returns>
        /// <param name="pipeline">Pipeline.</param>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<BadHeadingBlockParser>())
            {
                // Insert the parser before any other parsers and use '#' as the character identifier
                pipeline.BlockParsers.Insert(0, new BadHeadingBlockParser('#'));
            }
        }

        /// <summary>
        /// Not needed
        /// </summary>
        /// <returns>The setup.</returns>
        /// <param name="pipeline">Pipeline.</param>
        /// <param name="renderer">Renderer.</param>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // not needed
        }
    }

}
