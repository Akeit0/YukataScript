namespace YS.VM {
    public  unsafe partial class VirtualMachine {
        static void Action(VirtualMachine vm) {
            DelegateLibrary.Delegates[*(vm.CurrentDataPtr)++].Action();
        }
        static void Action1(VirtualMachine vm) {
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate1s[*currentDataPtr++].Action(vm.Variables[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action2(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate2s[*currentDataPtr++].Action(vars[*currentDataPtr++],  vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action3(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate3s[*currentDataPtr++].Action( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action4(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate4s[*currentDataPtr++].Action( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action5(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate5s[*currentDataPtr++].Action( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++], vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }static void Action6(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.Delegate6s[*currentDataPtr++].Action( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++], vars[*currentDataPtr++],vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
    }
}