using System;
using System.Text;
using YS.VM;
// ReSharper disable EqualExpressionComparison

namespace YS.Instructions {
    public sealed class IntRangeOp : IInstruction {

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" .. ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class NotOp : IInstruction {
       
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = !");
            IInstruction.AppendVariable(builder, vm);
        }
    }

    public enum ShortIntegers :ushort{
        Byte,
        Sbyte,
        Char,
        Short,
        Ushort,
    }
    public sealed class ToIntOp : IInstruction {
        public void Execute(VirtualMachine vm) {
            switch ((ShortIntegers)vm.ReadUshort()) {
                case ShortIntegers.Byte:  vm.ReadVariable().SetValue(-vm.ReadVariable().As<byte>()); 
                    return;
                case ShortIntegers.Sbyte:  vm.ReadVariable().SetValue(-vm.ReadVariable().As<sbyte>()); 
                    return;
                case ShortIntegers.Char:  vm.ReadVariable().SetValue(-vm.ReadVariable().As<char>()); 
                    return;
                case ShortIntegers.Short:  vm.ReadVariable().SetValue(-vm.ReadVariable().As<short>()); 
                    return;
                case ShortIntegers.Ushort:  vm.ReadVariable().SetValue(-vm.ReadVariable().As<ushort>()); 
                    return;
            }
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = (int)");
            IInstruction.AppendVariable(builder, vm);
        }
    }
   
   

    public sealed class IntArithmeticOp : IInstruction {

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            ArithmeticToCode(vm, builder, ref indentLevel);
        }
        public static void ArithmeticToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            var type= vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            switch (type) {
                case 0:builder.Append(" + ");
                    break;
                case 1:builder.Append(" - ");
                    break;
                case 2:builder.Append(" * ");
                    break;
                case 3:builder.Append(" / ");
                    break;
                case 4:builder.Append(" % ");
                    break;
                case 5:builder.Append(" == ");
                    break;
                case 6:builder.Append(" != ");
                    break;
                case 7:builder.Append(" < ");
                    break;
                case 8:builder.Append(" > ");
                    break;
                case 9:builder.Append(" <= ");
                    break;
                case 10:builder.Append(" >= ");
                    break;
            }
            
            IInstruction.AppendVariable(builder, vm);
        }
    }

    public sealed class LongArithmeticOp : IInstruction {
       

        public void Execute(VirtualMachine vm) {
            var type = vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() + vm.ReadVariable().As<long>()); return;
                case 1: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() - vm.ReadVariable().As<long>()); return;
                case 2: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() * vm.ReadVariable().As<long>()); return;
                case 3: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() / vm.ReadVariable().As<long>()); return;
                case 4: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() % vm.ReadVariable().As<long>()); return;
                case 5: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() == vm.ReadVariable().As<long>()); return;
                case 6: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() != vm.ReadVariable().As<long>()); return;
                case 7: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() < vm.ReadVariable().As<long>()); return;
                case 8: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() >vm.ReadVariable().As<long>()); return;
                case 9: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() <= vm.ReadVariable().As<long>()); return;
                case 10: vm.ReadVariable().SetValue(vm.ReadVariable().As<long>() >= vm.ReadVariable().As<long>()); return;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IntArithmeticOp.ArithmeticToCode(vm, builder, ref indentLevel);
        }
    }

    public sealed class ULongArithmeticOp : IInstruction { 
      
        public void Execute(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() + vm.ReadVariable().As<ulong>()); return;
                case 1: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() - vm.ReadVariable().As<ulong>()); return;
                case 2: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() * vm.ReadVariable().As<ulong>()); return;
                case 3: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() / vm.ReadVariable().As<ulong>()); return;
                case 4: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() % vm.ReadVariable().As<ulong>()); return;
                case 5: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() == vm.ReadVariable().As<ulong>()); return;
                case 6: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() != vm.ReadVariable().As<ulong>()); return;
                case 7: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() < vm.ReadVariable().As<ulong>()); return;
                case 8: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() >vm.ReadVariable().As<ulong>()); return;
                case 9: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() <= vm.ReadVariable().As<ulong>()); return;
                case 10: vm.ReadVariable().SetValue(vm.ReadVariable().As<ulong>() >= vm.ReadVariable().As<ulong>()); return;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IntArithmeticOp.ArithmeticToCode(vm, builder, ref indentLevel);
        }
    }
    public sealed class FloatArithmeticOp : IInstruction {
     
        public void Execute(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() + vm.ReadVariable().As<float>()); return;
                case 1: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() - vm.ReadVariable().As<float>()); return;
                case 2: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() * vm.ReadVariable().As<float>()); return;
                case 3: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() / vm.ReadVariable().As<float>()); return;
                case 4: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() % vm.ReadVariable().As<float>()); return;
                case 5: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() == vm.ReadVariable().As<float>()); return;
                case 6: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() != vm.ReadVariable().As<float>()); return;
                case 7: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() < vm.ReadVariable().As<float>()); return;
                case 8: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() >vm.ReadVariable().As<float>()); return;
                case 9: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() <= vm.ReadVariable().As<float>()); return;
                case 10: vm.ReadVariable().SetValue(vm.ReadVariable().As<float>() >= vm.ReadVariable().As<float>()); return;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IntArithmeticOp.ArithmeticToCode(vm, builder, ref indentLevel);
        }
    }
    public sealed class DoubleArithmeticOp : IInstruction {
      
        public void Execute(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() + vm.ReadVariable().As<double>()); return;
                case 1: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() - vm.ReadVariable().As<double>()); return;
                case 2: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() * vm.ReadVariable().As<double>()); return;
                case 3: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() / vm.ReadVariable().As<double>()); return;
                case 4: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() % vm.ReadVariable().As<double>()); return;
                case 5: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() == vm.ReadVariable().As<double>()); return;
                case 6: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() != vm.ReadVariable().As<double>()); return;
                case 7: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() < vm.ReadVariable().As<double>()); return;
                case 8: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() >vm.ReadVariable().As<double>()); return;
                case 9: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() <= vm.ReadVariable().As<double>()); return;
                case 10: vm.ReadVariable().SetValue(vm.ReadVariable().As<double>() >= vm.ReadVariable().As<double>()); return;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IntArithmeticOp.ArithmeticToCode(vm, builder, ref indentLevel);
        }
    }

    
    
    
    
}