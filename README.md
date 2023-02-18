# YukataScript
dev branch is the newest.
Script Language for Unity/C#

No more allocations(including boxing) and typechecks than C# at run time.

Theoretically all types and methods can be used except ByReflike(such as Span\<T\>).

The writing style is a mixture of C# and go lang.


But it's in alpha and very buggy.
I need your help.
## Very easy async/await on Editor. 
![Async](/Images/YSAsync.gif)
## Good Performance.
![Performance](/Images/YSPerformance.gif)
## Easy code generation.
![TypeSelect](/Images/YSTypeSelect.gif)
## Field type selection.
![FieldTypeSelec](/Images/YSFieldTypeSelect.gif)
## Easy to save values.
![Json](/Images/YSJson.gif)
## Method with attribute can be called without using.
![Reflection](/Images/Reflection.png)

## Supported
- Extention methods.
- Any TaskLike works when it returns void.


## Not currently supported
- funcions and classes
- Generics (Manual setting may solve it.)
- Await with return value 
<<<<<<< HEAD
- Primitive Types  without int, float,double,char,string 
=======
- Primitive Types  without int, float,double,char,string 
>>>>>>> af4f4320699c70500d68c9e9a70677a1e6af05f8
