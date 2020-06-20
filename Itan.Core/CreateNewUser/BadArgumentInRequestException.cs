using System;

namespace Itan.Core.CreateNewUser
{
    internal class BadArgumentInRequestException : Exception
    {
        private readonly string createUserHandlerName;
        private readonly string requestUserIdName;

        public BadArgumentInRequestException(string createUserHandlerName, string requestUserIdName)
        {
            this.createUserHandlerName = createUserHandlerName;
            this.requestUserIdName = requestUserIdName;
        }

        public override string Message => $"Exception when handling {this.createUserHandlerName}, problem with {this.requestUserIdName}";
    }
}