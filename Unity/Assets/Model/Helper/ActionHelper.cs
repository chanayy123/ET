using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ETModel
{
	public static class ActionHelper
	{
		public static void AddListener(this Button btn, Action action)
		{
			btn.onClick.AddListener(()=> { action(); });
		}
        public static void RemoveListener(this Button btn)
        {
            btn.onClick.RemoveAllListeners();
        }
	}
}