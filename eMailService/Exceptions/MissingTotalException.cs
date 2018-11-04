using System;

namespace eMailService.Exceptions
{
    public class MissingTotalException : Exception
    {
        public MissingTotalException(string message) : base(message) { }
    }
}