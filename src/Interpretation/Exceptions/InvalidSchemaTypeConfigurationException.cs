using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unfussiness.Components.JsonForms.Context.Exceptions
{
    public sealed class InvalidSchemaTypeConfigurationException : Exception
    {
        public InvalidSchemaTypeConfigurationException(string? message = null) : base(message)
        {
            
        }
    }
}
