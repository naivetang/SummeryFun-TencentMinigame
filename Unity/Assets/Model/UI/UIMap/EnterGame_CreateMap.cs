using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    [Event(EventIdType.EnterGame)]
    public class EnterGame_CreateMap : AEvent
    {
        public override void Run()
        {
            //UIFactory.Create<UIMapComponent>(ViewLayer.UIMainLayer, UIType.UIHall).Coroutine();

            //UIFactory.Create<UIStartComponent>(ViewLayer.UIFullScreenLayer, UIType.UIStart).Coroutine();

            //UIFactory.Create<UIGuideSceneComponent>(ViewLayer.UIFullScreenLayer, UIType.UIGuideScene).Coroutine();

            UIFactory.Create<UIStartComponent>(ViewLayer.UIPopupLayer, UIType.UIStart).Coroutine();
        }
    }


    [Event(EventIdType.CGFinish)]
    public class CGFinish_CreateHall : AEvent<GameObject>
    {

        public override void Run(GameObject a)
        {
            UIFactory.Create<UIMapComponent, GameObject>(ViewLayer.UIMainLayer, UIType.UIMap, a).Coroutine();

            UIFactory.Create<UIMainComponent>(ViewLayer.UIFixedLayer, UIType.UIMain).Coroutine();
        }
    }
}
