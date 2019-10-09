
using System;

namespace ETModel
{
    class VibrationControler : SingletonMono<VibrationControler>
    {
        protected virtual void OnDisable()
        {
            MMVibrationManager.iOSReleaseHaptics();
        }


        public static void Vibrate()
        {
            try
            {
                MMVibrationManager.Vibrate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
            
        }
    }
}
