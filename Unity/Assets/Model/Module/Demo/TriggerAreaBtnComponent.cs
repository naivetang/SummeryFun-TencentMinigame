using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class TriggerAreaBtnComponentAwakeSystem : AwakeSystem<TriggerAreaBtnComponent>
    {
        public override void Awake(TriggerAreaBtnComponent self)
        {
            self.Awake();
        }
    }

    // 所有的可触发事件的区域
    public class TriggerAreaBtnComponent : Component
    {
        private readonly Dictionary<long, TriggerAreaBtn> idTriggerAreaBtns = new Dictionary<long, TriggerAreaBtn>();

        public static TriggerAreaBtnComponent Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public void Add(TriggerAreaBtn TriggerAreaBtn)
        {
            this.idTriggerAreaBtns.Add(TriggerAreaBtn.config.Id, TriggerAreaBtn);
            //TriggerAreaBtn.Parent = this;
        }

        public TriggerAreaBtn Get(long id)
        {
            TriggerAreaBtn TriggerAreaBtn;
            this.idTriggerAreaBtns.TryGetValue(id, out TriggerAreaBtn);
            return TriggerAreaBtn;
        }

        public void Remove(long id)
        {
            this.idTriggerAreaBtns.Remove(id);
        }

        public int Count
        {
            get
            {
                return this.idTriggerAreaBtns.Count;
            }
        }

        public TriggerAreaBtn[] GetAll()
        {
            return this.idTriggerAreaBtns.Values.ToArray();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (TriggerAreaBtn TriggerAreaBtn in this.idTriggerAreaBtns.Values)
            {
                TriggerAreaBtn.Dispose();
            }

            Instance = null;
        }
    }
}
