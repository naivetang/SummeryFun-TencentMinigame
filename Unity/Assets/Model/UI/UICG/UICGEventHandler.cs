using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [Event(EventIdType.CGToHallFinish)]
    class UICGEventHandler : AEvent
    {
        public override void Run()
        {
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UICG);
        }
    }
}
