using System;
using System.Text;
using YS.VM;

namespace YS.Instructions {
    public class AwaitableVoidPattern : IInstruction{
        public static readonly byte Id = IInstruction.Count++;
        static AwaitableVoidPattern (){
            IInstruction.Instructions[Id]= new AwaitableVoidPattern();
        }
        public void Execute(VirtualMachine vm) {
            Variable awaiter;
            DelegateLibrary.Delegate2s[vm.ReadUshort()].Action(awaiter=vm.ReadVariable(), vm.ReadVariable());
            Variable isCompleted;
            DelegateLibrary.Delegate2s[vm.ReadUshort()].Action(isCompleted = vm.ReadVariable(), awaiter);
            var getResultId = vm.ReadUshort();
            if (isCompleted.As<bool>()) {
                vm.ReadUshort();
                DelegateLibrary.Delegate1s[getResultId].Action(awaiter);
            }
            else {
                var onCompleted = DelegateLibrary.Delegate2s[vm.ReadUshort()].Action;
                var getResult = DelegateLibrary.Delegate1s[getResultId].Action;
                vm.SetAsyncState();
                onCompleted(awaiter, new Variable<Action>(() => {
                    try {
                        getResult(awaiter);
                    }
                    catch (Exception e) {
                        vm.Exception = e;
                    }
                    finally {
                        vm.ProcessFromLast();
                    }
                }));
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            throw new NotImplementedException();
        }
    }
    public class CustomAwaiter : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static CustomAwaiter (){
            IInstruction.Instructions[Id]= new CustomAwaiter();
        }
        public void Execute(VirtualMachine vm) {
            var awaiter=  DelegateLibrary.Awaiters[vm.ReadUshort()];
            var result = vm.ReadVariable();
            var awaitable = vm.ReadVariable();
            awaiter.Run(awaitable,result,vm);
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder, indentLevel);
           vm.ReadUshort();
           var num = vm.ReadUshort();
           var variable = vm.Variables[num];
           if(variable .type!=null) {
               builder.Append("var_");
               builder.Append(num);
               builder.Append(" : ");
               builder.Append( variable.type.FullName );
               builder.Append(" = ");
           }
            builder.Append(" await   ");
            IInstruction.AppendVariable(builder, vm);
            
        }
    
    }
}