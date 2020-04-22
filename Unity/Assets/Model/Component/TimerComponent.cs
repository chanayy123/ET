using System.Collections.Generic;
using System.Threading;

namespace ETModel
{
	public class Timer
	{
		public long Id { get; set; }
		public long Time { get; set; }
		public ETTaskCompletionSource tcs;
	}

    [ObjectSystem]
    public class TimerComponentAwakeSystem : AwakeSystem<TimerComponent>
    {
        public override void Awake(TimerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
	public class TimerComponentUpdateSystem : UpdateSystem<TimerComponent>
	{
		public override void Update(TimerComponent self)
		{
			self.Update();
		}
	}

	public class TimerComponent : Component
	{
        public static TimerComponent Instance { get; private set; }
        private readonly Dictionary<long, Timer> timers = new Dictionary<long, Timer>();
		/// <summary>
		/// key: time, value: timer id
		/// </summary>
		private readonly MultiMap<long, long> timeId = new MultiMap<long, long>();

		private readonly Queue<long> timeOutTime = new Queue<long>();
		
		private readonly Queue<long> timeOutTimerIds = new Queue<long>();

		// 记录最小时间，不用每次都去MultiMap取第一个值
		private long minTime;

        public void Awake()
        {
            Instance = this;
        }

        public void Update()
		{
			if (this.timeId.Count == 0)
			{
				return;
			}

			long timeNow = TimeHelper.Now();

			if (timeNow < this.minTime)
			{
				return;
			}
			
			foreach (KeyValuePair<long, List<long>> kv in this.timeId.GetDictionary())
			{
				long k = kv.Key;
				if (k > timeNow)
				{
					minTime = k;
					break;
				}
				this.timeOutTime.Enqueue(k);
			}

			while(this.timeOutTime.Count > 0)
			{
				long time = this.timeOutTime.Dequeue();
				foreach(long timerId in this.timeId[time])
				{
					this.timeOutTimerIds.Enqueue(timerId);	
				}
				this.timeId.Remove(time);
			}

			while(this.timeOutTimerIds.Count > 0)
			{
				long timerId = this.timeOutTimerIds.Dequeue();
				Timer timer;
				if (!this.timers.TryGetValue(timerId, out timer))
				{
					continue;
				}
                this.timers.Remove(timerId);
                timer.tcs.SetResult();
                this.RecycleTimer(timer);

			}
		}

		private void Remove(long id)
		{
            if (this.timers.TryGetValue(id, out Timer timer))
            {
                this.timers.Remove(id);
                this.RecycleTimer(timer);
            }
        }

		public ETTask WaitTillAsync(long tillTime, CancellationToken cancellationToken)
		{
            //ETTaskCompletionSource tcs = new ETTaskCompletionSource();
            //Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = tillTime, tcs = tcs };
            Timer timer = MakeTimer(tillTime);
            this.timers[timer.Id] = timer;
			this.timeId.Add(timer.Time, timer.Id);
			if (timer.Time < this.minTime)
			{
				this.minTime = timer.Time;
			}
			cancellationToken.Register(() => { this.Remove(timer.Id); });
			return timer.tcs.Task;
		}

		public ETTask WaitTillAsync(long tillTime)
		{
            //ETTaskCompletionSource tcs = new ETTaskCompletionSource();
            //Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = tillTime, tcs = tcs };
            Timer timer = MakeTimer(tillTime);
            this.timers[timer.Id] = timer;
			this.timeId.Add(timer.Time, timer.Id);
			if (timer.Time < this.minTime)
			{
				this.minTime = timer.Time;
			}
			return timer.tcs.Task;
		}

		public ETTask WaitAsync(long time, CancellationToken cancellationToken)
		{
            //ETTaskCompletionSource tcs = new ETTaskCompletionSource();
            //Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = TimeHelper.Now() + time, tcs = tcs };
            Timer timer = MakeTimer(TimeHelper.Now() + time);
            this.timers[timer.Id] = timer;
			this.timeId.Add(timer.Time, timer.Id);
			if (timer.Time < this.minTime)
			{
				this.minTime = timer.Time;
			}
			cancellationToken.Register(() => { this.Remove(timer.Id); });
			return timer.tcs.Task;
		}

		public ETTask WaitAsync(long time)
		{
            //ETTaskCompletionSource tcs = new ETTaskCompletionSource();
            //Timer timer = new Timer { Id = IdGenerater.GenerateId(), Time = TimeHelper.Now() + time, tcs = tcs };
            Timer timer = MakeTimer(TimeHelper.Now() + time);
            this.timers[timer.Id] = timer;
			this.timeId.Add(timer.Time, timer.Id);
			if (timer.Time < this.minTime)
			{
				this.minTime = timer.Time;
			}
			return timer.tcs.Task;
		}

        private Timer MakeTimer(long time)
        {
            Timer timer = SimplePool.Instance.Fetch<Timer>();
            timer.Id = IdGenerater.GenerateId();
            timer.Time = time;
            timer.tcs = timer.tcs ?? new ETTaskCompletionSource();
            return timer;
        }

        private void RecycleTimer(Timer t)
        {
            t.tcs.Reset();
            SimplePool.Instance.Recycle(t);
        }

	}
}