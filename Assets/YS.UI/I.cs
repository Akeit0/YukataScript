using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
namespace YS.UI {
    public readonly struct I:IEnumerable<VisualElement> {
        public readonly VisualElement self;
        public I(VisualElement element) => self = element;
        public I(I element) => self = element.self;

        public IStyle style => self.style;
        public void Add(VisualElement child) => self.Add(child);
        public IEnumerator<VisualElement> GetEnumerator() {
            return self.Children().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

      
    }
    public readonly struct I<T>:IEnumerable<VisualElement> where T:VisualElement{
        public readonly T self;
        public I(T element) => self = element;
        public I(I<T> element) => self = element.self;
        public IStyle style => self.style;
      
        public void Add(VisualElement child) => self.Add(child);
        public IEnumerator<VisualElement> GetEnumerator() {
            return self.Children().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}