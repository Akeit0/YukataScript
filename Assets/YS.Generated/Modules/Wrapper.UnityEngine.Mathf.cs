using Mathf = UnityEngine.Mathf;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_MathfModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Mathf));
            module.RegisterConst("PI",new Variable<float>(Mathf.PI));
            module.RegisterConst("Infinity",new Variable<float>(Mathf.Infinity));
            module.RegisterConst("NegativeInfinity",new Variable<float>(Mathf.NegativeInfinity));
            module.RegisterConst("Deg2Rad",new Variable<float>(Mathf.Deg2Rad));
            module.RegisterConst("Rad2Deg",new Variable<float>(Mathf.Rad2Deg));
            module.RegisterConst("Epsilon",new Variable<float>(Mathf.Epsilon));
            module.RegisterMethod("ClosestPowerOfTwo", (result, input1) => result.SetValue(Mathf.ClosestPowerOfTwo(input1.As<int>())));
            module.RegisterMethod("IsPowerOfTwo", (result, input1) => result.SetValue(Mathf.IsPowerOfTwo(input1.As<int>())));
            module.RegisterMethod("NextPowerOfTwo", (result, input1) => result.SetValue(Mathf.NextPowerOfTwo(input1.As<int>())));
            module.RegisterMethod("GammaToLinearSpace", (result, input1) => result.SetValue(Mathf.GammaToLinearSpace(input1.As<float>())));
            module.RegisterMethod("LinearToGammaSpace", (result, input1) => result.SetValue(Mathf.LinearToGammaSpace(input1.As<float>())));
            module.RegisterMethod("CorrelatedColorTemperatureToRGB", (result, input1) => result.SetValue(Mathf.CorrelatedColorTemperatureToRGB(input1.As<float>())));
            module.RegisterMethod("FloatToHalf", (result, input1) => result.SetValue(Mathf.FloatToHalf(input1.As<float>())));
            module.RegisterMethod("HalfToFloat", (result, input1) => result.SetValue(Mathf.HalfToFloat(input1.As<ushort>())));
            module.RegisterMethod("PerlinNoise", (result, input1, input2) => result.SetValue(Mathf.PerlinNoise(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Sin", (result, input1) => result.SetValue(Mathf.Sin(input1.As<float>())));
            module.RegisterMethod("Cos", (result, input1) => result.SetValue(Mathf.Cos(input1.As<float>())));
            module.RegisterMethod("Tan", (result, input1) => result.SetValue(Mathf.Tan(input1.As<float>())));
            module.RegisterMethod("Asin", (result, input1) => result.SetValue(Mathf.Asin(input1.As<float>())));
            module.RegisterMethod("Acos", (result, input1) => result.SetValue(Mathf.Acos(input1.As<float>())));
            module.RegisterMethod("Atan", (result, input1) => result.SetValue(Mathf.Atan(input1.As<float>())));
            module.RegisterMethod("Atan2", (result, input1, input2) => result.SetValue(Mathf.Atan2(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Sqrt", (result, input1) => result.SetValue(Mathf.Sqrt(input1.As<float>())));
            module.RegisterMethod("Abs", Types(typeof(float)),(result, input1) => result.SetValue(Mathf.Abs(input1.As<float>())));
            module.RegisterMethod("Abs", Types(typeof(int)),(result, input1) => result.SetValue(Mathf.Abs(input1.As<int>())));
            module.RegisterMethod("Min", Types(typeof(float),typeof(float)),(result, input1, input2) => result.SetValue(Mathf.Min(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Min", Types(typeof(float[])),(result, input1) => result.SetValue(Mathf.Min(input1.As<float[]>())));
            module.RegisterMethod("Min", Types(typeof(int),typeof(int)),(result, input1, input2) => result.SetValue(Mathf.Min(input1.As<int>(),input2.As<int>())));
            module.RegisterMethod("Min", Types(typeof(int[])),(result, input1) => result.SetValue(Mathf.Min(input1.As<int[]>())));
            module.RegisterMethod("Max", Types(typeof(float),typeof(float)),(result, input1, input2) => result.SetValue(Mathf.Max(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Max", Types(typeof(float[])),(result, input1) => result.SetValue(Mathf.Max(input1.As<float[]>())));
            module.RegisterMethod("Max", Types(typeof(int),typeof(int)),(result, input1, input2) => result.SetValue(Mathf.Max(input1.As<int>(),input2.As<int>())));
            module.RegisterMethod("Max", Types(typeof(int[])),(result, input1) => result.SetValue(Mathf.Max(input1.As<int[]>())));
            module.RegisterMethod("Pow", (result, input1, input2) => result.SetValue(Mathf.Pow(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Exp", (result, input1) => result.SetValue(Mathf.Exp(input1.As<float>())));
            module.RegisterMethod("Log", Types(typeof(float),typeof(float)),(result, input1, input2) => result.SetValue(Mathf.Log(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("Log", Types(typeof(float)),(result, input1) => result.SetValue(Mathf.Log(input1.As<float>())));
            module.RegisterMethod("Log10", (result, input1) => result.SetValue(Mathf.Log10(input1.As<float>())));
            module.RegisterMethod("Ceil", (result, input1) => result.SetValue(Mathf.Ceil(input1.As<float>())));
            module.RegisterMethod("Floor", (result, input1) => result.SetValue(Mathf.Floor(input1.As<float>())));
            module.RegisterMethod("Round", (result, input1) => result.SetValue(Mathf.Round(input1.As<float>())));
            module.RegisterMethod("CeilToInt", (result, input1) => result.SetValue(Mathf.CeilToInt(input1.As<float>())));
            module.RegisterMethod("FloorToInt", (result, input1) => result.SetValue(Mathf.FloorToInt(input1.As<float>())));
            module.RegisterMethod("RoundToInt", (result, input1) => result.SetValue(Mathf.RoundToInt(input1.As<float>())));
            module.RegisterMethod("Sign", (result, input1) => result.SetValue(Mathf.Sign(input1.As<float>())));
            module.RegisterMethod("Clamp", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3) => result.SetValue(Mathf.Clamp(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("Clamp", Types(typeof(int),typeof(int),typeof(int)),(result, input1, input2, input3) => result.SetValue(Mathf.Clamp(input1.As<int>(),input2.As<int>(),input3.As<int>())));
            module.RegisterMethod("Clamp01", (result, input1) => result.SetValue(Mathf.Clamp01(input1.As<float>())));
            module.RegisterMethod("Lerp", (result, input1, input2, input3) => result.SetValue(Mathf.Lerp(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("LerpUnclamped", (result, input1, input2, input3) => result.SetValue(Mathf.LerpUnclamped(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("LerpAngle", (result, input1, input2, input3) => result.SetValue(Mathf.LerpAngle(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("MoveTowards", (result, input1, input2, input3) => result.SetValue(Mathf.MoveTowards(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("MoveTowardsAngle", (result, input1, input2, input3) => result.SetValue(Mathf.MoveTowardsAngle(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("SmoothStep", (result, input1, input2, input3) => result.SetValue(Mathf.SmoothStep(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("Gamma", (result, input1, input2, input3) => result.SetValue(Mathf.Gamma(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("Approximately", (result, input1, input2) => result.SetValue(Mathf.Approximately(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("SmoothDamp", Types(typeof(float),typeof(float),typeof(float).MakeByRefType(),typeof(float),typeof(float)),(result, input1, input2, input3, input4, input5) => result.SetValue(Mathf.SmoothDamp(input1.As<float>(),input2.As<float>(),ref input3.As<float>(),input4.As<float>(),input5.As<float>())));
            module.RegisterMethod("SmoothDamp", Types(typeof(float),typeof(float),typeof(float).MakeByRefType(),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(Mathf.SmoothDamp(input1.As<float>(),input2.As<float>(),ref input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("SmoothDampAngle", Types(typeof(float),typeof(float),typeof(float).MakeByRefType(),typeof(float),typeof(float)),(result, input1, input2, input3, input4, input5) => result.SetValue(Mathf.SmoothDampAngle(input1.As<float>(),input2.As<float>(),ref input3.As<float>(),input4.As<float>(),input5.As<float>())));
            module.RegisterMethod("SmoothDampAngle", Types(typeof(float),typeof(float),typeof(float).MakeByRefType(),typeof(float)),(result, input1, input2, input3, input4) => result.SetValue(Mathf.SmoothDampAngle(input1.As<float>(),input2.As<float>(),ref input3.As<float>(),input4.As<float>())));
            module.RegisterMethod("Repeat", (result, input1, input2) => result.SetValue(Mathf.Repeat(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("PingPong", (result, input1, input2) => result.SetValue(Mathf.PingPong(input1.As<float>(),input2.As<float>())));
            module.RegisterMethod("InverseLerp", (result, input1, input2, input3) => result.SetValue(Mathf.InverseLerp(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("DeltaAngle", (result, input1, input2) => result.SetValue(Mathf.DeltaAngle(input1.As<float>(),input2.As<float>())));
            module.ClearTempMembers();
                          if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}