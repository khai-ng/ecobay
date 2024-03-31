using MediatR;
using System.Text.Json.Serialization;

namespace Core.Events
{
    //public interface IIntergrationEvent: INotification
    //{
    //    Guid Id => Guid.NewGuid();
    //}

    public class IntergrationEvent : INotification
    {
        public IntergrationEvent()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private init; }
    }
}