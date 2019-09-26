using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETModel
{
    [ObjectSystem]
    public class WindowComponentAwakeSystem: AwakeSystem<WindowComponent>
    {
        public override void Awake(WindowComponent self)
        {
            self.Awake();
        }
    }
    /// <summary>
    /// windowName映射到window对应的UI类
    /// </summary>
    public class WindowComponent : Component
    {
        //private Dictionary<string, Func<Component>> windos = new Dictionary<string, Func<Component>>();
        private Dictionary<string, Type> windos = new Dictionary<string, Type>();
        
        public static WindowComponent Instance { get; set; }
        
        public void Awake()
        {
            Instance = this;
            
            RegistWindowCom(UIType.UIGuideScene, typeof(UIGuideSceneComponent));
        }

        public Type GetWindowCom(string str)
        {
            Type com;
        
            this.windos.TryGetValue(str, out com);
        
            return com;
        }
        
        public void RegistWindowCom(string win, Type com)
        {
            this.windos.Add(win,com);
        }
        
    }
}
