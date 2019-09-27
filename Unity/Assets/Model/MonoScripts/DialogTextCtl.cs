using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    public class DialogTextCtl : UIBehaviour
    {
        private GameObject textGo;
        
        private CancellationTokenSource cancellationTokenSource; 

        void Awake()
        {
            this.textGo = this.GetComponent<ReferenceCollector>().Get<GameObject>("TextContext");

            if (this.textGo == null)
            {
                Log.Error("text is null, gameObject name is " + this.gameObject.name);
            }
        }
        
        public void SetText(string text, float closeTime)
        {
            
            this.gameObject.SetActive(true);

            this.textGo.GetComponent<Text>().text = text;

            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
            }
            cancellationTokenSource = new CancellationTokenSource();
            
            this.AsyncClose(closeTime).Coroutine();
        }

        async ETVoid AsyncClose(float closeTime)
        {
            TimerComponent timer = Game.Scene.GetComponent<TimerComponent>();

            await timer.WaitAsync((long) (closeTime * 1000), this.cancellationTokenSource.Token);
            
            this.cancellationTokenSource.Dispose();

            this.cancellationTokenSource = null;
            
            this.gameObject.SetActive(false);
        }
    }
}