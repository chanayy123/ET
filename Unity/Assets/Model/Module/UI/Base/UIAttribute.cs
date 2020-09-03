using System;

namespace ETModel
{
	[AttributeUsage(AttributeTargets.Class)]
	public class UIAttribute : BaseAttribute
	{
		public string Type { get; }

		public UIAttribute(string type)
		{
			this.Type = type;
		}
	}
}