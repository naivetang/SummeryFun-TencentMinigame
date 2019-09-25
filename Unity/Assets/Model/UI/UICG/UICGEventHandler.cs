using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [Event(EventIdType.CGToHallFinish)]
    class CGToHallFinish : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UICG);
        }
    }

    [Event(EventIdType.EnterCG)]
    class EnterCG : AEvent
    {
        public override void Run()
        {
            UIFactory.Create<UICGComponent>(ViewLayer.UIBgLayer, UIType.UICG).Coroutine();
        }
    }

}
