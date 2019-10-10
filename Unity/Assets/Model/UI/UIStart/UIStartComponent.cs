using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using Google.Protobuf.Collections;
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

        private Button RegistBtn;

        private Text ErrorTip;

        private TaskQueryRsp taskQueryRsp;

        private GameObject AboutPage;


        private bool isNewAcc;

        private readonly string ACCOUNT = "ACCOUNT";
        private readonly string PASSWORD = "ACCOUNT";

        public void Awake()
        {
            isNewAcc = true;

            ReferenceCollector rc = this.GetParent<UIBase>().GameObject.GetComponent<ReferenceCollector>();

            this.startCom = rc.Get<GameObject>("StartCom");
            
            this.startButton =  rc.Get<GameObject>("Start").GetComponent<Button>();

            this.recallButton = rc.Get<GameObject>("recall").GetComponent<Button>();

            this.setButton = rc.Get<GameObject>("setting").GetComponent<Button>();

            this.loginCom = rc.Get<GameObject>("LoginCom");
            
            this.AboutPage = rc.Get<GameObject>("AboutPage");
            
            this.AboutPage.SetActive(false);

            this.userName = rc.Get<GameObject>("Name").GetComponent<InputField>();

            this.passWord = rc.Get<GameObject>("Password").GetComponent<InputField>();
            
            this.loginButton = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            
            this.cancleLogin = rc.Get<GameObject>("CancleLogin").GetComponent<Button>();
            
            this.RegistBtn = rc.Get<GameObject>("RegistBtn").GetComponent<Button>();
            
            this.ErrorTip = rc.Get<GameObject>("ErrorTip").GetComponent<Text>();

            Session session = Game.Scene.GetComponent<NetOuterComponent>().Create(GlobalConfigComponent.Instance.GlobalProto.Address);

            Game.Scene.AddComponent<SessionComponent>().Session = session;

            this.Init();
            
            this.AddListener();
            
            this.InitUserNameAndPASSWORD();
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
            
            this.RegistBtn.onClick.AddListener(this.RegistBtnOnClick);
        }

        void StartButOnClick()
        {
            Log.Debug("startbtn click");

            UpdateTask(null, 0, 0);
            
            SetAlpha(this.startButton);

            this.GetParent<UIBase>().GameObject.GetComponent<CanvasGroup>().DOFade(0, 1f).OnComplete(() =>
            {
                Game.EventSystem.Run(EventIdType.EnterCG);
                
                this.Close();
            });

            
            
        }

        public static void UpdateTask(List<int> finish, double posx, double posy)
        {
            TaskUpdateReq req = new TaskUpdateReq();


            req.FinishedTaskIds = new RepeatedField<int>();

            if (finish != null)
            {
                foreach (int i in finish)
                {
                    req.FinishedTaskIds.Add(i);
                }
            }

            req.PositionX = posx;
            
            req.PositionY = posy;
            
            UpdateTask(req).Coroutine();
            
            
        }


        static async ETVoid UpdateTask(TaskUpdateReq req)
        {
            if (SessionComponent.Instance == null || SessionComponent.Instance.Session == null)
                return;
            
            TaskUpdateRsp rsp = (TaskUpdateRsp) await SessionComponent.Instance.Session.Call(req);

            if (rsp.Error == (int)TaskUpdateRsp.Types.ErrorCode.Succeed)
            {
                Log.Info("更新进度成功");
            }
            else
            {
                Log.Info("更新进度失败：" + rsp.Error);
            }
        }

        void InitUserNameAndPASSWORD()
        {
            string account = PlayerPrefs.GetString(this.ACCOUNT, "");
            string password = PlayerPrefs.GetString(this.PASSWORD, "");

            this.userName.text = account;

            this.passWord.text = password;
        }

        void SetUserNameAndPASSWORD()
        {
            PlayerPrefs.SetString(this.ACCOUNT, this.userName.text);
            PlayerPrefs.GetString(this.PASSWORD, this.passWord.text);
        }


        /// <summary>
        /// 回忆
        /// </summary>
        void RecallButOnClick()
        {
            Log.Debug("recall click");
            
            SetAlpha(this.recallButton);

            if (this.isNewAcc || 
                this.taskQueryRsp == null || 
                Math.Abs(this.taskQueryRsp.PositionX) + Math.Abs(this.taskQueryRsp.PositionY) < 0.2f)
            {
                ShowErrorTip("不存在历史记录");
            }

            else
            {
                RecallLoad().Coroutine();
            }
            
        }
        
        async ETVoid RecallLoad()
        {
            await UIFactory.Create<UIMapComponent, GameObject, TaskQueryRsp>(ViewLayer.UIMainLayer, UIType.UIMap, null, this.taskQueryRsp);

            await UIFactory.Create<UIMainComponent>(ViewLayer.UIFixedLayer, UIType.UIMain);

            Game.EventSystem.Run(EventIdType.ShowJoystic);

            this.Close();
        }



        void SettingButOnClick()
        {
            Log.Debug("setting click");
            
            SetAlpha(this.setButton);
            
            this.AboutPage.gameObject.SetActive(true);
        }

        async void RegistBtnOnClick()
        {
            if (string.IsNullOrEmpty(this.userName.text) || string.IsNullOrEmpty(this.passWord.text))
            {
               
                
                return;
            }

            if (SessionComponent.Instance == null || SessionComponent.Instance.Session == null)
                return;

            RegisterRsp rsp = (RegisterRsp) await SessionComponent.Instance.Session.Call(new RegisterReq() { Account = this.userName.text, Password = this.passWord.text });

            if (rsp.Error == (int)RegisterRsp.Types.ErrorCode.Succeed)
            {
                ShowErrorTip("注册成功");
            }
            else if (rsp.Error == (int)RegisterRsp.Types.ErrorCode.AccountAlreadyExist)
            {
                ShowErrorTip("此账号已被注册");
                
                
            }
            else
            {
                ShowErrorTip(rsp.Error.ToString());
            }

        }

        void ShowErrorTip(string text)
        {
            this.ErrorTip.text = text;

            this.ErrorTip.gameObject.SetActive(true);
        }

        // 查询此号的进度
        async void Schedule()
        {
            if (SessionComponent.Instance == null || SessionComponent.Instance.Session == null)
                return;

            TaskQueryRsp rsp =(TaskQueryRsp) await SessionComponent.Instance.Session.Call(new TaskQueryReq());

            if (rsp.Error != (int) TaskQueryRsp.Types.ErrorCode.Succeed)
            {
                //ShowErrorTip(rsp.Error.ToString());

                this.taskQueryRsp = null;
            }
            else
            {
                taskQueryRsp = rsp;
            }
        }

        async void LoginButOnClick()
        {
            // UIFactory.Create<UIShaddockSceneComponent>(ViewLayer.UIPopupLayer, UIType.UIShaddockScene).Coroutine();
            // return;

            //UIFactory.Create<UIPigSceneComponent>(ViewLayer.UIPopupLayer, UIType.UIPigScene);
            Log.Debug("login click");
            
            Log.Debug("用户名：" + this.userName.text);
            Log.Debug("密码：" + this.passWord.text);

            if (this.userName.text.Equals("root") && this.passWord.text.Equals("root"))
            {
                Log.Info("login succeed");
                this.loginCom.SetActive(false);
                this.startCom.SetActive(true);
                SetAlpha(this.startButton);

                SessionComponent.Instance = null;
                
                return;
            }

            if (SessionComponent.Instance == null || SessionComponent.Instance.Session == null)
                return;


            LoginRsp rsp = (LoginRsp) await SessionComponent.Instance.Session.Call(new LoginReq() { Account = this.userName.text, Password = this.passWord.text });
            
            if ( rsp.Error == (int)LoginRsp.Types.ErrorCode.Succeed)
            {
                Log.Info("login succeed");
                this.loginCom.SetActive(false);
                this.startCom.SetActive(true);
                SetAlpha(this.startButton);

                this.Schedule();
                
                this.SetUserNameAndPASSWORD();

                isNewAcc = false;
                //UIFactory.Create<UIShaddockSceneComponent>(ViewLayer.UIPopupLayer, UIType.UIShaddockScene).Coroutine();
                //UIFactory.Create<UIPigSceneComponent>(ViewLayer.UIPopupLayer, UIType.UIPigScene);
            }
            else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginNotRegistered)
            {
                this.ErrorTip.text = "未注册的账号";
                
                this.ErrorTip.gameObject.SetActive(true);
                
                Log.Warning("account not exist, please register new account");
            }
            else if (rsp.Error == (int)LoginRsp.Types.ErrorCode.LoginPasswordWrong)
            {
                this.ErrorTip.text = "密码错误";

                this.ErrorTip.gameObject.SetActive(true);

                Log.Warning("password wrong");
            }
            /* FIXME: 测试*/
            // RegisterHelper.OnRegisterAsync(session, this.userName.text, this.passWord.text).Coroutine();
            // LoginHelper.OnLoginAsync(session, this.userName.text, this.passWord.text).Coroutine();
            // TaskUpdateHelper.OnTaskUpdateAsync(session).Coroutine();
            // TaskQueryHelper.OnTaskQueryAsync(session).Coroutine();


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
