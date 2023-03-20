using System;

namespace UnityTimer
{
    public class TickClock
    {
        private long _lastTick;
        private long _currentTick;
        private readonly Action<long> _onUpdate;
        private bool _isUpdate = false;

        public bool IsUpdate 
        {
            get 
            {
                return _isUpdate;
            }
        }

        public TickClock (Action<long> onUpdate , bool autoStart = false)
        {
            _onUpdate = onUpdate;
            _isUpdate = autoStart && _onUpdate != null;
            _lastTick = DateTime.Now.Ticks;
            TickerRuntime.Ins.AddTicker (this);
        }

        public void Update ()
        {
            if ( !_isUpdate )
            {
                return;
            }

            _currentTick = DateTime.Now.Ticks;
            _onUpdate.Invoke (_currentTick - _lastTick);
            _lastTick = _currentTick;
        }

        public void Start ()
        {
            _lastTick = DateTime.Now.Ticks;
            _isUpdate = _onUpdate != null;
        }

        public void Pause ()
        {
            _isUpdate = false;
        }

        public void Stop ()
        {
            TickerRuntime.Ins.RemoveTicker (this);
        }
    }
}