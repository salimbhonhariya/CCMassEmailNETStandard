using System;
using System.Runtime.Serialization;

namespace Email.CustomException
{
    [Serializable]
    internal class ContactRepositoryException : Exception
    {
        public ContactRepositoryException()
        {
        }

        public ContactRepositoryException(string message) : base(message)
        {
        }

        public ContactRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ContactRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}