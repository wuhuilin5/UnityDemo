using System;

using UnityEngine;
using System.Collections.Generic;

namespace Proto
{
	public class IntegerList
	{
		public List<int> list;

		public static IntegerList WrapIntegerList(List<int> intList)
		{
			return new IntegerList
			{ 
				list = intList 
			};
		}
	}
}

