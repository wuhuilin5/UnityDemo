using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityDemo.interfaces;
using UnityDemo.interfaces.manager;
using UnityDemo.manager;

using UnityDemo.interfaces.api;
using UnityDemo.api;

namespace UnityDemo
{
    public class Globals
    {
        private IGlobalApi api;
        private static Globals instance;

        private Globals()
        {
            init();
        }

        private static Globals getIntance()
        {
            if (instance == null)
            {
                instance = new Globals();
            }

            return instance;
        }

        private void init()
        {
            if (api == null)
            {
                api = new GlobalApi();
                api.init();
            }
        }

        public static IGlobalApi Api
        {
            get
            {
                return getIntance().api;
            }
        }
    }
}
