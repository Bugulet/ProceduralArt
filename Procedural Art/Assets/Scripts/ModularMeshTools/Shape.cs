using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Demo {
	/// <summary>
	/// The base class for any shape grammar symbol.
	/// Has some useful methods (CreateSymbol and SpawnPrefab, for non-terminal and terminal symbols resp.).
	/// Keeps track of spawned objects, and can delete them for you.
	/// Can also be used for a nice delayed spawn visualization (in play mode) - call the Generate method with a non-zero delay value.
	/// 
	/// For child classes (=grammar non-terminal symbols), you only need to implement the Execute method. 
	/// This should select and apply one of the rules for the symbol.
	/// </summary>
	public abstract class Shape : MonoBehaviour {
		public int NumberOfGeneratedObjects {
			get {
				if (generatedObjects!=null)
					return generatedObjects.Count;
				else
					return 0;
			}
		}
		List<GameObject> generatedObjects = null;

		protected Component parameters=null;

		// A utility method for creating (child) game objects with a Shape component:
		protected T CreateSymbol<T>(string name, Vector3 localPosition=new Vector3(), Quaternion localRotation=new Quaternion(), Transform parent=null) where T : Shape {
			if (parent==null) {
				parent=transform; // default: add as child game object
			}
			GameObject newObj = new GameObject(name);
			newObj.transform.parent=parent;
			newObj.transform.localPosition=localPosition;
			newObj.transform.localRotation=localRotation;
			AddGenerated(newObj);
			T component = newObj.AddComponent<T>();
			component.parameters = parameters;
			return component;
		}

		// A utility method for spawning prefabs:
		protected GameObject SpawnPrefab(GameObject prefab, Vector3 localPosition=new Vector3(), Quaternion localRotation=new Quaternion(), Transform parent=null) {
			if (parent==null) {
				parent=transform; // default: add as child game object
			}
			GameObject copy = Instantiate(prefab, parent);
			copy.transform.localPosition=localPosition;
			copy.transform.localRotation=localRotation;
			AddGenerated(copy);
			return copy;
		}

		protected GameObject AddGenerated(GameObject newObject) {
			if (generatedObjects==null) {
				generatedObjects=new List<GameObject>();
			}
			generatedObjects.Add(newObject);
			return newObject;
		}

		public void Generate(float delaySeconds = 0) {
			DeleteGenerated();
			if (delaySeconds==0 || !Application.isPlaying) {
				Execute();
			} else {
				StartCoroutine(DelayedExecute(delaySeconds));
			}
		}

		IEnumerator DelayedExecute(float delay) {
			yield return new WaitForSeconds(delay);
			Execute();
		}

		public void DeleteGenerated() {
			if (generatedObjects==null)
				return;
			foreach (GameObject gen in generatedObjects) {
				if (gen==null)
					continue;
				// Delete recursively: (needed for when it's not a child of this game object)
				Shape shapeComp = gen.GetComponent<Shape>();
				if (shapeComp!=null)
					shapeComp.DeleteGenerated();

				DestroyImmediate(gen);
			}
			generatedObjects.Clear();
		}

		// Implement this method in child classes (apply the grammar rule and/or spawn prefabs):
		protected abstract void Execute();		
	}
}