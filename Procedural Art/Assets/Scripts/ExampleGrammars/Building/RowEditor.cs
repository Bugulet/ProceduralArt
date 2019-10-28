using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Demo {
	[CustomEditor(typeof(Row))]
	public class RowEditor : Editor {
		public override void OnInspectorGUI() {
			Row targetRow = (Row)target;

			GUILayout.Label("Generated objects: "+targetRow.NumberOfGeneratedObjects);
			if (GUILayout.Button("Generate")) {
				targetRow.Generate(0.1f);
			}
			DrawDefaultInspector();
		}
	}
}