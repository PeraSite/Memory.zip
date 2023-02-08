using System;

namespace MemoryZip.Common {
	public static class StopwatchSystem {
		private static DateTime _startTime;

		public static void Start() {
			_startTime = DateTime.Now;
		}

		public static TimeSpan GetElapsed() {
			return (DateTime.Now - _startTime);
		}
	}
}
