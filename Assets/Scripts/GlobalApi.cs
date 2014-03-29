using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityDemo.interfaces.api;
using UnityDemo.interfaces.manager;
using UnityDemo.manager;

namespace UnityDemo.api
{
    public class GlobalApi:IGlobalApi
    {
        public void init()
        {
        }

        public ILoadFileManager loadFileManager { set; get; }
        public ILoadManger loadManager { set; get; }
    }
}
