using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class UIColliderTrigger : UIBehaviour
    {
        private Action<Collider2D> onTriggerEnter2D;
        private Action<Collider2D> onTriggerExit2D;
        private Action<Collider2D> onTriggerStay2D;

        public void RegistOnTriggerEnter2D(Action<Collider2D> p)
        {
            this.onTriggerEnter2D += p;
        }

        public void RegistOnTriggerExit2D(Action<Collider2D> p)
        {
            this.onTriggerExit2D += p;
        }

        public void RegistOnTriggerStay2D(Action<Collider2D> p)
        {
            this.onTriggerStay2D += p;
        }

        private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
        {
            if (this.onTriggerEnter2D != null)
                this.onTriggerEnter2D.Invoke(collision);
        }

        private void OnTriggerExit2D(UnityEngine.Collider2D collision)
        {
            if (this.onTriggerExit2D != null)
                this.onTriggerExit2D.Invoke(collision);
        }

        private void OnTriggerStay2D(UnityEngine.Collider2D collision)
        {
            if (this.onTriggerStay2D != null)
                this.onTriggerStay2D.Invoke(collision);
        }


    }
}
