using UnityEngine;

namespace Demo {
	public class BuildingPainter : MonoBehaviour {
		public GameObject HousePrefab;
		public int Width;
		public int Depth;
		public WallStyle[] wallStyles;
		public GameObject[] roofStyle;

		// For placing buildings in play mode:
		void Update() {
			if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) {
				RaycastHit hit;
				if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
					CreateHouse(hit.point);
				}
			}
		}

		public void CreateHouse(Vector3 position, Quaternion orientation=new Quaternion()) {
			if (Width==0 || Depth==0)
				return;

			GameObject house = Instantiate(HousePrefab);
			house.transform.localPosition = position;
			house.transform.localRotation = orientation;

			var houseBuilder = house.GetComponent<Stock>();
			houseBuilder.Width=Width; 
			houseBuilder.Depth=Depth; 

			// Choose random wall style:
			var param = house.GetComponent<BuildingParameters>();
			param.wallStyle=wallStyles[(int)(Random.value * wallStyles.Length)].wallParts;
			param.seed=(int)(Random.value*1000000);

			param.roofStyle=roofStyle;

			houseBuilder.Generate();
		}
	}
}