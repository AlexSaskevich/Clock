using System;

namespace Source.Code
{
    public class Clock
    {
        private DateTime _currentTime;

        public void SetTime(DateTime time)
        {
            _currentTime = time;
        }

        public DateTime GetTime()
        {
            return _currentTime;
        }
    }
}