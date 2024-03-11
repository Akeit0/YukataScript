#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif
using SpriteRenderer = UnityEngine.SpriteRenderer;
using _SpriteRenderer = YS.Variable<UnityEngine.SpriteRenderer>;
using Sprite = UnityEngine.Sprite;
using _Sprite = YS.Variable<UnityEngine.Sprite>;
using SpriteDrawMode = UnityEngine.SpriteDrawMode;
using _SpriteDrawMode = YS.Variable<UnityEngine.SpriteDrawMode>;
using Vector2 = UnityEngine.Vector2;
using _Vector2 = YS.Variable<UnityEngine.Vector2>;
using _float = YS.Variable<float>;
using SpriteTileMode = UnityEngine.SpriteTileMode;
using _SpriteTileMode = YS.Variable<UnityEngine.SpriteTileMode>;
using Color = UnityEngine.Color;
using _Color = YS.Variable<UnityEngine.Color>;
using SpriteMaskInteraction = UnityEngine.SpriteMaskInteraction;
using _SpriteMaskInteraction = YS.Variable<UnityEngine.SpriteMaskInteraction>;
using _bool = YS.Variable<bool>;
using SpriteSortPoint = UnityEngine.SpriteSortPoint;
using _SpriteSortPoint = YS.Variable<UnityEngine.SpriteSortPoint>;
using UnityAction = UnityEngine.Events.UnityAction<UnityEngine.SpriteRenderer>;
using _UnityAction = YS.Variable<UnityEngine.Events.UnityAction<UnityEngine.SpriteRenderer>>;

using static System.Runtime.CompilerServices.Unsafe;
using __V = YS.Variable;
namespace YS.Generated {
    public static partial  class Wrapper {
        static void UnityEngine_SpriteRenderer_ctor(__V res) =>  As<_SpriteRenderer>(res).value = new SpriteRenderer();
       static void UnityEngine_SpriteRenderer_get_sprite(__V res, __V  i1) => As<_Sprite>(res).value=As<_SpriteRenderer>(i1).value.sprite;
       static void UnityEngine_SpriteRenderer_set_sprite(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.sprite = As<_Sprite>(i2).value;
       static void UnityEngine_SpriteRenderer_get_drawMode(__V res, __V  i1) => As<_SpriteDrawMode>(res).value=As<_SpriteRenderer>(i1).value.drawMode;
       static void UnityEngine_SpriteRenderer_set_drawMode(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.drawMode = As<_SpriteDrawMode>(i2).value;
       static void UnityEngine_SpriteRenderer_get_size(__V res, __V  i1) => As<_Vector2>(res).value=As<_SpriteRenderer>(i1).value.size;
       static void UnityEngine_SpriteRenderer_set_size(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.size = As<_Vector2>(i2).value;
       static void UnityEngine_SpriteRenderer_get_adaptiveModeThreshold(__V res, __V  i1) => As<_float>(res).value=As<_SpriteRenderer>(i1).value.adaptiveModeThreshold;
       static void UnityEngine_SpriteRenderer_set_adaptiveModeThreshold(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.adaptiveModeThreshold = As<_float>(i2).value;
       static void UnityEngine_SpriteRenderer_get_tileMode(__V res, __V  i1) => As<_SpriteTileMode>(res).value=As<_SpriteRenderer>(i1).value.tileMode;
       static void UnityEngine_SpriteRenderer_set_tileMode(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.tileMode = As<_SpriteTileMode>(i2).value;
       static void UnityEngine_SpriteRenderer_get_color(__V res, __V  i1) => As<_Color>(res).value=As<_SpriteRenderer>(i1).value.color;
       static void UnityEngine_SpriteRenderer_set_color(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.color = As<_Color>(i2).value;
       static void UnityEngine_SpriteRenderer_get_maskInteraction(__V res, __V  i1) => As<_SpriteMaskInteraction>(res).value=As<_SpriteRenderer>(i1).value.maskInteraction;
       static void UnityEngine_SpriteRenderer_set_maskInteraction(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.maskInteraction = As<_SpriteMaskInteraction>(i2).value;
       static void UnityEngine_SpriteRenderer_get_flipX(__V res, __V  i1) => As<_bool>(res).value=As<_SpriteRenderer>(i1).value.flipX;
       static void UnityEngine_SpriteRenderer_set_flipX(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.flipX = As<_bool>(i2).value;
       static void UnityEngine_SpriteRenderer_get_flipY(__V res, __V  i1) => As<_bool>(res).value=As<_SpriteRenderer>(i1).value.flipY;
       static void UnityEngine_SpriteRenderer_set_flipY(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.flipY = As<_bool>(i2).value;
       static void UnityEngine_SpriteRenderer_get_spriteSortPoint(__V res, __V  i1) => As<_SpriteSortPoint>(res).value=As<_SpriteRenderer>(i1).value.spriteSortPoint;
       static void UnityEngine_SpriteRenderer_set_spriteSortPoint(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.spriteSortPoint = As<_SpriteSortPoint>(i2).value;
       static void UnityEngine_SpriteRenderer_RegisterSpriteChangeCallback(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.RegisterSpriteChangeCallback(As<_UnityAction>(i2).value);
       static void UnityEngine_SpriteRenderer_UnregisterSpriteChangeCallback(__V  i1, __V  i2) => As<_SpriteRenderer>(i1).value.UnregisterSpriteChangeCallback(As<_UnityAction>(i2).value);
#if !AOT

        static TypeModule Create_UnityEngine_SpriteRendererModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(SpriteRenderer));
            module.RegisterConstructor(UnityEngine_SpriteRenderer_ctor);
            module.RegisterPropertyGetter("sprite", UnityEngine_SpriteRenderer_get_sprite);
            module.RegisterPropertySetter("sprite", UnityEngine_SpriteRenderer_set_sprite);
            module.RegisterPropertyGetter("drawMode", UnityEngine_SpriteRenderer_get_drawMode);
            module.RegisterPropertySetter("drawMode", UnityEngine_SpriteRenderer_set_drawMode);
            module.RegisterPropertyGetter("size", UnityEngine_SpriteRenderer_get_size);
            module.RegisterPropertySetter("size", UnityEngine_SpriteRenderer_set_size);
            module.RegisterPropertyGetter("adaptiveModeThreshold", UnityEngine_SpriteRenderer_get_adaptiveModeThreshold);
            module.RegisterPropertySetter("adaptiveModeThreshold", UnityEngine_SpriteRenderer_set_adaptiveModeThreshold);
            module.RegisterPropertyGetter("tileMode", UnityEngine_SpriteRenderer_get_tileMode);
            module.RegisterPropertySetter("tileMode", UnityEngine_SpriteRenderer_set_tileMode);
            module.RegisterPropertyGetter("color", UnityEngine_SpriteRenderer_get_color);
            module.RegisterPropertySetter("color", UnityEngine_SpriteRenderer_set_color);
            module.RegisterPropertyGetter("maskInteraction", UnityEngine_SpriteRenderer_get_maskInteraction);
            module.RegisterPropertySetter("maskInteraction", UnityEngine_SpriteRenderer_set_maskInteraction);
            module.RegisterPropertyGetter("flipX", UnityEngine_SpriteRenderer_get_flipX);
            module.RegisterPropertySetter("flipX", UnityEngine_SpriteRenderer_set_flipX);
            module.RegisterPropertyGetter("flipY", UnityEngine_SpriteRenderer_get_flipY);
            module.RegisterPropertySetter("flipY", UnityEngine_SpriteRenderer_set_flipY);
            module.RegisterPropertyGetter("spriteSortPoint", UnityEngine_SpriteRenderer_get_spriteSortPoint);
            module.RegisterPropertySetter("spriteSortPoint", UnityEngine_SpriteRenderer_set_spriteSortPoint);
            module.RegisterMethod2("RegisterSpriteChangeCallback",UnityEngine_SpriteRenderer_RegisterSpriteChangeCallback);
            module.RegisterMethod2("UnregisterSpriteChangeCallback",UnityEngine_SpriteRenderer_UnregisterSpriteChangeCallback);
#else
        static unsafe TypeModule Create_UnityEngine_SpriteRendererModule(global::System.Type baseType=null) {
            var module = new TypeModule(typeof(SpriteRenderer));
            module.RegisterConstructor(&UnityEngine_SpriteRenderer_ctor);
            module.RegisterPropertyGetter("sprite", &UnityEngine_SpriteRenderer_get_sprite);
            module.RegisterPropertySetter("sprite",& UnityEngine_SpriteRenderer_set_sprite);
            module.RegisterPropertyGetter("drawMode", &UnityEngine_SpriteRenderer_get_drawMode);
            module.RegisterPropertySetter("drawMode",& UnityEngine_SpriteRenderer_set_drawMode);
            module.RegisterPropertyGetter("size", &UnityEngine_SpriteRenderer_get_size);
            module.RegisterPropertySetter("size",& UnityEngine_SpriteRenderer_set_size);
            module.RegisterPropertyGetter("adaptiveModeThreshold", &UnityEngine_SpriteRenderer_get_adaptiveModeThreshold);
            module.RegisterPropertySetter("adaptiveModeThreshold",& UnityEngine_SpriteRenderer_set_adaptiveModeThreshold);
            module.RegisterPropertyGetter("tileMode", &UnityEngine_SpriteRenderer_get_tileMode);
            module.RegisterPropertySetter("tileMode",& UnityEngine_SpriteRenderer_set_tileMode);
            module.RegisterPropertyGetter("color", &UnityEngine_SpriteRenderer_get_color);
            module.RegisterPropertySetter("color",& UnityEngine_SpriteRenderer_set_color);
            module.RegisterPropertyGetter("maskInteraction", &UnityEngine_SpriteRenderer_get_maskInteraction);
            module.RegisterPropertySetter("maskInteraction",& UnityEngine_SpriteRenderer_set_maskInteraction);
            module.RegisterPropertyGetter("flipX", &UnityEngine_SpriteRenderer_get_flipX);
            module.RegisterPropertySetter("flipX",& UnityEngine_SpriteRenderer_set_flipX);
            module.RegisterPropertyGetter("flipY", &UnityEngine_SpriteRenderer_get_flipY);
            module.RegisterPropertySetter("flipY",& UnityEngine_SpriteRenderer_set_flipY);
            module.RegisterPropertyGetter("spriteSortPoint", &UnityEngine_SpriteRenderer_get_spriteSortPoint);
            module.RegisterPropertySetter("spriteSortPoint",& UnityEngine_SpriteRenderer_set_spriteSortPoint);
            module.RegisterMethod2("RegisterSpriteChangeCallback",&UnityEngine_SpriteRenderer_RegisterSpriteChangeCallback);
            module.RegisterMethod2("UnregisterSpriteChangeCallback",&UnityEngine_SpriteRenderer_UnregisterSpriteChangeCallback);
#endif
            module.ClearTempMembers();
            if (baseType != null) module.BaseModule = baseType.GetModule();
            return module;
        }
    }
}