using System;

namespace Itan.Core.CreateNewUser
{
    internal class BadArgumentInRequestException : Exception
    {
        private readonly string _createUserHandlerName;
        private readonly string _requestUserIdName;

        public BadArgumentInRequestException(string createUserHandlerName, string requestUserIdName)
        {
            _createUserHandlerName = createUserHandlerName;
            _requestUserIdName = requestUserIdName;
        }

        public override string Message => $"Exception when handling {_createUserHandlerName}, problem with {_requestUserIdName}";
    }
}