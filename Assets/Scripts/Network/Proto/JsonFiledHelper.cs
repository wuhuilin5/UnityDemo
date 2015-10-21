using LitJson;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Proto
{
	public class JsonFiledHelper
	{
		public static float ToFloat(JsonData jsondata)
		{
			return (float)Convert.ToDouble(jsondata.ToString());
		}

		public static string ToString(JsonData jsondata)
		{
			return jsondata.ToString();
		}

		public static int ToInt(JsonData jsondata)
		{
			return Convert.ToInt32(jsondata.ToString());
		}

		public static List<int> ToIntList(JsonData jsondata)
		{
			List<int> list = new List<int>();
			if(jsondata == null){
				return list;
			}

			if(jsondata.IsInt)
			{
				list.Add(JsonFiledHelper.ToInt(jsondata));
			}else
			{
				for(int i = 0; i < jsondata.Count; i++ )
				{
					list.Add(JsonFiledHelper.ToInt(jsondata[i]));
				}
			}
			return list;
		}

		public static List<IntegerList> ToIntListList(JsonData jsondata)
		{
			List<IntegerList> list = new List<IntegerList>();
			if(jsondata == null){
				return list;
			}

			if(jsondata.IsArray)
			{
				for(int i = 0; i < jsondata.Count; i++ )
				{
					JsonData subList = jsondata[i];
					if(subList.IsArray)
					{
						List<int> intList = JsonFiledHelper.ToIntList(subList);
						list.Add(IntegerList.WrapIntegerList(intList));
					}
					else
					{
						list.Add(null);
					}
				}
			}
			return list;
		}
	}
}

