using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblivionBSAUncompressor
{

    [Serializable]
    public class BSAException : Exception
    {
        public BSAException() { }
        public BSAException(string message) : base(message) { }
        public BSAException(string message, Exception inner) : base(message, inner) { }
        protected BSAException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
