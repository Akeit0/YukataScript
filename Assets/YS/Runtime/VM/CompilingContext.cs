using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using YS.AST;
using YS.Collections;
using YS.Lexer;
using YS.Modules;
using YS.Instructions;
using YS.Text;

namespace YS.VM {
    
    public class CompilingContext {
        Scope scope;
        public VirtualMachine Engine;

        public SimpleList<NameSpace> NameSpaceNodes=new ();
        public SimpleList<TypeModule> UsingStaticModules=new ();
        public readonly UshortSet ConstSet=new (16);
        public readonly StringHashSet ConstNameSet=new (32);
        public SimpleList<ushort> Lines;

        
   
        int currentStatementNum;
        public void MoveToNextStatement() => ++currentStatementNum;
        public ushort GetCurrentLine() => Lines[currentStatementNum];

        public void Init(SimpleList<ushort> lines) {
            Lines = lines;
            ConstSet.Clear();
            ConstSet.Add(1);
            ConstNameSet.Clear();
            NameSpaceNodes.Clear();
            UsingStaticModules.Clear();
            currentStatementNum = -1;
        }
        public CompilingContext() {
            scope = new Scope();
        }

        public void SetEngine(VirtualMachine engine) {
            Engine = engine;
            scope.Dictionary = engine.Dictionary;
            Engine.CodeToLine.Clear();
        }

        public void AddNameSpace(string name) {
            if (ModuleLibrary.Root.TryGetNameSpace(name, out var node)) {
                NameSpaceNodes.Add(node);
            }
            else {
                throw new Exception(name+"not found");
            }
        }
        public void AddUsingStaticType(string name) {
            if (ModuleLibrary.Root.TryGetType(name, out var type)) {
                UsingStaticModules.Add(ModuleLibrary.GetModule(type));
            }
            else {
                throw new Exception(name+"not found");
            }
        }
        public void Emit(byte opCode) {
            Engine.CodeToLine.Add(GetCurrentLine());
            Engine.Emit(opCode);
        }
        public void EmitMethod(MethodID id) {
            Engine.CodeToLine.Add(GetCurrentLine());
            Engine.Emit(id);
        }
    
        public unsafe void EmitIntData(int data){
            var d = data;
            var p = (ushort*) &d;
            for (int i = 0; i < 2; i++) {
                EmitData(p[i]);
            }
        }
        public unsafe void EmitFloatData(float data){
            var d = data;
            var p = (ushort*) &d;
            for (int i = 0; i < 2; i++) {
                EmitData(p[i]);
            }
        }
        public unsafe void EmitDoubleData(double data){
            var d = data;
            var p = (ushort*) &d;
            for (int i = 0; i < 4; i++) {
                EmitData(p[i]);
            }
        }
        public void EmitData(ushort data) => Engine.EmitData(data);
        public void EmitData(ArgumentData data) => Engine.EmitData(data.Id);
        public void EmitData(ushort data0,ushort data1)=> Engine.EmitData(data0,data1);
        public void EmitData(ushort data0,ushort data1,ushort data2) => Engine.EmitData(data0,data1,data2);
        
        
        public void EmitCopy(ushort target,ushort src) {
            EmitData(target,src);
            Emit(Copy.Id);
        }
        public void EmitCopyObject(ushort target,ushort src) {
            EmitData(target,src);
            Emit(CopyObject.Id);
        }
        public void EmitReadChar(ushort target,ushort value) {
            EmitData(target);
            EmitData(value);
            Emit(Read16.Id);
        }
        public void EmitReadInt(ushort target,int value) {
            EmitData(target);
            EmitIntData(value);
            Emit(Read32.Id);
        }
        public void EmitReadFloat(ushort target,float value) {
            EmitData(target);
            EmitFloatData(value);
            Emit(Read32.Id);
        }
        
        public void EmitReadDouble(ushort target,double value) {
            EmitData(target);
            EmitDoubleData(value);
            Emit(Read64.Id);
        }


        public bool TryEmitIntrinsic(BinaryOpType type,(ushort id,Variable variable) target, (ushort id,Variable variable) left, (ushort id,Variable variable) right) {
            
            switch (left.variable) {
                case Variable<int>: {
                    if (right.variable is Variable<int>) {
                        if ((type is < BinaryOpType.Equal or >= BinaryOpType.BitwiseAnd)&&target.variable is Variable<int>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }if (target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }
                    }
                    return false;
                }case Variable<float>: {
                    if (right.variable is Variable<float>) {
                        if (type<BinaryOpType.Equal&&target.variable is Variable<float>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }if (target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }
                    }
                    return false;
                }case Variable<double>: {
                    if (right.variable is Variable<double>) {
                        if (type<BinaryOpType.Equal&&target.variable is Variable<double>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(DoubleArithmeticOp.Id);
                            return true;
                        }if ( BinaryOpType.Equal>=type && target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(DoubleArithmeticOp.Id);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        public bool TryEmitIntrinsic(BinaryOpType type,(ushort id,Variable variable) target, (ushort id,Variable variable,bool isLiteral) left, (ushort id,Variable variable,bool isLiteral) right) {
           
            switch (left.variable) {
                case Variable<int>: {
                    if (right.variable is Variable<int>) {
                        if ((type is < BinaryOpType.Equal or >= BinaryOpType.BitwiseAnd)&&target.variable is Variable<int>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }if (target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }
                    }
                    return false;
                }case Variable<float>: {
                    if (right.variable is Variable<float>) {
                        if (type<BinaryOpType.Equal&&target.variable is Variable<float>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }if (target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }
                    }else if (right.variable is Variable<int>) {
                        if (right.isLiteral) {
                            var current= right.variable.As<int>();
                            if (current == 0) {
                                var newVariable = Constants.FloatZero;
                                Set(right.id,newVariable);
                            }
                            else {
                                var newVariable = new Variable<float>(current);
                                Set(right.id,newVariable);
                            }
                        }
                        else {
                            var newValueId = AddVariable<float>().id;
                            EmitData(newValueId, right.id);
                            right.id= newValueId;
                            Emit(IntToFloat.Id);
                        }
                        if (type<BinaryOpType.Equal&&target.variable is Variable<float>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }if (target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }
                        
                    }
                    return false;
                }case Variable<double>: {
                    if (right.variable is Variable<double>) {
                        if (type<BinaryOpType.Equal&&target.variable is Variable<double>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(DoubleArithmeticOp.Id);
                            return true;
                        }if ( BinaryOpType.Equal>=type && target.variable is Variable<bool>) {
                            EmitData((ushort)type);
                            EmitData(target.id,left.id,right.id);
                            Emit(DoubleArithmeticOp.Id);
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }
        public bool TryEmitIntrinsic(TokenType tokenType,(ushort id,Variable variable) left, (ushort id,Variable variable) right,out (ushort id,Variable variable) target) {
            var type = tokenType.ToBinaryOpType();
            switch (left.variable) {
                case Variable<int>: {
                    if (right.variable is Variable<int>) {
                        if (type is < BinaryOpType.Equal or >= BinaryOpType.BitwiseAnd) {
                            EmitData((ushort)type);
                            var newVariable = new Variable<int>();
                            var targetId = AddVariable(newVariable);
                           target = (targetId, newVariable);
                            EmitData(targetId,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }else {
                            EmitData((ushort)type);
                            var newVariable = new Variable<bool>();
                            var targetId = AddVariable(newVariable);
                            target = (targetId, newVariable);
                            EmitData(target.id,left.id,right.id);
                            Emit(IntArithmeticOp.Id);
                            return true;
                        }
                    }

                    target = default;
                    return true;
                }case Variable<float>: {
                    if (right.variable is Variable<float>) {
                        if (type<BinaryOpType.Equal) {
                            EmitData((ushort)type);
                            var newVariable = new Variable<float>();
                            var targetId = AddVariable(newVariable);
                            target = (targetId, newVariable);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            return true;
                        }if (type<BinaryOpType.BitwiseAnd) {
                            EmitData((ushort)type);
                            var newVariable = new Variable<bool>();
                            var targetId = AddVariable(newVariable);
                            target = (targetId, newVariable);
                            EmitData(target.id,left.id,right.id);
                            Emit(FloatArithmeticOp.Id);
                            
                            return true;
                        }
                    }
                    
                    target = default;
                    return true;
                }
            }
            target = default;
            return false;
        }
        
        public ushort AddVariable(Variable variable) => Engine.AddVariable(variable);
        public (ushort id,Variable variable) AddVariable(Type type) => Engine.AddVariable(type);
        public (ushort id,Variable variable) AddVariable<T>()=> Engine.AddVariable<T>();
        
        
        
        public ushort  AddBoxed(ushort src) {
            var target=Engine.AddVariable<object>();
            Engine.EmitData(target.Item1,src);
            Engine.Emit(Boxing.Id);
            return target.Item1;
        }

      
        public void Set(ushort id,Variable variable) {
            Engine.Variables[id] = variable;
        }

        public void Define(string name, ushort id) { 
            if (ConstNameSet.Contains(name)) {
                throw new Exception(name + " is already defined as const");
            }
            scope.Dictionary[name] = id;
        }
        public void Define(ReadOnlySpan<char> name, ushort id) { 
            if (ConstNameSet.Contains(name)) {
                throw new Exception(name.ToString() + " is already defined as const");
            }
            scope.Dictionary[name] = id;
        }
       
        public void DefineAsConst(ReadOnlySpan<char> name, ushort id) {
            var type=GetVariable(id).type;
            if (type.IsValueType && !type.IsPrimitive &&!type.IsEnum&& !type.IsDefined(typeof(IsReadOnlyAttribute), false)) {
                throw new Exception(name.ToString() +" "+type.Build()+ " is mutable struct so  this cannot be const");
            }
            ConstSet.Add(id);
            if (!ConstNameSet.Add(name.ToString())) {
                throw new Exception(name.ToString() + " is already defined as const");
            }
            scope.Dictionary[name] = id;
        }

        public bool IsConst(ushort id) => ConstSet.Contains(id);
        public bool IsConst((ushort id,Variable v) data) => ConstSet.Contains(data.id);

        public (ushort opId, ushort dataId) EnterToScope() {
            EmitData(0,0);
            scope = scope.EnterToScope();
            return (LastOpId, LastDataId);
        }

        public void ExitFromScope((ushort,ushort) enderData ) {
            
            ExitFromScope(enderData.Item1, enderData.Item2);
        }
         void ExitFromScope(ushort opId, ushort dataId ) {

            Engine.UnmanagedData[dataId-1] = (ushort)(LastOpId - opId);
            Engine.UnmanagedData[dataId] = (ushort)(LastDataId - dataId);

            scope=scope.ExitFromScope();
        }
        
        public ushort Get(string name) {
            return scope.Get(name);
        }
        public bool TryGet(string name,out ushort id) {
            return scope.TryGet(name,out  id);
        }
        public (ushort id ,Variable variable) Get(ReadOnlySpan<char> name) {
            var id = scope.Get(name);
            return (id,Engine.GetVariable(id));
        }
        public bool TryGet(ReadOnlySpan<char> name,out ushort id) {
            return scope.TryGet(name,out  id);
        }
        public bool TryGet(int hashCode,ReadOnlySpan<char> name,out ushort id) {
            return scope.TryGet(hashCode,name,out  id);
        }

        public static SimpleList<MethodData> Stack;
        public bool TryGetExFunction(Type type, ReadOnlySpan<char> methodName,  List<MethodData> methods) {
            var hash = methodName.GetExHashCode();
            ModuleLibrary.Root.Activate();
            var result =  ModuleLibrary.Root.TryGetExtensionMethods(type,hash,methodName,  methods);
            foreach (var nameSpace in NameSpaceNodes.AsSpan()) {
                nameSpace.Activate();
                if (nameSpace.TryGetExtensionMethods(type,hash,methodName,  methods)) {
                    result = true;
                }
            }
       
            return result;
        }
        
        public bool TryGetSomething(int hashCode,ReadOnlySpan<char> name,out (ushort,object) something) {
            if (scope.TryGet(hashCode,name, out var id)) {
                something= (id, GetVariable(id));
                return true;
            }
            if (GlobalModule.Instance.TryGetMember(hashCode,name, out var member)) {
                
                something= (0, member);
                return true;
            }
            
            foreach (var module in UsingStaticModules.AsSpan()) {
                if (module.TryGetMember(hashCode, name, out member)) {
                    if (member is Variable constant) {
                        something =(AddVariable(constant), constant);
                    }
                    else {
                        something= (0, member);
                    }
                    return true;
                }
            }
            
            foreach (var node in NameSpaceNodes.AsSpan()) {
                if (node.TryGetTypeOrNameSpace(hashCode, name,out var type, out var child)) {
                    if (type is not null) {
                        something =(0, type);
                        return true;
                    } 
                    if (child is not null) {
                        something =(0, child);
                        return true;
                    } 
                }
            }
            {
                if (ModuleLibrary.Root.TryGetTypeOrNameSpace(hashCode, name, out var type, out var child)) {
                    if (type is not null) {
                        something = (0, type);
                        return true;
                    }

                    if (child is not null) {
                        something = (0, child);
                        return true;
                    }
                }
            }
            
            something = default;
            return false;
        }

        
        public Type GetType(ReadOnlySpan<char> name) {
            var hashCode = name.GetExHashCode();
            if(ModuleLibrary.Root.TryGetType(hashCode, name,out var type)) {
                if (type is not null) {
                    return type;
                }
            }
            foreach (var node in NameSpaceNodes.AsSpan()) {
                if (node.TryGetType(hashCode, name,out  type)) {
                    if (type is not null) {
                        return type;
                    }
                }
            }
            throw new KeyNotFoundException(name.ToString());
        }
      

        public Variable GetVariable(ushort id) => Engine.GetVariable(id);
        
        
        public ushort LastVarId => Engine.LastVarId;
        public ushort LastOpId =>  Engine.LastOpId;
        public ushort LastDataId =>  Engine.LastDataId;
    }

    public class Scope {
        public Scope(StringDictionary<ushort> dictionary) {
            Dictionary = dictionary;
        }
        public Scope(){}
         Scope(Scope parent) {
            Dictionary = new StringDictionary<ushort>(16);
            _parent = parent;
            parent._child = this;
            _depth = parent._depth + 1;
        }

        public Scope EnterToScope() {
            if (_child == null) return new Scope(this);
            return _child;
        }
        public Scope ExitFromScope() {
            Dictionary.Clear();
            return _parent;
        }
        public ushort Get(string name) {
            if (TryGet(name, out var id)) {
                return id;
            }
            if (_parent == null) return 0;
            if (_parent.TryGet(name, out id)) {
                return id;
            }
            throw new KeyNotFoundException(name);
        }
        public ushort Get(ReadOnlySpan<char> name) {
            if (TryGet(name, out var id)) {
                return id;
            }
            if (_parent == null) return 0;
            if (_parent.TryGet(name, out id)) {
                return id;
            }
            throw new KeyNotFoundException(name.ToString());
        }
        public bool TryGet(string name,out ushort id) {
            if (Dictionary.TryGetValue(name,out  id)) {
                return true;
            }
            if (_parent != null) {
                return _parent.TryGet(name, out id);
            }
            
            return false;
        }
        public bool TryGet(ReadOnlySpan<char> name,out ushort id) {
            if (Dictionary.TryGetValue(name,out  id)) {
                return true;
            }
            if (_parent != null) {
                return _parent.TryGet(name, out id);
            }
            
            return false;
        }
        public bool TryGet(int hashCode,ReadOnlySpan<char> name,out ushort id) {
            if (Dictionary.TryGetValue(hashCode,name,out  id)) {
                return true;
            }
            if (_parent != null) {
                return _parent.TryGet(hashCode,name, out id);
            }
            
            return false;
        }
        public StringDictionary<ushort> Dictionary;
        Scope _parent;
        Scope _child;
         int _depth;
    }
}