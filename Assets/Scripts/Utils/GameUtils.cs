using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UnityDemo.Utils
{
    public sealed class GameUtils
    {
        /// <summary>
        /// 获取孩子支点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform GetChild(Transform parent, string childName)
        {
            var child = parent.FindChild(childName);
            if (child == null)
            {
                foreach (Transform t in parent)
                {
                    child = GetChild(t, childName);
                    if (child != null)
                        return child;
                }
            }
            return child;
        }

    }
}
