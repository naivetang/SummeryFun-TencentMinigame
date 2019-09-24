using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETModel
{
    public class UINextButton : Button
    {

        [SerializeField]
        private float pressScale = 1.2f;

        public int a;

        private float _targetScale = 1f;

        private Vector3 _originScale;

        private Transform _cacheTransform;

        private Transform cacheTransform
        {
            get
            {
                if (_cacheTransform == null)
                {
                    _cacheTransform = transform;
                    _originScale = _cacheTransform.localScale;
                }
                return _cacheTransform;
            }
        }


        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            this.PlayZoomAnimation();
            
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            SetScaleState(state);
        }

        private void SetScaleState(SelectionState state)
        {
            float scale = state == SelectionState.Pressed ? pressScale : 1;
            if (Mathf.Approximately(_targetScale, scale)) return;
            _targetScale = scale;
            cacheTransform.DOScale(_targetScale * _originScale, 0.15f);
        }

        void PlayZoomAnimation()
        {
            
        }
    }
}
