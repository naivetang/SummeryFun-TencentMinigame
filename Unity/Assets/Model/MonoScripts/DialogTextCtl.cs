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

        private GameObject hasNext;

        void Awake()
        {
            this.textGo = this.GetComponent<ReferenceCollector>().Get<GameObject>("TextContext");
            
            this.hasNext = this.GetComponent<ReferenceCollector>().Get<GameObject>("hasNext");

            if (this.textGo == null)
            {
                Log.Error("text is null, gameObject name is " + this.gameObject.name);
            }
        }

        public void CloseDialog(float delay = 0)
        {
            if (this.gameObject.activeSelf)
            {
                
                if(this.cancellationTokenSource != null)
                {
                    this.cancellationTokenSource.Cancel();

                    this.cancellationTokenSource = null;
                }
                this.cancellationTokenSource = new CancellationTokenSource();
                
                AsyncClose(delay).Coroutine();
                
            }
        }
        
        public void SetText(string text, float closeTime, bool hasNextDialog = false)
        {
            
            this.gameObject.SetActive(true);

            this.textGo.GetComponent<Text>().text = text;

            this.hasNext.SetActive(hasNextDialog);


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

            if(this.textGo ==null )
            {
                return;
            }

            this.cancellationTokenSource.Dispose();

            this.cancellationTokenSource = null;
        
            this.gameObject.SetActive(false);            
        }
    }
}