using System.Collections.Generic;

namespace UnityTimer
{
    public class TickerRuntime : SingletonMonoBehaviour<TickerRuntime>
    {
        private readonly List<TickClock> _tickClocks = new List<TickClock> ();

        public void AddTicker (TickClock tickClock)
        {
            if ( tickClock == null )
            {
                return;
            }

            if ( _tickClocks.Contains (tickClock) )
            {
                return;
            }

            _tickClocks.Add (tickClock);
        }

        public void RemoveTicker (TickClock tickClock)
        {
            _tickClocks.Remove (tickClock);
        }

        void Update ()
        {
            for ( int i = 0 ; i < _tickClocks.Count ; i++ )
            {
                _tickClocks [i].Update ();
            }
        }
    }
}