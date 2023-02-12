using System.Text;
using YS.VM;

namespace YS.Instructions {
   
    public sealed class Copy : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Copy (){
            IInstruction.Instructions[Id]= new Copy();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().CopyFrom(vm.ReadVariable()); 
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
        }

    }
    public sealed class Read16 : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Read16 (){
            IInstruction.Instructions[Id]= new Read16();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.ReadUshort()); 
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" =  ");
            builder.Append(vm.ReadUshort());
        }

    }
    public sealed class Read32 : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Read32 (){
            IInstruction.Instructions[Id]= new Read32();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.Read<int>()); 
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" =  ");
            builder.Append(vm.Read<int>());
        }

    }
   
    
    public sealed class Read64 : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Read64 (){
            IInstruction.Instructions[Id]= new Read64();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.Read<long>()); 
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("copy 64bit integer to");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" ");
            builder.Append(vm.Read<long>());
        }
    }
   
    public sealed class Is_null : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Is_null (){
            IInstruction.Instructions[Id]= new Is_null();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.ReadVariable().As<object>() is null); 
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" is null ");
        }
    }
    
    public sealed class CopyObject : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static CopyObject (){
            IInstruction.Instructions[Id]= new CopyObject();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.ReadVariable().As<object>());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = (object)");
            IInstruction.AppendVariable(builder, vm);
        }
    } 
    public sealed class Boxing : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static Boxing (){
            IInstruction.Instructions[Id]= new Boxing();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue(vm.ReadVariable().ToObject());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" = (object) ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    
    public sealed class NumericCast : IInstruction {
        public void Execute(VirtualMachine vm) {
            throw new System.NotImplementedException();
        }

        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            throw new System.NotImplementedException();
        }
    }
    


    public sealed class IntToDouble : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static IntToDouble (){
            IInstruction.Instructions[Id]= new IntToDouble();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue((double)vm.ReadVariable().As<int>());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast int to double : to ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class IntToFloat : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static IntToFloat (){
            IInstruction.Instructions[Id]= new IntToFloat();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue((float)vm.ReadVariable().As<int>());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast int to float : to ");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
    public sealed class FloatToDouble : IInstruction {
        public static readonly byte Id = IInstruction.Count++;
        static FloatToDouble (){
            IInstruction.Instructions[Id]= new FloatToDouble();
        }
        public void Execute(VirtualMachine vm) {
            vm.ReadVariable().SetValue((double)vm.ReadVariable().As<float>());
        }
        public void ToCode(VirtualMachine vm, StringBuilder builder, ref int indentLevel) {
            IInstruction.AppendIndent(builder,indentLevel);
            builder.Append("cast float to double : to");
            IInstruction.AppendVariable(builder, vm);
            builder.Append(" from ");
            IInstruction.AppendVariable(builder, vm);
        }
    }
}