using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel;
public class ServiceTemporailyUnavailableException : Exception
{
    public ServiceTemporailyUnavailableException(TimeSpan suggestedRetry, string? message = null, Exception? innerException = null)
    : base(message, innerException)
    {
        SuggestedRetry = suggestedRetry;
    }

    public TimeSpan SuggestedRetry { get; private set; }
}
