using System.Collections.Generic;
using UnityEngine;
using System;

namespace Demo {
	public class CleanUp : MonoBehaviour {
		// Add all the shape types here that you don't want to collapse:
		List<Type> noCollapse = new List<Type> { typeof(Row) };

		void Update() {
			if (Input.GetKeyDown(KeyCode.F1)) {
				RemoveShapeComponents(gameObject);
			}
			if (Input.GetKeyDown(KeyCode.F2)) {
				CollapseHierarchy(gameObject,transform);
			}
		}

		public void RemoveShapeComponents(GameObject current) {
			Shape shape = current.GetComponent<Shape>();
			DestroyImmediate(shape);
			// Continue recursively with children:
			for (int i=0;i<current.transform.childCount;i++) {
				RemoveShapeComponents(current.transform.GetChild(i).gameObject);
			}			
		}

		public void CollapseHierarchy(GameObject current, Transform root) {
			Shape shape = current.GetComponent<Shape>();
			// Change the parent to the parent of the parent,
			// for every game object that 
			//  (1) has a shape component that is not in the "no collapse" list, and
			//  (2) is not already at the root of the current shape hierarchy:
			if (shape!=null && !noCollapse.Contains(shape.GetType()) && 
				current.transform!=root && current.transform.parent!=root && current.transform.parent!=null) 
			{
				current.transform.SetParent(current.transform.parent.parent, true);
			}

			// Continue recursively with children:
			for (int i=0;i<current.transform.childCount;i++) {
				CollapseHierarchy(current.transform.GetChild(i).gameObject, root);
			}

			// Completely remove useless game objects:
			// (If you have game objects that have no renderer or collider component, 
			//  but that you don't consider useless, add those components to the if-condition!)
			if (current.transform.childCount==0 && 
				current.GetComponent<MeshRenderer>()==null &&
				current.GetComponent<Collider>()==null) 
			{
				DestroyImmediate(current);
			}
		}
	}
}