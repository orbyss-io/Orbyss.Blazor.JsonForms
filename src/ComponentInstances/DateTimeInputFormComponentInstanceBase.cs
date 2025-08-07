using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbyss.Blazor.JsonForms.ComponentInstances
{
    public abstract class DateTimeInputFormComponentInstanceBase<TDate> : DateOnlyInputFormComponentInstanceBase<TDate>
    {
        public DateTime? MinimumTime { get; set; }

        public DateTime? MaximumTime { get; set; }

        public int? TimeStep { get; set; }

        protected sealed override IDictionary<string, object?> GetDateInputParameter()
        {
            var result = GetDateTimeInputParameter();

            result[nameof(MinimumTime)] = MinimumTime;
            result[nameof(MaximumTime)] = MaximumTime;
            result[nameof(TimeStep)] = TimeStep;

            return result;
        }

        protected virtual IDictionary<string, object?> GetDateTimeInputParameter()
        {
            return new Dictionary<string, object?>();
        }
    }
}
