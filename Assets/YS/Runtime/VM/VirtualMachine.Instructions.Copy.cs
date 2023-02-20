namespace YS.VM {
    public unsafe partial  class VirtualMachine {
       
        static void Read8 (VirtualMachine vm) {
            vm.ReadVariable<byte>().value=((byte)*vm.CurrentDataPtr++); 
        }
        static void Read16 (VirtualMachine vm) {
            vm.ReadVariable<ushort>().value=(*vm.CurrentDataPtr++); 
        }
    
        static void Read32 (VirtualMachine vm) {
            vm.ReadVariable<int>().value=(*(int*)vm.CurrentDataPtr);
            vm.CurrentDataPtr += 2;
        }


        static void Read64 (VirtualMachine vm) {
            vm.ReadVariable<long>().value=(*(long*)vm.CurrentDataPtr);
            vm.CurrentDataPtr += 4;
        }
        static void Copy(VirtualMachine vm) {
            vm.ReadVariable().CopyFrom(vm.ReadVariable()); 
        }
        static void CopyObject(VirtualMachine vm) {
            vm.ReadVariable<object>().value=vm.ReadValue<object>();
        }
        static void IsNull(VirtualMachine vm) {
            vm.ReadVariable<bool>().value=vm.ReadValue<object>()is null;
        }
    }
}