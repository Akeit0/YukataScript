using System;
using System.Collections;
using YS.Collections;

namespace YS.Async {
    public interface IPooledCoroutine<T> :IEnumerator,IDisposable,IPoolNode<T>{ }
}