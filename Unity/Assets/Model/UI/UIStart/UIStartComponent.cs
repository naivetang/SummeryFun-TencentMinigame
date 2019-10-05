using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [ObjectSystem]
    public class UIStartAwakeSystem : AwakeSystem<UIStartComponent>
    {
        public override void Awake(UIStartComponent self)
        {
            self.Awake();
        }
    }

    public class UIStartComponent : Component
    {
        private GameObject startCom;

        
        
        private Button startButton;
        private Button recallButton;
        private Button setButton;

        private GameObject loginCom;

        private InputField userName;
        
        private InputField passWord;

        private Button loginButton;

        private Button cancleLogin;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.startCom = rc.Get<GameObject>("StartCom");
            
            this.startButton =  rc.Get<GameObject>("Start").GetComponent<Button>();

            this.recallButton = rc.Get<GameObject>("recall").GetComponent<Button>();

            this.setButton = rc.Get<GameObject>("setting").GetComponent<Button>();

            this.loginCom = rc.Get<GameObject>("LoginCom");

            this.userName = rc.Get<GameObject>("Name").GetComponent<InputField>();

            this.passWord = rc.Get<GameObject>("Password").GetComponent<InputField>();
            
            this.loginButton = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            
            this.cancleLogin = rc.Get<GameObject>("CancleLogin").GetComponent<Button>();
            

            this.Init();
            
            this.AddListener();
        }

        void Init()
        {
            //this.passWord.contentType = InputField.ContentType.Password;

            SetAlpha(this.startButton);
        }

        void SetAlpha(Button button)
        {
            this.startButton.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
            this.recallButton.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
            this.setButton.GetComponent<CanvasRenderer>().SetAlpha(0.5f);
            
            button.GetComponent<CanvasRenderer>().SetAlpha(1f);
        }

        void AddListener()
        {
            this.startButton.onClick.AddListener(this.StartButOnClick); 
            
            this.recallButton.onClick.AddListener(this.RecallButOnClick);
            
            this.setButton.onClick.AddListener(this.SettingButOnClick);
            
            this.loginButton.onClick.AddListener(this.LoginButOnClick);
            
            this.cancleLogin.onClick.AddListener(this.CancleButOnClick);
        }

        void StartButOnClick()
        {
            Log.Debug("startbtn click");
            
            SetAlpha(this.startButton);

            this.GetParent<UIBase>().GameObject.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                Game.EventSystem.Run(EventIdType.EnterCG);
                
                this.Close();
            });

        }
        
        

        void RecallButOnClick()
        {
            Log.Debug("recall click");
            
            SetAlpha(this.recallButton);
        }

        void SettingButOnClick()
        {
            Log.Debug("setting click");
            
            SetAlpha(this.setButton);
            
        }

        async void LoginButOnClick()
        {
            Log.Debug("login click");
            
            Log.Debug("用户名：" + this.userName.text);
            Log.Debug("密码：" + this.passWord.text); 
            // Session session = Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);
            //
            // LoginRsp rsp = (LoginRsp) await session.Call(new LoginReq() { Account = this.userName.text, Password = this.passWord.text });
            // if (rsp.Error == (int)LoginRsp.Types.ErrorCode.Succeed)
            // {
            //     Log.Info("login succeed");
            //     this.loginCom.SetActive(false);
            //     this.startCom.SetActive(true);
            //     SetAlpha(this.startButton);
            //     this.userName.text = "";
            //     this.passWord.text = "";
            // }
            // else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginNotRegistered)
            // {
            //     Log.Warning("account not exist, please register new account");
            // }
            // else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginPasswordWrong)
            // {
            //     Log.Warning("password wrong");
            // }
            /* FIXME: 测试*/
            // RegisterHelper.OnRegisterAsync(session, this.userName.text, this.passWord.text).Coroutine();
            // LoginHelper.OnLoginAsync(session, this.userName.text, this.passWord.text).Coroutine();
            // TaskUpdateHelper.OnTaskUpdateAsync(session).Coroutine();
            // TaskQueryHelper.OnTaskQueryAsync(session).Coroutine();
            
            this.loginCom.SetActive(false);
            this.startCom.SetActive(true);
            SetAlpha(this.startButton);
            this.userName.text = "";
            this.passWord.text = "";

            //UIFactory.Create<UIShaddockSceneComponent>(ViewLayer.UIPopupLayer, UIType.UIShaddockScene).Coroutine();

        }

        void CancleButOnClick()
        {
            Log.Debug("Cancle click");


            
            
            this.startCom.SetActive(false);
            
            this.loginCom.SetActive(true);
        }

        void Close()
        {
            Game.Scene.GetComponent<UIComponent>().RemoveUI(UIType.UIStart);
        }

    }
}
