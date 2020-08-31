
using System;
using UnityEngine;

namespace ETModel
{
    class VibrationControler : SingletonMono<VibrationControler>
    {
        public static void Vibrate()
        {
            Handheld.Vibrate();
        }
    }
}
