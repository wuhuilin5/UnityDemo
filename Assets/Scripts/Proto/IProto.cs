using System;
namespace Proto
{
	public interface IProto
	{
		string getTxtName();

		bool LoadFromJson(string jsonObject);
	}
}

