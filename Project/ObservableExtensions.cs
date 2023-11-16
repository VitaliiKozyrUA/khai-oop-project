using System.Reactive.Linq;

namespace Project;

public static class ObservableExtensions
{
    public static IDisposable SubscribeDistinct<T>(this IObservable<T> source, Action<T> onNext)
    {
        return source.Distinct().Subscribe(onNext);
    }
    
    public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
    {
        return source.DistinctUntilChanged(EqualityComparer<T>.Default);
    }

    private static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source, IEqualityComparer<T> comparer)
    {
        return Observable.Create<T>(observer =>
        {
            var isFirst = true;
            T previous = default!;

            return source.Subscribe(
                value =>
                {
                    if (isFirst || !comparer.Equals(value, previous))
                    {
                        isFirst = false;
                        previous = value;
                        observer.OnNext(value);
                    }
                },
                observer.OnError,
                observer.OnCompleted
            );
        });
    }
}