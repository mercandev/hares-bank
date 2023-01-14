using System;
namespace HB.Service.Engine
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

