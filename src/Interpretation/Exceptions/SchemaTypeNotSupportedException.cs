using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbyss.Components.JsonForms.Interpretation.Exceptions
{
    public sealed class SchemaTypeNotSupportedException : Exception
    {
        public SchemaTypeNotSupportedException(string? message = null) : base(message)
        {
            
        }
    }
}
