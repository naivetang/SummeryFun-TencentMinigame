using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    public class UIMultImage : Image
    {
        [SerializeField] private bool nativeSize;
        [SerializeField] private bool needMask = false;

        public Sprite[] sprites;


        protected override void Awake()
        {
            base.Awake();
            maskable = needMask;
        }

        /// <summary>
        /// 设置状态 0-n
        /// </summary>
        /// <param name="index"></param>
        public void SetSprite(int index)
        {
            if (index >= 0 && index < sprites.Length)
            {
                this.sprite = sprites[index];
                if (nativeSize)
                    SetNativeSize();
            }
        }
    }
}
