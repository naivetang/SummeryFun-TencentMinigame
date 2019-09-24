using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UI;
using ETModel;
using UnityEngine;

namespace ETEditor
{
    [CanEditMultipleObjects, CustomEditor(typeof(UINextButton), true)]
    public class UINextButtonEditor : ButtonEditor
    {
        private SerializedProperty pressScaleProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            pressScaleProperty = this.serializedObject.FindProperty("pressScale");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(pressScaleProperty, new GUIContent("按下缩放值"));
        }
    }
}
