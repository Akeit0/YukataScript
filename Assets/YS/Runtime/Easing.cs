
using System;
using static UnityEngine.Mathf;
namespace YS {
	public enum EasingType
    {
        Linear=0,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
    }
    public static class Easing {

	    public static float Ease(float x, EasingType easingType) {
		    return easingType switch {
			    EasingType.Linear => EaseLinear(x),
			    EasingType.InSine => EaseInSine(x),
			    EasingType.OutSine => EaseOutSine(x),
			    EasingType.InOutSine => EaseInOutSine(x),
			    EasingType.InQuad => EaseInQuad(x),
			    EasingType.OutQuad => EaseOutQuad(x),
			    EasingType.InOutQuad => EaseInOutQuad(x),
			    EasingType.InCubic => EaseInCubic(x),
			    EasingType.OutCubic => EaseOutCubic(x),
			    EasingType.InOutCubic => EaseInOutCubic(x),
			    EasingType.InQuart => EaseInQuart(x),
			    EasingType.OutQuart => EaseOutQuart(x),
			    EasingType.InOutQuart => EaseInOutQuart(x),
			    EasingType.InQuint => EaseInQuint(x),
			    EasingType.OutQuint => EaseOutQuint(x),
			    EasingType.InOutQuint => EaseInOutQuint(x),
			    EasingType.InExpo => EaseInExpo(x),
			    EasingType.OutExpo => EaseOutExpo(x),
			    EasingType.InOutExpo => EaseInOutExpo(x),
			    EasingType.InCirc => EaseInCirc(x),
			    EasingType.OutCirc => EaseOutCirc(x),
			    EasingType.InOutCirc => EaseInOutCirc(x),
			    EasingType.InElastic => EaseInElastic(x),
			    EasingType.OutElastic => EaseOutElastic(x),
			    EasingType.InOutElastic => EaseInOutElastic(x),
			    EasingType.InBack => EaseInBack(x),
			    EasingType.OutBack => EaseOutBack(x),
			    EasingType.InOutBack => EaseInOutBack(x),
			    EasingType.InBounce => EaseInBounce(x),
			    EasingType.OutBounce => EaseOutBounce(x),
			    EasingType.InOutBounce => EaseInOutBounce(x),
			    _ => throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null)
		    };
	    }
	    
	    public static float EaseLinear(float x) => x;
	    
	    public static float EaseInSine(float x)=> 1 - Cos((x * PI) / 2);
	    public static float EaseOutSine(float x)=>  Sin((x * PI) / 2);
	    public static float EaseInOutSine(float x)=>  -(Cos(PI * x) - 1) / 2;

	 

		public static float EaseInQuad(float x) => x * x;

		public static float EaseOutQuad(float x) => x * (2f-x);

		public static float EaseInOutQuad(float x) => x < 0.5 ? 2 * x * x : 1 - EaseInQuad(- x + 1) *2;

		

		public static float EaseInCubic(float x) => x * x * x;

		public static float EaseOutCubic(float x) => 1 - EaseInCubic(1-x);

		public static float EaseInOutCubic(float x)=>x < 0.5 ? 4 * EaseInCubic(x) : 1 - EaseInCubic(-2 * x + 2) / 2;

		

		public static float EaseInQuart(float x) => x * x * x * x;

		public static float EaseOutQuart(float x) => 1-EaseInQuart(1-x);

		public static float EaseInOutQuart(float x) => x < 0.5 ? 8 * EaseInQuart(x) : 1 - EaseInQuart(-2 * x + 2) / 2;

		

		public static float EaseInQuint(float x) => x * x * x * x *x;
		public static float EaseOutQuint(float x) => 1-EaseInQuint(1-x);

		public static float EaseInOutQuint(float x) => x < 0.5 ? 16 * EaseInQuint(x) : 1 - EaseInQuint(-2 * x + 2) / 2;

		public static float EaseInExpo(float x) => x == 0 ? 0 : Pow(2, 10 * x - 10);

		public static float EaseOutExpo(float x) => x == 1 ? 1 : 1 - Pow(2, -10 * x);

		public static float EaseInOutExpo(float x) => x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? Pow(2, 20 * x - 10) / 2 : (2 - Pow(2, -20 * x + 10)) / 2;
		public static float EaseInCirc(float x) => 1 - Sqrt(1 -x*x);
		public static float EaseOutCirc(float x) => Sqrt(1 - (x-1)*(x-1));
		public static float EaseInOutCirc(float x) =>  x < 0.5 ? (1 - Sqrt(1 - 4*x*x)) / 2 : (Sqrt(1 - (-2 * x + 2)*(-2 * x + 2)) + 1) / 2;
		public static float EaseInBack(float x) => 2.70158f * x * x * x - 1.70158f * x * x;
		
		public static float EaseOutBack(float x)=> 1 + 2.70158f * EaseInCubic(x - 1) + 1.70158f  * EaseInQuad(x - 1);
		
		public static float EaseInOutBack(float x)  {
			const float c = 2.5949f;
			return x < 0.5
				? (4 * x*x* ((c + 1) * 2 * x - c)) / 2
				: (EaseInQuad(2 * x - 2) * ((c + 1) * (x * 2 - 2) + c) + 2) / 2;
		}

		public static float EaseInElastic(float x) => x == 0 ? 0 : x == 1 ? 1 : -Pow(2, 10 * x - 10) * Sin((x * 10 - 10.75f) * 2 * PI / 3);
		
		public static float EaseOutElastic(float x)  => x == 0 ? 0 : x == 1 ? 1 : Pow(2, -10 * x) * Sin((x * 10 - 0.75f) * 2 * PI / 3) + 1;
		
		public static float EaseInOutElastic(float x) 
			=> x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? -(Pow(2, 20 * x - 10) * Sin((20 * x - 11.125f) * (2 * PI / 4.5f))) / 2 : Pow(2, -20 * x + 10) * Sin((20 * x - 11.125f) * (2 * PI / 4.5f)) / 2 + 1;

		public static float EaseInBounce(float x) => 1 - EaseOutBounce(1 - x);
		public static float EaseOutBounce(float x) {
			const float n1 = 7.5625f;
			const float d1 = 2.75f;
			switch (x) {
				case < 1 / d1:
					return n1 * x * x;
				case < 2 / d1:
					return n1 * (x -= 1.5f / d1) * x + 0.75f;
			}
			if (x < 2.5 / d1) {
				return n1 * (x -= 2.25f / d1) * x + 0.9375f;
			}  {
				return n1 * (x -= 2.625f / d1) * x + 0.984375f;
			}
		}
		public static float EaseInOutBounce(float x) => x < 0.5
			? (1 - EaseOutBounce(1 - 2 * x)) / 2
			: (1 + EaseOutBounce(2 * x - 1)) / 2;
	}
}
