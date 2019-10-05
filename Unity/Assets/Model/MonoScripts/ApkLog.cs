using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ETModel
{
    public class logdata
    {
        public string output = "";
        public string stack = "";
        public static logdata Init(string o, string s)
        {
            logdata log = new logdata();
            log.output = o;
            log.stack = s;
            return log;
        }
        public void Show(GUIStyle style = null ,bool showstack = false)
        {
            
            GUILayout.Label(output,style);
            if (showstack)
                GUILayout.Label(stack,style);
        }
    }
    /// <summary>
    /// 手机调试脚本
    /// 本脚本挂在一个空对象或转换场景时不删除的对象即可
    /// 错误和异常输出日记路径 Application.persistentDataPath
    /// </summary>
    public class ApkLog : MonoBehaviour
    {

        List<logdata> logDatas = new List<logdata>();//log链表
        List<logdata> errorDatas = new List<logdata>();//错误和异常链表
        List<logdata> warningDatas = new List<logdata>();//警告链表

        static List<string> mWriteTxt = new List<string>();
        Vector2 uiLog;
        Vector2 uiError;
        Vector2 uiWarning;
        bool open = false;
        bool showLog = false;
        bool showError = false;
        bool showWarning = false;
        private string outpath;



        private GUIStyle ButtonStyle;

        private GUIStyle logTextStyle;
        private GUIStyle warringTextStyle;
        private GUIStyle errorTextStyle;
        
        void Start()
        {
            //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
            //Debug.Log(Application.persistentDataPath);
            outpath = Application.persistentDataPath + "/outLog.txt";
            //每次启动客户端删除之前保存的Log
            if (System.IO.File.Exists(outpath))
            {
                File.Delete(outpath);
            }
            //转换场景不删除
            Application.DontDestroyOnLoad(gameObject);

            this.logTextStyle = new GUIStyle();
            this.logTextStyle.fontSize = 30;
            this.logTextStyle.normal.textColor = Color.white;


            this.warringTextStyle= new GUIStyle();
            this.warringTextStyle.fontSize = 30;
            this.warringTextStyle.normal.textColor = Color.yellow;

            this.errorTextStyle = new GUIStyle();
            this.errorTextStyle.fontSize = 40;
            this.errorTextStyle.normal.textColor = Color.red;


            ButtonStyle = new GUIStyle();

            this.ButtonStyle.fontSize = 50;
        }
        void OnEnable()
        {
            //注册log监听
            Application.RegisterLogCallback(HangleLog);
        }

        void OnDisable()
        {
            // Remove callback when object goes out of scope
            //当对象超出范围，删除回调。
            Application.RegisterLogCallback(null);
        }
        void HangleLog(string logString, string stackTrace, UnityEngine.LogType type)
        {
            switch (type)
            {
                case UnityEngine.LogType.Log:
                    logDatas.Add(logdata.Init(logString, stackTrace));
                    break;
                case UnityEngine.LogType.Error:
                case UnityEngine.LogType.Exception:
                    errorDatas.Add(logdata.Init(logString, stackTrace));
                    mWriteTxt.Add(logString);
                    mWriteTxt.Add(stackTrace);
                    break;
                case UnityEngine.LogType.Warning:
                    warningDatas.Add(logdata.Init(logString, stackTrace));
                    break;
            }
        }
        void Update()
        {
            //因为写入文件的操作必须在主线程中完成，所以在Update中才给你写入文件。
            if (errorDatas.Count > 0)
            {
                string[] temp = mWriteTxt.ToArray();
                foreach (string t in temp)
                {
                    using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                    {
                        writer.WriteLine(t);
                    }
                    mWriteTxt.Remove(t);
                }
            }
        }

        private int width = 200;
        private int height = 150;
        
        void OnGUI()
        {

            return;

            GUILayout.BeginHorizontal();


            //GUI.skin.button.fontSize = 35;

            GUI.skin.button.normal.textColor = Color.white;


            if (GUILayout.Button(">>Open", GUILayout.Height(height), GUILayout.Width(width), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
                open = !open;
            if (open)
            {
                if (GUILayout.Button("Clear", GUILayout.Height(height), GUILayout.Width(width)))
                {
                    logDatas = new List<logdata>();
                    errorDatas = new List<logdata>();
                    warningDatas = new List<logdata>();
                }
                if (GUILayout.Button("Log:" + showLog, GUILayout.Height(height), GUILayout.Width(width)))
                {
                    showLog = !showLog;
                    if (open == true)
                        open = !open;
                }
                if (GUILayout.Button("Error:" + showError, GUILayout.Height(height), GUILayout.Width(width)))
                {
                    showError = !showError;
                    if (open == true)
                        open = !open;
                }
                if (GUILayout.Button("Warning:" + showWarning, GUILayout.Height(height), GUILayout.Width(width+30)))
                {
                    showWarning = !showWarning;
                    if (open == true)
                        open = !open;
                }
            }
            GUILayout.EndHorizontal();
            
            if (showLog)
            {
                
                // GUIStyle lable = new GUIStyle();
                //
                // lable.fontSize = 50;
                //
                // GUI.color = Color.white;
                //
                // GUI.backgroundColor = Color.blue;
                //
                // GUI.skin.label = lable;
                
                uiLog = GUILayout.BeginScrollView(uiLog);

                GUI.backgroundColor = Color.black;

                foreach (var va in logDatas)
                {
                    va.Show(this.logTextStyle);
                }
                GUILayout.EndScrollView();
            }
            if (showError)
            {

                uiError = GUILayout.BeginScrollView(uiError);
                foreach (var va in errorDatas)
                {
                    va.Show(this.errorTextStyle, true);
                }
                GUILayout.EndScrollView();
            }
            if (showWarning)
            {
                uiWarning = GUILayout.BeginScrollView(uiWarning);
                foreach (var va in warningDatas)
                {
                    va.Show(this.warringTextStyle);
                }
                GUILayout.EndScrollView();
            }
        }
    }
}
