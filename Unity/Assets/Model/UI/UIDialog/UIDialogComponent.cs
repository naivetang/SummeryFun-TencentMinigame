using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIDialogComponentSystem : AwakeSystem<UIDialogComponent>
    {
        public override void Awake(UIDialogComponent self)
        {
            self.Awake();
        }
    }


    public class UIDialogComponent : Component
    {
        private Text text;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.text = rc.Get<Text>("TextContext");
        }
        

        public void SetText(string context)
        {
            this.text.text = context;
        }
    }
}
