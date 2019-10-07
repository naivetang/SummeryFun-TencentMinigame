using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIBookPrompt : MonoBehaviour
    {

        [SerializeField]
        private int pageIndex;

        private UIMultImage image;

        private bool finish;

        private Button btn;

        private UIBookComponent uibook;
        void Awake()
        {
            this.image = this.GetComponent<ReferenceCollector>().Get<GameObject>("image").GetComponent<UIMultImage>();

            this.btn = this.GetComponent<UINextButton>();
            
            this.btn.onClick.AddListener(this.BtnClick);

            uibook = Game.Scene.GetComponent<UIComponent>().GetUI(UIType.UIBook).GetComponent<UIBookComponent>();

            this.finish = this.uibook.hasOpenedPage(this.pageIndex);
        }

        private void Start()
        {
            this.image.SetSprite(this.finish ? 1 : 0);
        }

        void BtnClick()
        {

            if (this.finish)
            {
                uibook.CutPage(this.pageIndex);
            }
            else
            {
                uibook.ShowNotFinishTip();
            }
        }



    }
}
