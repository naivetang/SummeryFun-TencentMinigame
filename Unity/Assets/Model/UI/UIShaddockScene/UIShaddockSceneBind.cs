using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{

    [Serializable]
    public struct Angle
    {
        [CustomLabel("旋转角度")]
        public float rotation;

        [CustomLabel("旋转速度")]
        public float speed;
    }
    public class UIShaddockSceneBind : UIBehaviour
    {

        [HideInInspector]
        public int angleIndex = 0;

        public List<Angle> Angles;

        
        [HideInInspector]
        public int speedIndex = 0;
        //[CustomLabel("打中柚子后的提升的速度,百分比")]
        public List<float> addSpped;
    }
}