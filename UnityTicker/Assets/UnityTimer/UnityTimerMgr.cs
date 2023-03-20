using System;

namespace UnityTimer
{
    public static class UnityTimerMgr
    {
        public const long TICK_COUNT_PER_MS = 10000;
        public const long SECOND_TICK_COUNT = 1000 * TICK_COUNT_PER_MS;
        public const long MIN_TICK_COUNT = 60 * SECOND_TICK_COUNT;
        public const long FRAME_TICK_COUNT = 15 * TICK_COUNT_PER_MS;

        public static CountTickClock CreateFrameClock (Action<long> onUpdate , bool autoStart = true)
        {
            return new CountTickClock (FRAME_TICK_COUNT , onUpdate , autoStart);
        }

        public static CountTickClock CreateSecondClock (Action<long> onUpdate , bool autoStart = true)
        {
            return new CountTickClock (SECOND_TICK_COUNT , onUpdate , autoStart);
        }

        public static CountTickClock CreateMinuteClock (Action<long> onUpdate , bool autoStart = true)
        {
            return new CountTickClock (MIN_TICK_COUNT , onUpdate , autoStart);
        }
    }
}