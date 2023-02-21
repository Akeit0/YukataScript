namespace YS.VM {
    public  unsafe partial class VirtualMachine {
        static void Action(VirtualMachine vm) {
            DelegateLibrary.GetPtr0(*(vm.CurrentDataPtr)++)();
        }
        static void Action1(VirtualMachine vm) {
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr1(*currentDataPtr++)(vm.Variables[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action2(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr2(*currentDataPtr++)(vars[*currentDataPtr++],  vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action3(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr3(*currentDataPtr++)( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
       
        static void Action4(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr4(*currentDataPtr++)( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
        static void Action5(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr5(*currentDataPtr++)( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++], vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }static void Action6(VirtualMachine vm){
            var vars = vm.Variables;
            var currentDataPtr = vm.CurrentDataPtr;
            DelegateLibrary.GetPtr6(*currentDataPtr++)( vars[*currentDataPtr++],  vars[*currentDataPtr++],
                vars[*currentDataPtr++],  vars[*currentDataPtr++], vars[*currentDataPtr++],vars[*currentDataPtr++]);
            vm.CurrentDataPtr = currentDataPtr;
        }
    }
}