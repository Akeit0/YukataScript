using Color = UnityEngine.Color;
using IFormatProvider = System.IFormatProvider;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_ColorModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(Color));
            module.RegisterConstructor(typeof(float),typeof(float),typeof(float),typeof(float),(result, input1, input2, input3, input4) => result.SetValue( new Color(input1.As<float>(),input2.As<float>(),input3.As<float>(),input4.As<float>())));
            module.RegisterConstructor(typeof(float),typeof(float),typeof(float),(result, input1, input2, input3) => result.SetValue( new Color(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterFieldGetter("r", (result, input1) => result.SetValue(input1.As<Color>().r));
            module.RegisterFieldSetter("r", (input1, input2) => input1.As<Color>().r=input2.As<float>());
            module.RegisterFieldGetter("g", (result, input1) => result.SetValue(input1.As<Color>().g));
            module.RegisterFieldSetter("g", (input1, input2) => input1.As<Color>().g=input2.As<float>());
            module.RegisterFieldGetter("b", (result, input1) => result.SetValue(input1.As<Color>().b));
            module.RegisterFieldSetter("b", (input1, input2) => input1.As<Color>().b=input2.As<float>());
            module.RegisterFieldGetter("a", (result, input1) => result.SetValue(input1.As<Color>().a));
            module.RegisterFieldSetter("a", (input1, input2) => input1.As<Color>().a=input2.As<float>());
            module.RegisterPropertyGetter("red", result => result.SetValue(Color.red));
            module.RegisterPropertyGetter("green", result => result.SetValue(Color.green));
            module.RegisterPropertyGetter("blue", result => result.SetValue(Color.blue));
            module.RegisterPropertyGetter("white", result => result.SetValue(Color.white));
            module.RegisterPropertyGetter("black", result => result.SetValue(Color.black));
            module.RegisterPropertyGetter("yellow", result => result.SetValue(Color.yellow));
            module.RegisterPropertyGetter("cyan", result => result.SetValue(Color.cyan));
            module.RegisterPropertyGetter("magenta", result => result.SetValue(Color.magenta));
            module.RegisterPropertyGetter("gray", result => result.SetValue(Color.gray));
            module.RegisterPropertyGetter("grey", result => result.SetValue(Color.grey));
            module.RegisterPropertyGetter("clear", result => result.SetValue(Color.clear));
            module.RegisterPropertyGetter("grayscale", (result, input1) => result.SetValue(input1.As<Color>().grayscale));
            module.RegisterPropertyGetter("linear", (result, input1) => result.SetValue(input1.As<Color>().linear));
            module.RegisterPropertyGetter("gamma", (result, input1) => result.SetValue(input1.As<Color>().gamma));
            module.RegisterPropertyGetter("maxColorComponent", (result, input1) => result.SetValue(input1.As<Color>().maxColorComponent));
            module.RegisterMethod("ToString", Types(),(result, input1) => result.SetValue(input1.As<Color>().ToString()));
            module.RegisterMethod("ToString", Types(typeof(string)),(result, input1, input2) => result.SetValue(input1.As<Color>().ToString(input2.As<string>())));
            module.RegisterMethod("ToString", Types(typeof(string),typeof(IFormatProvider)),(result, input1, input2, input3) => result.SetValue(input1.As<Color>().ToString(input2.As<string>(),input3.As<IFormatProvider>())));
            module.RegisterMethod("GetHashCode", (result, input1) => result.SetValue(input1.As<Color>().GetHashCode()));
            module.RegisterMethod("Equals", Types(typeof(object)),(result, input1, input2) => result.SetValue(input1.As<Color>().Equals(input2.As<object>())));
            module.RegisterMethod("Equals", Types(typeof(Color)),(result, input1, input2) => result.SetValue(input1.As<Color>().Equals(input2.As<Color>())));
            module.RegisterMethod("op_Addition", (result, input1, input2) => result.SetValue(input1.As<Color>()+input2.As<Color>()));
            module.RegisterMethod("op_Subtraction", (result, input1, input2) => result.SetValue(input1.As<Color>()-input2.As<Color>()));
            module.RegisterMethod("op_Multiply", Types(typeof(Color),typeof(Color)),(result, input1, input2) => result.SetValue(input1.As<Color>()*input2.As<Color>()));
            module.RegisterMethod("op_Multiply", Types(typeof(Color),typeof(float)),(result, input1, input2) => result.SetValue(input1.As<Color>()*input2.As<float>()));
            module.RegisterMethod("op_Multiply", Types(typeof(float),typeof(Color)),(result, input1, input2) => result.SetValue(input1.As<float>()*input2.As<Color>()));
            module.RegisterMethod("op_Division", (result, input1, input2) => result.SetValue(input1.As<Color>()/input2.As<float>()));
            module.RegisterMethod("op_Equality", (result, input1, input2) => result.SetValue(input1.As<Color>()==input2.As<Color>()));
            module.RegisterMethod("op_Inequality", (result, input1, input2) => result.SetValue(input1.As<Color>()!=input2.As<Color>()));
            module.RegisterMethod("Lerp", (result, input1, input2, input3) => result.SetValue(Color.Lerp(input1.As<Color>(),input2.As<Color>(),input3.As<float>())));
            module.RegisterMethod("LerpUnclamped", (result, input1, input2, input3) => result.SetValue(Color.LerpUnclamped(input1.As<Color>(),input2.As<Color>(),input3.As<float>())));
            module.RegisterMethod("get_Item", (result, input1, input2) => result.SetValue(input1.As<Color>()[input2.As<int>()]));
            module.RegisterMethod("set_Item", (input1, input2, input3)  =>  input1.As<Color>()[input2.As<int>()]=input3.As<float>());
            module.RegisterMethod("RGBToHSV", (input1, input2, input3, input4)  => Color.RGBToHSV(input1.As<Color>(),out input2.As<float>(),out input3.As<float>(),out input4.As<float>()));
            module.RegisterMethod("HSVToRGB", Types(typeof(float),typeof(float),typeof(float)),(result, input1, input2, input3) => result.SetValue(Color.HSVToRGB(input1.As<float>(),input2.As<float>(),input3.As<float>())));
            module.RegisterMethod("HSVToRGB", Types(typeof(float),typeof(float),typeof(float),typeof(bool)),(result, input1, input2, input3, input4) => result.SetValue(Color.HSVToRGB(input1.As<float>(),input2.As<float>(),input3.As<float>(),input4.As<bool>())));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}