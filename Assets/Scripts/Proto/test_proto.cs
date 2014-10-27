using LitJson;
using UnityEngine;
using System;
using System.Collections.Generic;

using System.Collections;

namespace Proto
{
	[Serializable]
	public class test_proto : ScriptableObject, IProto
	{
		public delegate void ListItemCall(test_proto_item item);

		public Dictionary<string, test_proto_item> table = new Dictionary<string, test_proto_item>();

		public List<test_proto_item> _table = new List<test_proto_item>();

		public int count
		{
			get 
			{ 
				return _table.Count; 
			}
		}

		public void ListItem(test_proto.ListItemCall Func)
		{
			foreach(test_proto_item item in _table)
			{
				Func(item);
			}
		}

		public test_proto_item Last()
		{
			if(_table.Count != 0 )
			{
				return this._table[_table.Count-1];
			}
			return null;
		}

		public test_proto_item First()
		{
			if(_table.Count != 0 )
			{
				return this._table[0];
			}

			return null;
		}

		public test_proto_item Find(string key)
		{
			if( key == null || key == string.Empty)
			{
				return null;
			}

			ToDictionary();
			if(!table.ContainsKey(key))
			{
				return null;
			}
			return table[key];
		}
	
		public string getTxtName()
		{
			return "test_proto.txt";
		}

		public bool LoadFromJson(string jsonObject)
		{
			JsonData jsonData = JsonMapper.ToObject(jsonObject);
			_table.Clear();
			table.Clear();

			using(IEnumerator<string> enumerator = jsonData.Keys.GetEnumerator())
			{
				while(enumerator.MoveNext())
				{
					JsonData json_key = enumerator.Current;
					string key = json_key.ToString();
					JsonData data = jsonData[key];
					this._table.Add(new test_proto_item
					{
						id = key,
						name = JsonFiledHelper.ToString(data["name"])
					});
				}
			}

			return true;
		}

		public void ToDictionary()
		{
			if(_table.Count != table.Count)
			{
				this.table.Clear();
				foreach(test_proto_item item in _table)
				{
					this.table[item.id] = item;
				}
			}
		}
	}
}
