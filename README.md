# YukataScript
Demo  https://akeit0.github.io/YukataScript/

__Requires Unity 2021.3 or higher__

Script Language for Unity/C#

No more allocations(including boxing) and typechecks than C# at run time.

Theoretically all types and methods can be used except ByReflike(such as Span\<T\>).

The writing style is a mixture of C# and go lang.


__But it's in alpha and very buggy.__
I need your help.
## Very easy async/await on Editor. 
![Async](/Images/YSAsync.gif)
## *Exelent Performance*(I have achived 18ms on Editor and 7.4ms with IL2CPP while  C# on Editor takes 4.5ms to do same things. C# is a little faster ).
![Performance](/Images/YSPerformance.gif)
## Easy code generation.
![TypeSelect](/Images/YSTypeSelect.gif)
## Field type selection.
![FieldTypeSelec](/Images/YSFieldTypeSelect.gif)
## Easy to save values.
![Json](/Images/YSJson.gif)
## Method with attribute can be called without using.(This is in C#)
![Reflection](/Images/Reflection.png)

## Supported
- Extention methods.
- Any TaskLike works when it returns void.


## Not currently supported
- funcions and classes
- Generics (Manual setting may solve it.)
- Await with return value 
