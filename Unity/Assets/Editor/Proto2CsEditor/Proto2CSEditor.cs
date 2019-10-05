using System.Diagnostics;
using ETModel;
using UnityEditor;
using UnityEngine;

namespace ETEditor
{
	internal class OpcodeInfo
	{
		public string Name;
		public int Opcode;
	}

	public class Proto2CSEditor: EditorWindow
	{
		[MenuItem("Tools/Proto2CS")]
		public static void AllProto2CS()
		{
			Process process = ProcessHelper.Run("dotnet", "Proto2CS.dll", "../Proto/", true);
			Log.Info(process.StandardOutput.ReadToEnd());
			AssetDatabase.Refresh();
		}
	}


    public class CreateCollider : EditorWindow
    {
        [MenuItem("Tools/生成Collider")]
        public static void Create()
        {
            GameObject[] gos = Selection.gameObjects;

            if(gos != null)
                foreach (GameObject gameObject in gos)
                {
                    BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();

                    if (collider == null)
                        collider = gameObject.AddComponent<BoxCollider2D>();

                    RectTransform transform = gameObject.transform as RectTransform;
                    
                    if (collider != null && transform != null)
                    {
                        collider.size = transform.sizeDelta;
                    }
                }
        }
    }
}
