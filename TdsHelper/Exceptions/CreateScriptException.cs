using System;

namespace TdsHelper.Exceptions
{
    public class CreateScriptException : Exception
    {
        public CreateScriptException(string message)
            :base(message)
        {
            
        }
    }
}