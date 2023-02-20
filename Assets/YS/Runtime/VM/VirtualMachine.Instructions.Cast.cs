namespace YS.VM {
        public partial class VirtualMachine {
                static void Boxing(VirtualMachine vm) {
                        vm.ReadVariable<object>().value=(vm.ReadVariable().ToObject());
                }
               
                static void IntToFloat(VirtualMachine vm) {
                        vm.ReadVariable<float>().value=( vm.ReadValue<int>());
                }
                static void IntToDouble(VirtualMachine vm) {
                        vm.ReadVariable<double>().value=(vm.ReadValue<int>());
                }
                static void FloatToDouble(VirtualMachine vm) {
                        vm.ReadVariable<double>().value=( vm.ReadValue<float>());
                }
        }
}