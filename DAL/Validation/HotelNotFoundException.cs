using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Validation
{
    [Serializable]
    public class HotelNotFoundException:Exception
    {
        public HotelNotFoundException()
        {

        }
        public HotelNotFoundException(string message) : base(message)
        {

        }
        public HotelNotFoundException(string message, string paramName) : base(message)
        {

        }

        public HotelNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected HotelNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
