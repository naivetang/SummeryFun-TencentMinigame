using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETModel;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ETEditor
{
    [CanEditMultipleObjects, CustomEditor(typeof(UIMultImage), true)]
    public class UIMultImageEditor : ImageEditor
    {
        private UIMultImage _target;
        private SerializedProperty maskableProperty;
        private SerializedProperty dataProperty;
        private SerializedProperty bNativeSize;


        protected virtual void OnEnable()
        {
            base.OnEnable();

            _target = target as UIMultImage;
            maskableProperty = this.serializedObject.FindProperty("needMask");
            this.dataProperty = this.serializedObject.FindProperty("sprites");
            bNativeSize = serializedObject.FindProperty("nativeSize");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            this.serializedObject.Update();
            EditorGUILayout.Space();

            EditorGUI.indentLevel = (EditorGUI.indentLevel + 1);
            EditorGUILayout.PropertyField(maskableProperty, new GUIContent("maskable"));
            EditorGUILayout.PropertyField(bNativeSize, new GUIContent("Use Native Size"));
            EditorGUILayout.PropertyField(this.dataProperty, new GUIContent("Sprite"), true);
            EditorGUI.indentLevel = (EditorGUI.indentLevel - 1);

            //		var sprites = _target.sprites;
            //		for (int i=0,len=sprites.Length;i<len;i++)
            //		{
            //			var sprite = sprites[i];
            //			EditorGUILayout.PropertyField(sprite, new GUIContent(i.ToString()));
            //		}



            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
