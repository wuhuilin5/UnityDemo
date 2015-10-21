using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo
{
    public sealed class ModelDefine
    {
        //装备部件名字
        public static readonly string PART_HEAD = "part_tb";
        public static readonly string PART_ARMOR = "part_jj";
        public static readonly string PART_WEAPON = "part_wq";
        public static readonly string PART_ARM = "part_sb";
        public static readonly string PART_SHOES = "part_xz";
        public static readonly string PART_SHOULDER = "part_xj";
        public static readonly string PART_LEG = "part_kz";
        public static readonly string PART_WING = "part_cb";

        private static readonly Dictionary<int, string> ModelSlotNameDict = new Dictionary<int, string>()
        {
            {AttrKeyDefine.ATTR_DISPLAYID_HEAD, PART_HEAD},
            {AttrKeyDefine.ATTR_DISPLAYID_ARMOR, PART_ARMOR},
            {AttrKeyDefine.ATTR_DISPLAYID_WEAPON, PART_WEAPON},
            {AttrKeyDefine.ATTR_DISPLAYID_ARM, PART_ARM},
            {AttrKeyDefine.ATTR_DISPLAYID_SHOES, PART_SHOES},
            {AttrKeyDefine.ATTR_DISPLAYID_SHOULDER, PART_SHOULDER},
            {AttrKeyDefine.ATTR_DISPLAYID_LEG, PART_LEG},
            {AttrKeyDefine.ATTR_DISPLAYID_WING, PART_WING}
        };

        /// <summary>
        /// 获取装备槽位名字
        /// </summary>
        /// <param name="slotId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetSlotName(int slotId, int index = 0)
        {
            return ModelSlotNameDict[slotId];
        }
    }
}
