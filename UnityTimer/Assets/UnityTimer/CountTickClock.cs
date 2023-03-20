﻿using System;
using Primise4CSharp;
using UnityEngine;

namespace UnityTimer
{
    public class CountTickClock
    {
        private long _totalTick;
        private long _utilTime;
        private long _tickCount;
        private long _totalUpdateCount;

        private Action<long> _onUpdate;
        private readonly TickClock _tickClock;

        public CountTickClock (long tickCount , bool autoStart = false)
        {
            _tickCount = tickCount;
            _utilTime = tickCount / UnityTimerMgr.TICK_COUNT_PER_MS;
            _tickClock = new TickClock (Update , autoStart);
            _onUpdate = (v) => { };
        }

        public CountTickClock (long tickCount , Action<long> onUpdate , bool autoStart = false)
        {
            _onUpdate = onUpdate;
            _tickCount = tickCount;
            _utilTime = tickCount / UnityTimerMgr.TICK_COUNT_PER_MS;

            if ( _onUpdate == null )
            {
                _tickClock = new TickClock (default , autoStart);
                return;
            }
            _tickClock = new TickClock (Update , autoStart);
        }

        public void Update (long elapsedTick)
        {
            _totalTick += elapsedTick;
            long val = _totalTick / _tickCount;
            if ( val > 0 )
            {
                _totalUpdateCount++;
                _onUpdate.Invoke (_totalUpdateCount);
                _totalTick = 0;
            }
        }

        public void Start ()
        {
            _tickClock.Start ();
        }

        public void Pause ()
        {
            _tickClock.Pause ();
        }

        public void Stop ()
        {
            _tickClock.Stop ();
        }

        public Promise Wait (int utilCount)
        {
            Promise promise = new Promise ();
            void onUpdate (long v)
            {
                if ( _totalUpdateCount >= utilCount )
                {
                    promise.Resolve ();
                    _onUpdate -= onUpdate;
                }
            }
            _onUpdate += onUpdate;
            return promise;
        }
    }
}