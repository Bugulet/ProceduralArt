using UnityEngine;
using UnityEditor;

namespace Demo {
	[CustomEditor(typeof(BuildingPainter))]
	public class BuildingPainterEditor : Editor {
		// Note: OnSceneGUI is only called if this game object is selected in the scene view!
		public void OnSceneGUI() {
			BuildingPainter targetPainter = (BuildingPainter)target;

			Event e = Event.current;
			if (e.type==EventType.KeyDown && e.keyCode == KeyCode.Space) {
				Debug.Log("Space pressed - trying to create building");
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

				RaycastHit hit;
				if (Physics.Raycast(ray, out hit)) {
					Debug.Log("Placing building at "+hit.point);
					targetPainter.CreateHouse(hit.point);
				}
			}

			if (e.type==EventType.MouseDown) {
				Debug.Log("BuildingPainter selected - press space to place a building at the mouse position");
			}

		}
	}
}
