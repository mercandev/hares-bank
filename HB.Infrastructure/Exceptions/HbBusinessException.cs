using System;
namespace HB.Infrastructure.Exceptions
{
    [Serializable]
    public class HbBusinessException : Exception
    {
        public HbBusinessException() : base()
        {

        }

        public HbBusinessException(string message) : base(message)
        {
        }
    }
}

