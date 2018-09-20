using System;

namespace ExtraUniRx
{
    public static class TenseObserverExtensions
    {
        public static void On<T>(this IObserver<Tuple<T, Tense>> self, T value, Tense tense)
        {
            self.OnNext(new Tuple<T, Tense>(value, tense));
        }

        public static void On(this IObserver<Tense> self, Tense tense)
        {
            self.OnNext(tense);
        }

        public static void Will<T>(this IObserver<Tuple<T, Tense>> self, T value)
        {
            self.On(value, Tense.Will);
        }

        public static void Will(this IObserver<Tense> self)
        {
            self.On(Tense.Will);
        }

        public static void Do<T>(this IObserver<Tuple<T, Tense>> self, T value)
        {
            self.On(value, Tense.Do);
        }

        public static void Do(this IObserver<Tense> self)
        {
            self.On(Tense.Do);
        }

        public static void Did<T>(this IObserver<Tuple<T, Tense>> self, T value)
        {
            self.On(value, Tense.Did);
        }

        public static void Did(this IObserver<Tense> self)
        {
            self.On(Tense.Did);
        }
    }
}