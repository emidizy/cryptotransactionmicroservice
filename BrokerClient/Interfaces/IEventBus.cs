using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventPublisher.Interfaces
{
    public interface IEventBus
    {
        Task PublishEvent(string payload, string eventId);
    }
}
