// ReSharper disable EqualExpressionComparison
namespace YS.VM {
    public enum BinaryOpType :ushort{
        Addition=0,
        Subtraction=1,
        Multiply=2,
        Division=3,
        Modulus=4,
        Equal=5,
        Inequal=6,
        LessThan=7,
        GreaterThan=8,
        LessThanOrEqual=9,
        GreaterThanOrEqual=10,
        BitwiseAnd=11,
        BitwiseOr=12,
        ExclusiveOr=13,
        LeftShift=14,
        RightShift=15
    }
    public partial class VirtualMachine {
        static void IntRange(VirtualMachine vm) {
            vm.ReadVariable<IntRange>().value=(new IntRange(vm.ReadValue<int>(),vm.ReadValue<int>())); 
        }
        static void IntArithmetic(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable<int>().value=(vm.ReadValue<int>() + vm.ReadValue<int>()); return;
                case 1: vm.ReadVariable<int>().value=(vm.ReadValue<int>() - vm.ReadValue<int>()); return;
                case 2: vm.ReadVariable<int>().value=(vm.ReadValue<int>() * vm.ReadValue<int>()); return;
                case 3: vm.ReadVariable<int>().value=(vm.ReadValue<int>() / vm.ReadValue<int>()); return;
                case 4: vm.ReadVariable<int>().value=(vm.ReadValue<int>() % vm.ReadValue<int>()); return;
                case 5: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() == vm.ReadValue<int>()); return;
                case 6: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() != vm.ReadValue<int>()); return;
                case 7: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() < vm.ReadValue<int>()); return;
                case 8: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() >vm.ReadValue<int>()); return;
                case 9: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() <= vm.ReadValue<int>()); return;
                case 10: vm.ReadVariable<bool>().value=(vm.ReadValue<int>() >= vm.ReadValue<int>()); return;
            }
        }   
        static void FloatArithmetic(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable<float>().value=(vm.ReadValue<float>() + vm.ReadValue<float>()); return;
                case 1: vm.ReadVariable<float>().value=(vm.ReadValue<float>() - vm.ReadValue<float>()); return;
                case 2: vm.ReadVariable<float>().value=(vm.ReadValue<float>() * vm.ReadValue<float>()); return;
                case 3: vm.ReadVariable<float>().value=(vm.ReadValue<float>() / vm.ReadValue<float>()); return;
                case 4: vm.ReadVariable<float>().value=(vm.ReadValue<float>() % vm.ReadValue<float>()); return;
                case 5: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() == vm.ReadValue<float>()); return;
                case 6: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() != vm.ReadValue<float>()); return;
                case 7: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() < vm.ReadValue<float>()); return;
                case 8: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() >vm.ReadValue<float>()); return;
                case 9: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() <= vm.ReadValue<float>()); return;
                case 10: vm.ReadVariable<bool>().value=(vm.ReadValue<float>() >= vm.ReadValue<float>()); return;
            }
        }
        static void DoubleArithmetic(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable<double>().value=(vm.ReadValue<double>() + vm.ReadValue<double>()); return;
                case 1: vm.ReadVariable<double>().value=(vm.ReadValue<double>() - vm.ReadValue<double>()); return;
                case 2: vm.ReadVariable<double>().value=(vm.ReadValue<double>() * vm.ReadValue<double>()); return;
                case 3: vm.ReadVariable<double>().value=(vm.ReadValue<double>() / vm.ReadValue<double>()); return;
                case 4: vm.ReadVariable<double>().value=(vm.ReadValue<double>() % vm.ReadValue<double>()); return;
                case 5: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() == vm.ReadValue<double>()); return;
                case 6: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() != vm.ReadValue<double>()); return;
                case 7: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() < vm.ReadValue<double>()); return;
                case 8: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() >vm.ReadValue<double>()); return;
                case 9: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() <= vm.ReadValue<double>()); return;
                case 10: vm.ReadVariable<bool>().value=(vm.ReadValue<double>() >= vm.ReadValue<double>()); return;
            }
        }

    }
}