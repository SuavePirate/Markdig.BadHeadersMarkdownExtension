using System;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
namespace Markdig.BadHeaders
{
    /// <summary>
    /// Bad heading block parser. Does the same thing as the header parser, but doesn't require a space.
    /// Using a private class to ensure all markdown logic is contained within this service.
    /// </summary>
    public class BadHeadingBlockParser : HeadingBlockParser
    {
        private readonly char _headChar;
        public BadHeadingBlockParser(char headChar)
        {
            _headChar = headChar;
        }
        public override BlockState TryOpen(BlockProcessor processor)
        {
            // If we are in a CodeIndent, early exit
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // 4.2 ATX headings
            // An ATX heading consists of a string of characters, parsed as inline content, 
            // between an opening sequence of 1–6 unescaped # characters and an optional 
            // closing sequence of any number of unescaped # characters. The opening sequence 
            // of # characters must be followed by a space or by the end of line. The optional
            // closing sequence of #s must be preceded by a space and may be followed by spaces
            // only. The opening # character may be indented 0-3 spaces. The raw contents of 
            // the heading are stripped of leading and trailing spaces before being parsed as 
            // inline content. The heading level is equal to the number of # characters in the 
            // opening sequence.

            // We are not doing this ^^ we don't have the spaces... so we need to handle that adjusted logic here
            var column = processor.Column;
            var line = processor.Line;
            var sourcePosition = line.Start;
            var c = line.CurrentChar;
            var matchingChar = c;

            int leadingCount = 0;

            // get how many of the headChar we have and limit to 6 (h6 is the last handled header)
            while (c == _headChar && leadingCount <= 6)
            {
                if (c != matchingChar)
                {
                    break;
                }
                c = line.NextChar();
                leadingCount++;
            }

            // A space is NOT required after leading #
            if (leadingCount > 0 && leadingCount <= 6)
            {
                // Move to the content
                var headingBlock = new HeadingBlock(this)
                {
                    HeaderChar = matchingChar,
                    Level = leadingCount,
                    Column = column,
                    Span = { Start = sourcePosition }
                };
                processor.NewBlocks.Push(headingBlock);
                processor.GoToColumn(column + leadingCount); // no +1 - skip the space

                // Gives a chance to parse attributes
                if (TryParseAttributes != null)
                {
                    TryParseAttributes(processor, ref processor.Line, headingBlock);
                }

                // The optional closing sequence of #s must not be preceded by a space and may be followed by spaces only.
                int endState = 0;
                int countClosingTags = 0;
                for (int i = processor.Line.End; i >= processor.Line.Start; i--)  // Go up to Start in order to match the no space after the first ###
                {
                    c = processor.Line.Text[i];
                    if (endState == 0)
                    {
                        if (c.IsSpaceOrTab())
                        {
                            continue;
                        }
                        endState = 1;
                    }
                    if (endState == 1)
                    {
                        if (c == matchingChar)
                        {
                            countClosingTags++;
                            continue;
                        }

                        if (countClosingTags > 0)
                        {
                            if (c.IsSpaceOrTab())
                            {
                                processor.Line.End = i - 1;
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Setup the source end position of this element
                headingBlock.Span.End = processor.Line.End;

                // We expect a single line, so don't continue
                return BlockState.Break;
            }

            // Else we don't have an header
            return BlockState.None;
        }
    }
}
