using UnityEngine;

namespace ETModel
{
    public class AnimationEvent : MonoBehaviour
    {
        private void SendAnimationEventMessage(string msg)
        {
            Game.EventSystem.Run(msg);
        }
    }
}