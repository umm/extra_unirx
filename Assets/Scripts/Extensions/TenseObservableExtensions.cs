using System;
using UniRx;

namespace ExtraUniRx
{
    public static class TenseObservableExtensions
    {
        public static IObservable<T> When<T>(this IObservable<Tuple<T, Tense>> self, Tense tense)
        {
            return self.Where(x => x.Item2 == tense).Select(x => x.Item1);
        }

        public static IObservable<Unit> When(this IObservable<Tense> self, Tense tense)
        {
            return self.Select(x => new Tuple<Unit, Tense>(Unit.Default, x)).When(tense);
        }

        public static IObservable<T> WhenWill<T>(this IObservable<Tuple<T, Tense>> self)
        {
            return self.When(Tense.Will);
        }

        public static IObservable<Unit> WhenWill(this IObservable<Tense> self)
        {
            return self.When(Tense.Will);
        }

        public static IObservable<T> WhenDo<T>(this IObservable<Tuple<T, Tense>> self)
        {
            return self.When(Tense.Do);
        }

        public static IObservable<Unit> WhenDo(this IObservable<Tense> self)
        {
            return self.When(Tense.Do);
        }

        public static IObservable<T> WhenDid<T>(this IObservable<Tuple<T, Tense>> self)
        {
            return self.When(Tense.Did);
        }

        public static IObservable<Unit> WhenDid(this IObservable<Tense> self)
        {
            return self.When(Tense.Did);
        }
    }
}