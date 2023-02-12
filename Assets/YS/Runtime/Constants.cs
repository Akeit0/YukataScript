using System;

namespace YS {
    public static class Constants {
        public static readonly Variable<int> IntZero = new ();
        public static readonly Variable<float> FloatZero = new ();
        public static readonly Variable<double> DoubleZero = new ();
        public static readonly Variable<object> Null = new ();
        public static readonly Variable<bool> True = new (true);
        public static readonly Variable<bool> False = new (false);
    }
}