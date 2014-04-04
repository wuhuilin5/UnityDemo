﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityDemo.interfaces.manager;

namespace UnityDemo.interfaces.api
{
    public interface IGlobalApi
    {
		void init();
		
        ILoadFileManager loadFileManager { set; get; }
        ILoadManger loadManager { set; get; }
    }
}