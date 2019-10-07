using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIBookText : MonoBehaviour
    {
        [SerializeField]
        private Text details;

        [SerializeField]
        private Text title;


        public void Show(int index)
        {
            BookDetails config = Game.Scene.GetComponent<ConfigComponent>().Get(typeof (BookDetails), index) as BookDetails;

            if (config == null)
            {
                Log.Error("book不存在配置，id:" + index);
                return;
            }

            this.details.text = config.Text;

            this.title.text = config.Title;
        }


    }
}
