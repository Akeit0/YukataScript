using SpriteRenderer = UnityEngine.SpriteRenderer;
using Sprite = UnityEngine.Sprite;
using SpriteDrawMode = UnityEngine.SpriteDrawMode;
using Vector2 = UnityEngine.Vector2;
using SpriteTileMode = UnityEngine.SpriteTileMode;
using Color = UnityEngine.Color;
using SpriteMaskInteraction = UnityEngine.SpriteMaskInteraction;
using SpriteSortPoint = UnityEngine.SpriteSortPoint;
using UnityAction = UnityEngine.Events.UnityAction<UnityEngine.SpriteRenderer>;

namespace YS.Generated {
    public static partial class Wrapper {
        public static TypeModule Create_UnityEngine_SpriteRendererModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(SpriteRenderer));
            module.RegisterConstructor(result => result.SetValue( new SpriteRenderer()));
            module.RegisterPropertyGetter("sprite", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().sprite));
            module.RegisterPropertySetter("sprite", (input1, input2) => input1.As<SpriteRenderer>().sprite=input2.As<Sprite>());
            module.RegisterPropertyGetter("drawMode", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().drawMode));
            module.RegisterPropertySetter("drawMode", (input1, input2) => input1.As<SpriteRenderer>().drawMode=input2.As<SpriteDrawMode>());
            module.RegisterPropertyGetter("size", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().size));
            module.RegisterPropertySetter("size", (input1, input2) => input1.As<SpriteRenderer>().size=input2.As<Vector2>());
            module.RegisterPropertyGetter("adaptiveModeThreshold", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().adaptiveModeThreshold));
            module.RegisterPropertySetter("adaptiveModeThreshold", (input1, input2) => input1.As<SpriteRenderer>().adaptiveModeThreshold=input2.As<float>());
            module.RegisterPropertyGetter("tileMode", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().tileMode));
            module.RegisterPropertySetter("tileMode", (input1, input2) => input1.As<SpriteRenderer>().tileMode=input2.As<SpriteTileMode>());
            module.RegisterPropertyGetter("color", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().color));
            module.RegisterPropertySetter("color", (input1, input2) => input1.As<SpriteRenderer>().color=input2.As<Color>());
            module.RegisterPropertyGetter("maskInteraction", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().maskInteraction));
            module.RegisterPropertySetter("maskInteraction", (input1, input2) => input1.As<SpriteRenderer>().maskInteraction=input2.As<SpriteMaskInteraction>());
            module.RegisterPropertyGetter("flipX", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().flipX));
            module.RegisterPropertySetter("flipX", (input1, input2) => input1.As<SpriteRenderer>().flipX=input2.As<bool>());
            module.RegisterPropertyGetter("flipY", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().flipY));
            module.RegisterPropertySetter("flipY", (input1, input2) => input1.As<SpriteRenderer>().flipY=input2.As<bool>());
            module.RegisterPropertyGetter("spriteSortPoint", (result, input1) => result.SetValue(input1.As<SpriteRenderer>().spriteSortPoint));
            module.RegisterPropertySetter("spriteSortPoint", (input1, input2) => input1.As<SpriteRenderer>().spriteSortPoint=input2.As<SpriteSortPoint>());
            module.RegisterMethod("RegisterSpriteChangeCallback", (input1, input2) => input1.As<SpriteRenderer>().RegisterSpriteChangeCallback(input2.As<UnityAction>()));
            module.RegisterMethod("UnregisterSpriteChangeCallback", (input1, input2) => input1.As<SpriteRenderer>().UnregisterSpriteChangeCallback(input2.As<UnityAction>()));
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}