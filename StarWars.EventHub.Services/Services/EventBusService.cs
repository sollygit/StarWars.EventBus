using StarWars.EventHub.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.EventHub.Services.Services
{
    public interface IEventBusService<T> : IObservable<T>
    {
        Task Process(T request);
    }

    public class EventBusService : IEventBusService<OrderRequest>
    {
        readonly List<IObserver<OrderRequest>> subjects;

        public EventBusService()
        {
            subjects = new List<IObserver<OrderRequest>>();
        }

        public IDisposable Subscribe(IObserver<OrderRequest> subject)
        {
            if (!subjects.Contains(subject))
            {
                subjects.Add(subject);
            }
            return new Unsubscriber(subjects, subject);
        }

        public Task Process(OrderRequest request)
        {
            foreach (var subject in subjects)
            {
                subject.OnNext(request);
            }

            return Task.CompletedTask;
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<OrderRequest>> _subjects;
            private readonly IObserver<OrderRequest> _subject;

            public Unsubscriber(List<IObserver<OrderRequest>> subjects, IObserver<OrderRequest> subject)
            {
                _subjects = subjects;
                _subject = subject;
            }

            public void Dispose()
            {
                if (_subject != null)
                {
                    _subjects.Remove(_subject);
                }
            }
        }
    }
}
