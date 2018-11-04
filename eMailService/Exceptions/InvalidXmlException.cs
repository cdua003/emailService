using System;
namespace eMailService.Exceptions
{
    public class InvalidXmlException : Exception
    {
        public InvalidXmlException(string message) : base(message) { }
    }
}