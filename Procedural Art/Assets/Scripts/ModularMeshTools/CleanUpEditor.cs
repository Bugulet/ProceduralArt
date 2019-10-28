using UnityEngine;
using UnityEditor;

namespace Demo {
	[CustomEditor(typeof(CleanUp))]
	public class CleanUpEditor : Editor {
		public override void OnInspectorGUI() {
			CleanUp targetCleaner = (CleanUp)target;

			if (GUILayout.Button("Collapse Hierarchy")) {
				targetCleaner.CollapseHierarchy(targetCleaner.gameObject,targetCleaner.transform);
			}
			if (GUILayout.Button("Remove Shape Components")) {
				targetCleaner.RemoveShapeComponents(targetCleaner.gameObject);
			}
			DrawDefaultInspector();
		}
	}
}