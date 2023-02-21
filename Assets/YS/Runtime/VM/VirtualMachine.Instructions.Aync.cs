using System;

namespace YS.VM {
    public  partial  class VirtualMachine{
        static unsafe  void AwaitableVoidPattern(VirtualMachine vm) {
            Variable awaiter;
            DelegateLibrary.GetPtr2(*(vm.CurrentDataPtr)++)(awaiter=vm.ReadVariable(), vm.ReadVariable());
            Variable isCompleted;
            DelegateLibrary.GetPtr2(*(vm.CurrentDataPtr)++)(isCompleted = vm.ReadVariable(), awaiter);
            var getResultId = vm.ReadUshort();
            if (isCompleted.As<bool>()) {
                vm.ReadUshort();
                DelegateLibrary.GetPtr1(*(vm.CurrentDataPtr)++)(awaiter);
            }
            else {
                var onCompleted = DelegateLibrary.GetPtr2(*(vm.CurrentDataPtr)++);
                var getResult = DelegateLibrary.GetPtr1(*(vm.CurrentDataPtr)++);
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