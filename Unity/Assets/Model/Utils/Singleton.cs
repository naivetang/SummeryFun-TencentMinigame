using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 单例类
/// </summary>


namespace ETModel
{
    public class Singleton<T> where  T : class, new()
    {
        private static T _instance;

        private static void CreateInstance()
        {
            if (_instance == null)
            {
                _instance = new T();

                (_instance as Singleton<T>).Init();
            }
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateInstance();


                }

                return _instance;
            }
        }

        //--------------------------------------
        /// 返回单件实例
        //-------------------------------------- 
        public static T GetInstance()
        {
            if (_instance == null)
            {
                CreateInstance();
            }

            return _instance;
        }

        //--------------------------------------
        /// 删除单件实例
        //--------------------------------------
        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                (_instance as Singleton<T>).UnInit();
                _instance = null;
            }
        }

        //--------------------------------------
        /// 是否被实例化
        //-------------------------------------- 
        public static bool HasInstance()
        {
            return (_instance != null);
        }

        //--------------------------------------
        /// 初始化
        /// @需要在派生类中实现
        //-------------------------------------- 
        public virtual void Init()
        {

        }

        //--------------------------------------
        /// 反初始化
        /// @需要在派生类中实现
        //-------------------------------------- 
        public virtual void UnInit()
        {

        }
    }
}
