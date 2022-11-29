using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    public static Action<int> ScoreUpdateEvent;
    public static Action PositionResetEvent;
    public static Action<float> BoostPowerupEvent;
    public static Action<float> ShieldPowerupEvent;
    public static Action GameOverEvent;
}
