using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo.Interfaces
{
    public interface ICharManager
    {
        BaseData mainCharData { get; }
        void InitMainChar(object attr);
        object GetAttrValue(int key);
       
        void AddListener(int key, Action<int, object, object> listener);
        void RemoveListener(int key, Action<int, object, object> listener);
    }
}
