using UnityEngine;

namespace Demo {

    [ExecuteInEditMode]
	public class BuildTrigger : MonoBehaviour {
		public KeyCode BuildKey;

		Shape Root;
		BuildingParameters parameters;

		void Start() {
			Root=GetComponent<Shape>();
			parameters=GetComponent<BuildingParameters>();
		}

        public void Generate()
        {
            if (parameters != null)
            {
                parameters.ResetRandom();
            }
            if (Root != null)
            {
                Root.Generate();
            }
        }

		void Update() {
			if (Input.GetKeyDown(BuildKey)) {
                Generate();
			}
		}
	}
}