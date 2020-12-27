using System;
using Itan.Common;

namespace Itan.Wrappers
{
    public class FeedReaderWrapperParseStringException : ItanException
    {
        public FeedReaderWrapperParseStringException(Exception exception)
            : base(nameof(FeedReaderWrapperParseStringException), exception)
        {
        }

        public override string Message => "There was a problem parsing string into CodeHollow Feed item";
    }
}