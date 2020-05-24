﻿using System.Collections.Generic;

namespace ETModel
{
	public class GateSessionKeyComponent : Component
	{
        public  const int KEY_VALID_TIME = 20000;
		private readonly Dictionary<string, int> sessionKey = new Dictionary<string, int>();
		
		public void Add(string key, int userId)
		{
			this.sessionKey.Add(key, userId);
            this.TimeoutRemoveKey(key);
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

		private async void TimeoutRemoveKey(string key)
		{
			await Game.Scene.GetComponent<TimerComponent>().WaitAsync(KEY_VALID_TIME);
			this.sessionKey.Remove(key);
		}
	}
}
