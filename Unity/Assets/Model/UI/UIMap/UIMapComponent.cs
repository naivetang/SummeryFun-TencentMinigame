using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{

    [ObjectSystem]
    public class UIMapComponentSystem : AwakeSystem<UIMapComponent,GameObject, TaskQueryRsp>
    {
        public override void Awake(UIMapComponent self,GameObject go, TaskQueryRsp pos)
        {
            self.Awake(go, pos);
        }
    }
    
    public class UIMapComponent : Component 
    {
        //private GameObject _joyStick;
        
        /// <summary>
        /// 触发某个行为：进场景
        /// </summary>
        //private GameObject _actionBtn;

        /// <summary>
        /// 对话提示
        /// </summary>
        //private GameObject _dialogBtn;


        private GameObject _player;

        private GameObject _bg;

        private GameObject _context;

        private CanFllowPigComponent _canFllowPigCom;

        private CancellationTokenSource cancel;

        private GameObject fllowPig;



        public void Awake(GameObject player, TaskQueryRsp taskQueryRsp)
        {
            AsyncAwake(player, taskQueryRsp).Coroutine();
            
            
        }

        async ETVoid AsyncAwake(GameObject player, TaskQueryRsp taskQueryRsp)
        {
            Vector3 oldPos = this.GetParent<UIBase>().GameObject.transform.position;

            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            this._context = rc.Get<GameObject>("Context");

            this._bg = rc.Get<GameObject>("BG");
            
            if (player == null)
            {
                _player = rc.Get<GameObject>("Player");
                
                _player.gameObject.SetActive(true);
                
                _bg.gameObject.SetActive(true);

                await timer.WaitAsync((long) (0.01f * 1000));
            }
            else
            {
                _player = player;
            }

            //_joyStick = rc.Get<GameObject>("JoyStick");

            //_actionBtn = rc.Get<GameObject>("ActionBtn");

            //_actionBtn.SetActive(false);

            //_dialogBtn = rc.Get<GameObject>("DialogBtn");


            //_dialogBtn.SetActive(false);

            //_player = rc.Get<GameObject>("Player");

            fllowPig = rc.Get<GameObject>("CanFllowPig");
            if (taskQueryRsp == null)
            {
                this.GetParent<UIBase>().GameObject.transform.position = new Vector3(oldPos.x, 2000, oldPos.y);
                this.PlayUpToDownAnimation();

                _canFllowPigCom = ComponentFactory.Create<CanFllowPigComponent, GameObject>(fllowPig);
            }
            else
            {
                HuiYi(taskQueryRsp).Coroutine();
            }

            


            

            this.cancel = new CancellationTokenSource();


            this.AsyncUpdate(this.cancel.Token).Coroutine();
        }

        private static List<UIAutoSetDepth> allDepths = new List<UIAutoSetDepth>();

        public void AddAutoDepth(UIAutoSetDepth a)
        {
            allDepths.Add(a);
        }

        public void RemoveDepth(UIAutoSetDepth a)
        {
            if (allDepths.Contains(a))
            {
                allDepths.Remove(a);
                
                this.cancel.Cancel();
                
                this.cancel.Dispose();
                
                this.cancel = new CancellationTokenSource();

                this.AsyncUpdate(this.cancel.Token).Coroutine();
            }
            
        }

        async ETVoid HuiYi(TaskQueryRsp taskQueryRsp)
        {

            this._player.transform.position = new Vector3((float) taskQueryRsp.PositionX, (float) taskQueryRsp.PositionY, 0);

            foreach (int doneTask in taskQueryRsp.DoneTasks)
            {
                if(doneTask < 0 || doneTask >= UIBookComponent.hadOpenPage.Count)
                    continue;
                UIBookComponent.hadOpenPage[doneTask] = true;
            }

            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync((long) (0.2f) * 1000);
            
            _canFllowPigCom = ComponentFactory.Create<CanFllowPigComponent, GameObject>(fllowPig);

            this.InitPlayer();
        }

        int SortRule(UIAutoSetDepth a, UIAutoSetDepth b)
        {
            return (int)(b.transform.position.y - a.transform.position.y);
        }

        async ETVoid AsyncUpdate(CancellationToken token)
        {
            TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();

            if (timerComponent == null)
            {
                return;
            }
            
            while (true)
            {
                await timerComponent.WaitAsync((long) 0.8f * 1000,token);


                if (this.IsDisposed)
                    return;
                
                this.UpdateRenderDepth();
            }
        }


        private EventProxy finalResult;
        
        void FinalResult(List<object> list)
        {
            allDepths.Clear();
        }

        public void UpdateRenderDepth()
        {
            allDepths.Sort(this.SortRule);

            int minDepth = 201;
            
            foreach (UIAutoSetDepth setDepth in allDepths)
            {
                if (setDepth != null&& setDepth.isActiveAndEnabled)
                {

                    try
                    {
                        setDepth.canvas.sortingOrder = minDepth;
                        ++minDepth;
                    }
                    catch (Exception e)
                    {
                        allDepths.Remove(setDepth);

                        Log.Error(e);
                    }
                    
                    
                }
            }
        }

        void PlayUpToDownAnimation()
        {
            this.GetParent<UIBase>().GameObject.transform.DOLocalMoveY(0, 4f).SetEase(Ease.Linear).OnComplete(this.AnimationComplte);
        }

        void AnimationComplte()
        {
            
            this._bg.SetActive(true);
            
            //this._joyStick.gameObject.GetComponent<CanvasGroup>().DOFade(1f, 1f);
            
            this.AddListener();

            this.InitPlayer();

            Game.EventSystem.Run(EventIdType.CGToHallFinish);
        }
        

        private void AddListener()
        {
            this.finalResult = new EventProxy(this.FinalResult);

            Game.EventSystem.RegisterEvent(EventIdType.FinalResult, this.finalResult);

            //Game.EventSystem.RegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));

            // _actionBtn.GetComponent<Button>().onClick.AddListener(() =>
            // {
            //     UIFactory.Create<UIDragonflyComponent>(ViewLayer.UIFullScreenLayer, UIType.UIDragonfly).Coroutine();
            // });
        }

        private void RemoveListener()
        {
            Game.EventSystem.UnRegisterEvent(EventIdType.FinalResult, this.finalResult);

            //Game.EventSystem.UnRegisterEvent(EventIdType.TriggerAera, new EventProxy(TriggerArea));
        }

        private void TriggerArea(List<object> obj)
        {
            // string action = obj[0] as string;
            // int triggerId = (int)obj[1];
            //
            // if(triggerId == 1 && action.Equals("Enter"))
            //     this._actionBtn.SetActive(true);
            //
            // if (triggerId == 1 && action.Equals("Exit"))
            //     this._actionBtn.SetActive(false);
        }

        

        private void InitPlayer()
        {
            
            
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            this._player.transform.SetParent(this._context.transform);

            unitComponent.MyUnit.AddComponent<UnitMoveComponent>();

            unitComponent.MyUnit.AddComponent<UnitCameraFollowComponent>();

            //
            // Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(IdGenerater.GenerateId(), this._player);
            //
            // unitComponent.MyUnit = unit;
            //
            // unit.AddComponent<UnitMoveComponent>();
        }

        public Vector2 GetForceGuidePos()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            return rc.Get<GameObject>("forceGuidePos").transform.position;
        }
       

        public override void Dispose()
        {
            base.Dispose();
            
            this._canFllowPigCom.Dispose();

            if (this.cancel != null)
            {
                this.cancel.Cancel();
                this.cancel.Dispose();
                this.cancel = null;
            }
            
            
            RemoveListener();
        }
    }
}
