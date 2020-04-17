using System.Collections.Generic;

namespace ETModel
{
	public class GateSessionKeyComponent : Component
	{
		private readonly Dictionary<string, int> sessionKey = new Dictionary<string, int>();
		
		public void Add(string key, int userId)
		{
			this.sessionKey.Add(key, userId);
			this.TimeoutRemoveKey(key).Coroutine();
		}

		public int Get(string key)
		{
			this.sessionKey.TryGetValue(key, out int userId);
			return userId;
		}

		public void Remove(string key)
		{
			this.sessionKey.Remove(key);
		}

		private async ETVoid TimeoutRemoveKey(string key)
		{
			await Game.Scene.GetComponent<TimerComponent>().WaitAsync(20000);
			this.sessionKey.Remove(key);
		}
	}
}
