#if !UNITY_EDITOR&&ENABLE_IL2CPP
#define AOT
#endif


using System;

namespace YS.VM {
    public enum Instructions:byte {
        Action,
        Action1,
        Action2,
        Action3,
        Action4,
        Action5,
        Action6,
        MethodInfoInvoke,
        Copy,
        Read8,
        Read16,
        Read32,
        Read64,
        CopyObject,
        IsNull,
        Return,
        Break,
        BreakIf,
        Continue,
        InfiniteLoop,
        ForEach,
        If,
        IfElse,
        Else,
        AwaitableVoidPattern,
        CustomAwaiter,
        IntRange,
        IntArithmetic,
        FloatArithmetic,
        DoubleArithmetic,
        Boxing,
        IntToFloat,
        IntToDouble,
        FloatToDouble,
    }
    public unsafe partial  class VirtualMachine {
        static delegate*<VirtualMachine, void>[] InstructionArray;

        static VirtualMachine() {
            InitInstructions();
        }
        static void InitInstructions() {
#if AOT
            InstructionArray=new delegate*<VirtualMachine, void>[] {
                &Action,
                &Action1,
                &Action2,
                &Action3,
                &Action4,
                &Action5,
                &Action6,
                &MethodInfoInvoke,
                &Copy,
                &Read8,
                &Read16,
                &Read32,
                &Read64,
                &CopyObject,
                &IsNull,
                &Return,
                &Break,
                &BreakIf,
                &Continue,
                &InfiniteLoop,
                &ForEach,
                &If,
                &IfElse,
                &Else,
                &AwaitableVoidPattern,
                &CustomAwaiter,
                &IntRange,
                &IntArithmetic,
                &FloatArithmetic,
                &DoubleArithmetic,
                &Boxing,
                &IntToFloat,
                &IntToDouble,
                &FloatToDouble,
            };
#else
             InstructionArray=new delegate*<VirtualMachine, void>[] {
                GetFunctionPtr(Action),
                GetFunctionPtr(Action1),
                GetFunctionPtr(Action2),
                GetFunctionPtr(Action3),
                GetFunctionPtr(Action4),
                GetFunctionPtr(Action5),
                GetFunctionPtr(Action6),
                GetFunctionPtr(MethodInfoInvoke),
                GetFunctionPtr(Copy),
                GetFunctionPtr(Read8),
                GetFunctionPtr(Read16),
                GetFunctionPtr(Read32),
                GetFunctionPtr(Read64),
                GetFunctionPtr(CopyObject),
                GetFunctionPtr(IsNull),
                GetFunctionPtr(Return),
                GetFunctionPtr(Break),
                GetFunctionPtr(BreakIf),
                GetFunctionPtr(Continue),
                GetFunctionPtr(InfiniteLoop),
                GetFunctionPtr(ForEach),
                GetFunctionPtr(If),
                GetFunctionPtr(IfElse),
                GetFunctionPtr(Else),
                GetFunctionPtr(AwaitableVoidPattern),
                GetFunctionPtr(CustomAwaiter),
                GetFunctionPtr(IntRange),
                GetFunctionPtr(IntArithmetic),
                GetFunctionPtr(FloatArithmetic),
                GetFunctionPtr(DoubleArithmetic),
                GetFunctionPtr(Boxing),
                GetFunctionPtr(IntToFloat),
                GetFunctionPtr(IntToDouble),
                 GetFunctionPtr(FloatToDouble),
            };
#endif
        }
        /// <summary>
        /// This method is to avoid to get slower function pointer on mono.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        static delegate*<VirtualMachine, void> GetFunctionPtr(Action<VirtualMachine> action) {
            return (delegate*<VirtualMachine, void>)action.Method.MethodHandle.GetFunctionPointer();
        }

    }
}