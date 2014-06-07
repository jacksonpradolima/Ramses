using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Ramses.Business
{
    public class BizException : ApplicationException, ISerializable
    {
        public BizException()
            : base()
        {
            // Add implementation.
        }
        public BizException(string message)
            : base(message)
        {
            // Add implementation.
        }
        public BizException(string message, Exception inner)
            : base(message, inner)
        {
            // Add implementation.
        }

        // This constructor is needed for serialization.
        protected BizException(SerializationInfo info, StreamingContext context)
        {
            // Add implementation.
        }
    }
}
