using System;
using System.Text;
using YS.VM;
// ReSharper disable EqualExpressionComparison

namespace YS.Instructions {
    public sealed class IntRangeOp : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static IntRangeOp (){
            IInstruction.Instructions[Id]= new IntRangeOp();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(new IntRange(vm.ReadVariable().As<int>(),vm.ReadVariable().As<int>())); 
        }

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
        public static readonly byte Id = IInstruction.Count++;
        static NotOp (){
            IInstruction.Instructions[Id]= new NotOp();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(!vm.ReadVariable().As<bool>()); 
        }
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
        public static readonly byte Id = IInstruction.Count++;
        static ToIntOp (){
            IInstruction.Instructions[Id]= new ToIntOp();
        }
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
   

    public sealed class IntArithmeticOp : IInstruction {
        public static readonly byte Id = IInstruction.Count++;

        static IntArithmeticOp() {
            IInstruction.Instructions[Id] = new IntArithmeticOp();
        }
        public void Execute(VirtualMachine vm) {
            var type =  vm.ReadUshort();
            switch (type) {
                case 0: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() + vm.ReadVariable().As<int>()); return;
                case 1: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() - vm.ReadVariable().As<int>()); return;
                case 2: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() * vm.ReadVariable().As<int>()); return;
                case 3: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() / vm.ReadVariable().As<int>()); return;
                case 4: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() % vm.ReadVariable().As<int>()); return;
                case 5: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() == vm.ReadVariable().As<int>()); return;
                case 6: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() != vm.ReadVariable().As<int>()); return;
                case 7: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() < vm.ReadVariable().As<int>()); return;
                case 8: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() >vm.ReadVariable().As<int>()); return;
                case 9: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() <= vm.ReadVariable().As<int>()); return;
                case 10: vm.ReadVariable().SetValue(vm.ReadVariable().As<int>() >= vm.ReadVariable().As<int>()); return;
            }
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" + ");
            IInstruction.AppendVariable(builder, vm);
        }
    }

    public sealed class LongArithmeticOp : IInstruction {
        public static readonly byte Id = IInstruction.Count++;

        static LongArithmeticOp() {
            IInstruction.Instructions[Id] = new LongArithmeticOp();
        }

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
            IInstruction.AppendIndent(builder, indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" + ");
            IInstruction.AppendVariable(builder, vm);
        }
    }

    public sealed class ULongArithmeticOp : IInstruction { 
        public static readonly byte Id = IInstruction.Count++;

        static ULongArithmeticOp() {
            IInstruction.Instructions[Id] = new ULongArithmeticOp();
        }
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
            IInstruction.AppendIndent(builder,indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" + ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class FloatArithmeticOp : IInstruction {
        public static readonly byte Id = IInstruction.Count++;

        static FloatArithmeticOp() {
            IInstruction.Instructions[Id] = new FloatArithmeticOp();
        }
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
            IInstruction.AppendIndent(builder,indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" + ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class DoubleArithmeticOp : IInstruction {
        public static readonly byte Id = IInstruction.Count++;

        static DoubleArithmeticOp() {
            IInstruction.Instructions[Id] = new DoubleArithmeticOp();
        }
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
            IInstruction.AppendIndent(builder,indentLevel);
            vm.ReadUshort();
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" + ");
            IInstruction.AppendVariable(builder, vm);
        }
    }

    
    
    
    
}