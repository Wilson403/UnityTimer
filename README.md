

#### Overview

Easy-to-use timer designed for Untiy

------

#### How to run the demo

* doule click on the Start.scene

------

#### How to use it

* You can create timers for any time unit

  ```c#
  public static CountTickClock CreateFrameClock (Action<long> onUpdate , bool autoStart = true)
  {
      return new CountTickClock (FRAME_TICK_COUNT , onUpdate , autoStart);
  }
  
  public static CountTickClock CreateSecondClock (Action<long> onUpdate , bool autoStart = true)
  {
      return new CountTickClock (SECOND_TICK_COUNT , onUpdate , autoStart);
  }
  ```

* Wait for the specified time and then execute

  ```c#
  UnityTimerMgr.CreateSecondClock ().Wait (10).Then (() => 
  {
      Debug.Log ("After 10 seconds");
  });
  ```

* **Refer to StartDemo.cs for more details**
