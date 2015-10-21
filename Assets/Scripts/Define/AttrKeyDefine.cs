using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo
{
    public sealed class AttrKeyDefine
    {
        //data key 定义
        public static readonly int CLIENT_SELF_KEY_INDEX = 1000000;
        public static readonly int ATTR_ID = CLIENT_SELF_KEY_INDEX + 1;             //Int64     id
        public static readonly int ATTR_MODEL_SCALE = CLIENT_SELF_KEY_INDEX + 2;

        public static readonly int ATTR_CLASS = 1002;
        public static readonly int ATTR_LEVEL = 2001;


        public static readonly int ATTR_DISPLAYID_ENTITY = 3024;                            //实体模板
        public static readonly int ATTR_DISPLAYID_PART_MIN = ATTR_DISPLAYID_ENTITY + 2;     //部件开始
        public static readonly int ATTR_DISPLAYID_HEAD = 3026;                              //头盔
        public static readonly int ATTR_DISPLAYID_SHOULDER = 3028;                          //胸甲
        public static readonly int ATTR_DISPLAYID_ARM = 3030;                               //护腕
        public static readonly int ATTR_DISPLAYID_LEG = 3032;                               //脚部
        public static readonly int ATTR_DISPLAYID_SHOES = 3034;                             //鞋
        public static readonly int ATTR_DISPLAYID_WEAPON = 3036;                            //武器
        public static readonly int ATTR_DISPLAYID_WING = 3038;                              //翅膀
        public static readonly int ATTR_DISPLAYID_ARMOR = 3034;                             //肩甲
        public static readonly int ATTR_DISPLAYID_PART_MAX = ATTR_DISPLAYID_ARMOR;          //部件结束
    }   
}

