using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using YS.Collections;
using YS.VM;

namespace YS.Modules {
    public readonly struct MethodID {
        public readonly ushort Index;
        public readonly byte InstructionId;

        public MethodID(ushort index, byte instructionId) {
            Index = index;
            InstructionId = instructionId;
        }
        public (object Delegate, MethodData Description) GetData() {
            var tuple = DelegateLibrary.FromID(this);
            return (tuple.Delegate, tuple.MethodData);
        }
    }


    public enum FieldType {
        InstancedSetter,
        InstancedGetter,
        StaticSetter,
        StaticGetter,
    }
  
    public enum MethodType:byte {
        Static,
        Constructor,
        Instance,
        Extension
    }
    
    public enum PassType :byte{
        Value,
        In,
        Ref,
        Out
    }
    
    
    public readonly struct ParamDescription {
        public readonly string ParamName;
        public readonly Type Type;// TODO I want ushort IDs that corresponds to types, but can I handle the cost of searching every time?
        public readonly PassType PassType;

      
       public ParamDescription(string name,Type type,PassType passType= PassType.Value) {
            ParamName = string.Intern(name);
            Type = type;
            PassType =passType;
       }
      
       

        public ParamDescription(ParameterInfo info) {
            ParamName = info.Name;
            var type = info.ParameterType;
            if(type.IsByRef) {
                Type = info.ParameterType.GetElementType();
                if (info.IsIn) {
                    PassType = PassType.In;
                }else if (info.IsOut) {
                    PassType = PassType.Out;
                }
                else {
                    PassType = PassType.Ref;
                }
            }
            else {
                Type = type;
                PassType = PassType.Value;
            }
        }

        public override string ToString() {
            switch (PassType) {
                case PassType.Value: return Type.Build() + ' ' + ParamName;
                case PassType.In: return "in "+Type.Build() + ' ' + ParamName;
                case PassType.Ref: return "ref "+Type.Build() + ' ' + ParamName;
                case PassType.Out: return "out "+Type.Build() + ' ' + ParamName;
            }
            return base.ToString();
        }
    }
    public sealed class MethodData {
        public readonly Type DeclaringType;
        public readonly MethodType MethodType;
        public readonly string MethodName;
        public readonly ParamDescription[] ParamData;
        public readonly Variable[] DefaultValues;
        public readonly Type ReturnType;
        public readonly ushort Index;
        public readonly byte InstructionId;

        public MethodID ID => new MethodID(Index, InstructionId);
       public  int DefaultValueCount => DefaultValues?.Length ?? 0;
       public  int ParameterCount => ParamData?.Length ?? 0;
       public bool HasReturnValue => ReturnType is not  null;
       public  bool IsArgumentCountValidWith(int inputLength) {
          if (ParamData == null) {
              return (inputLength == 0 && MethodType == MethodType.Static)||
                     (inputLength == 1 && MethodType == MethodType.Instance);
          }
          var length = ParamData.Length+ (MethodType == MethodType.Instance ? 1 : 0);
          return length == inputLength|| (length > inputLength && length <= inputLength + DefaultValueCount);
       }
       public Span<Variable> GetDefaultValuesToSet(int inputCount) {
           if (DefaultValues==null||DefaultValues.Length==0) {
               return Span<Variable>.Empty;
           }
           var length = ParamData.Length;
           if(length!=inputCount) {
               return DefaultValues.AsSpan(DefaultValues.Length+inputCount-length);
           }
           return Span<Variable>.Empty;
       }
        public  MethodData(MethodInfo info,Delegate action) {
           var parameterInfos = info.GetParameters();
           var defaultValues = GetDefaultValues(parameterInfos);
           ParamDescription[] paramData=null;
            if (info.IsStatic) {
                if (0<parameterInfos.Length&&info.IsDefined(typeof(ExtensionAttribute)))
                    MethodType = MethodType.Extension;
                else
                    MethodType = MethodType.Static;
            }
            else {
                MethodType = MethodType.Instance;
            }
            if(parameterInfos.Length!=0) {
                paramData = new ParamDescription[parameterInfos.Length];
                for (var index = 0; index < parameterInfos.Length; index++) {
                    var parameterInfo = parameterInfos[index];
                    paramData[index]=new ParamDescription(parameterInfo);
                }
            }
            DeclaringType = info.DeclaringType;
            MethodName = string.Intern(info.Name);
            var returnType = info.ReturnType;
            if (returnType != typeof(void)) {
                ReturnType = returnType.IsByRef ? returnType.GetElementType() : returnType;
            }
            ParamData = paramData;
            DefaultValues = defaultValues;
            var actionParamCount = (byte) (parameterInfos.Length + (info.IsStatic ? 0 : 1)+(ReturnType is null?0:1));
            Index = DelegateLibrary.Add(this, action,actionParamCount);
            InstructionId = actionParamCount;
        }
     public  MethodData(MethodInfo info) {
        
           var parameterInfos = info.GetParameters();
           var defaultValues = GetDefaultValues(parameterInfos);
            
            ParamDescription[] paramData=null;
            if (info.IsStatic) {
                if (0<parameterInfos.Length&&info.IsDefined(typeof(ExtensionAttribute)))
                    MethodType = MethodType.Extension;
                else
                    MethodType = MethodType.Static;
            }
            else {
                MethodType = MethodType.Instance;
            }

            if(parameterInfos.Length!=0) {
                paramData = new ParamDescription[parameterInfos.Length];
                for (var index = 0; index < parameterInfos.Length; index++) {
                    var parameterInfo = parameterInfos[index];
                    paramData[index]=new ParamDescription(parameterInfo);
                }
            }
            
            DeclaringType = info.DeclaringType;
            MethodName = string.Intern(info.Name);
            var returnType = info.ReturnType;
            if (returnType != typeof(void)) {
                ReturnType = returnType.IsByRef ? returnType.GetElementType() : returnType;
            }
            ParamData = paramData;
            DefaultValues = defaultValues;

            if (MethodType == MethodType.Static&&ParamData == null&&ReturnType is null) {
                var action = (Action)Delegate.CreateDelegate(typeof(Action), info);
                Index = DelegateLibrary.Add(this, action,0);
                InstructionId = 0;
            }
            else 
            {
                var methodID = DelegateLibrary.Add(this, info);
                Index = methodID.Index;
                InstructionId = methodID.InstructionId;
            }
           
        }
     
       

        public static Action<Variable> CreateFunc<T>(MethodInfo methodInfo) {
            var d = (Func<T>) Delegate.CreateDelegate(typeof(Func<T>), methodInfo);
            return (result) => result.SetValue(d());
        }
        public static Action<Variable,Variable> CreateFunc<T1,T2>(MethodInfo methodInfo) {
            var d = (Func<T1,T2>) Delegate.CreateDelegate(typeof(Func<T1,T2>), methodInfo);
            return (result,input1) => result.SetValue(d(input1.As<T1>()));
        }
        public static Action<Variable> CreateAction<T>(MethodInfo methodInfo) {
            var d = (Action<T>) Delegate.CreateDelegate(typeof(Action<T>), methodInfo);
            return (input1) => d(input1.As<T>());
        }
        public static Action<Variable,Variable> CreateAction<T1,T2>(MethodInfo methodInfo) {
            var d = (Action<T1,T2>) Delegate.CreateDelegate(typeof(Action<T1,T2>), methodInfo);
            return (input1,input2) => d(input1.As<T1>(),input2.As<T2>());
        }
        
        
        public  MethodData (ConstructorInfo info,Delegate action) {
           var parameterInfos = info.GetParameters();
           var defaultValues = GetDefaultValues(parameterInfos);
           
            ParamDescription[] paramData=null;
            if(parameterInfos.Length!=0) {
                paramData = new ParamDescription[parameterInfos.Length];
                for (var index = 0; index < parameterInfos.Length; index++) {
                    var parameterInfo = parameterInfos[index];
                    paramData[index]=new ParamDescription(parameterInfo);
                }
            }
            DeclaringType = info.DeclaringType;
            MethodType = MethodType.Constructor;
            MethodName = ".ctor";
            ReturnType = DeclaringType;
            ParamData = paramData;
            DefaultValues = defaultValues;
            Index = DelegateLibrary.Add(this, action,parameterInfos.Length+1);
            InstructionId = (byte) (parameterInfos.Length+1);
         }
        public static MethodData New(FieldInfo fieldInfo,FieldType fieldType,Delegate action) {
           var name = fieldInfo.Name;
           switch (fieldType) {
               case FieldType.InstancedGetter: return new MethodData(fieldInfo.DeclaringType,MethodType.Instance,"get_"+name ,
                   fieldInfo.FieldType, null,action);
                case FieldType.InstancedSetter: return new MethodData(fieldInfo.DeclaringType,MethodType.Instance,"set_"+name ,
                   null, new []{new ParamDescription(name,fieldInfo.FieldType)},action);
               case FieldType.StaticGetter: return new MethodData(fieldInfo.DeclaringType,MethodType.Static,"set_"+name ,
                   fieldInfo.FieldType,null,action);
               case FieldType.StaticSetter: return new MethodData(fieldInfo.DeclaringType,MethodType.Static,"set_"+name ,
                   null, new []{new ParamDescription(name,fieldInfo.FieldType)},action);

           }

           return null;
       }
        public static (MethodData prefix ,MethodData postfix) NewRefUnary(Type declaringType,string paramName,bool isIncrement,Delegate prefixAction,Delegate postfixAction) {
           var prefix = new MethodData(declaringType, MethodType.Static,
               isIncrement ? "op_Increment" : "op_Decrement", declaringType,
               new ParamDescription[] {new (paramName, declaringType, PassType.Ref)},prefixAction);
           var postfix = new MethodData(declaringType, MethodType.Static,
               isIncrement ? "op_PostIncrement" : "op_PostDecrement", declaringType,
               new ParamDescription[] {new (paramName, declaringType, PassType.Ref)},postfixAction);
           return (prefix, postfix);

       }
        public MethodData(Type declaringType,MethodType methodType, string methodName, Type returnType, ParamDescription[] paramData,Delegate action,Variable[] defaultValues=null) {
           DeclaringType = declaringType;
           MethodType = methodType;
           MethodName = string.Intern(methodName);
           if (returnType is not null && returnType != typeof(void)) {
               ReturnType = returnType.IsByRef ? returnType.GetElementType() : returnType;
           }
           ParamData = paramData;
           DefaultValues = defaultValues;
           var actionParamCount = (byte)((ParamData?.Length ?? 0)+ (MethodType == MethodType.Instance ? 1 : 0)+
                                                   (ReturnType is null ? 0 : 1));
           Index = DelegateLibrary.Add(this, action,actionParamCount);
           InstructionId = actionParamCount;
          
       }
       
       public override string ToString() {
           var builder = new StringBuilder(20);
           if (MethodType == MethodType.Constructor) {
               builder.Append(ReturnType.Build());
           }
           else {
               if(MethodType==MethodType.Static) {
                   builder.Append("static ");
               }

               builder.Append(ReturnType is null ? "void" : ReturnType.Build());
                
               builder.Append(' ');
               builder.Append(MethodName);
           }
           
           builder.Append('(');
           if (MethodType == MethodType.Instance) {
               builder.Append( DeclaringType.Build() );
               builder.Append( ' ' );
               builder.Append("this");
           }
           if (ParamData != null) {
               int i = 0;
               for (; i < ParamData.Length-1; i++) {
                   builder.Append(ParamData[i].ToString());
                   builder.Append(',');
                   builder.Append(' ');
               }
               builder.Append(ParamData[i].ToString());
           }
           builder.Append(')');
          return builder.ToString();
       }

       public bool IsAppropriate(Span<Type> inputs) {
           if (!IsArgumentCountValidWith(inputs.Length)) return false;
           for (int i = 0; i < inputs.Length; i++) {
               if (!IsAssignable(ParamData[i].Type,inputs[i])) return false;
           }
           return true;
       }
       
       public static bool IsAssignable(Type target, Type src) {
           if(target==typeof(Variable))return true;
           if (target.IsAssignableFrom(src)) return true;
           return target==typeof(float)&&(src == typeof(int)||src == typeof(double));
       }
      
       
       public static Variable[] GetDefaultValues(ParameterInfo[] parameterInfos) {
            
           int length = 0;
           for (int i = parameterInfos.Length-1; 0<=i ; --i) {
               if (parameterInfos[i].HasDefaultValue) {
                   ++length;
               }else break;
           }
           if(length==0)return null;
            var array= new Variable[length];
            var defaultStart = parameterInfos.Length - length;
           for (int i = parameterInfos.Length-1; defaultStart<=i ; --i) {
               var defaultValue = parameterInfos[i].RawDefaultValue;
               var arrayIndex = i - defaultStart;
               if (defaultValue == null) {
                   array[arrayIndex] = Constants.Null;
               }
               else {
                   var type = defaultValue.GetType();
                   if (type == typeof(bool)) {
                       array[arrayIndex] =(bool)defaultValue?Constants.True:Constants.False;
                       continue;
                   }
                   var box = Variable.New(type);
                   box.SetObject(defaultValue);
                   array[arrayIndex] =box;
               }
           }

           return array;
       }

    }
}