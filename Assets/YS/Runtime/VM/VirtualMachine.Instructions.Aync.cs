using System;

namespace YS.VM {
    public  partial  class VirtualMachine{
        static void AwaitableVoidPattern(VirtualMachine vm) {
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

        static void CustomAwaiter(VirtualMachine vm) {
            var awaiter=  DelegateLibrary.Awaiters[vm.ReadUshort()];
            var result = vm.ReadVariable();
            var awaitable = vm.ReadVariable();
            awaiter.Run(awaitable,result,vm);
        }
    }
}