using System;
using Itan.Common;

namespace Itan.Functions.Workers.Exceptions
{
    public class NewsWriterInsertNewsLinkException : ItanException
    {
        public NewsWriterInsertNewsLinkException(Exception exception):
            base(nameof(NewsWriterInsertNewsLinkException), exception)
        {
        }

        public override string Message => "There was problem while inserting news link into mssql database";
    }
}