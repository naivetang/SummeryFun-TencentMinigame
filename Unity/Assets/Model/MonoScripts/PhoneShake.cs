using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class PhoneShake : UIBehaviour
    {
        void OnEnable()
        {
            VibrationControler.Vibrate();
        }
        
    }
}