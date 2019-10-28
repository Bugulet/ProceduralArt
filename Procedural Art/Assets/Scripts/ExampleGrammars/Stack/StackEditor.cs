using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Demo {
	/// <summary>
	/// This editor script adds two buttons to the inspector of the Stack component, 
	/// which can be used to call generate and destroy methods.
	/// </summary>
	[CustomEditor(typeof(Stack))]
	public class StackEditor : Editor {
		public override void OnInspectorGUI() {
			Stack targetStack = (Stack)target;

			GUILayout.Label("Generated objects: "+targetStack.NumberOfGeneratedObjects);
			if (GUILayout.Button("Generate")) {
				targetStack.Generate(0.1f);
			}
			if (GUILayout.Button("Destroy")) {
				targetStack.DeleteGenerated();
			}
			DrawDefaultInspector();
		}
	}
}
