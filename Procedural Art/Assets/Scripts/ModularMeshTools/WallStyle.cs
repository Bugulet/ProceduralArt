using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
	[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WallStyle", order = 1)]
	public class WallStyle : ScriptableObject { 
		public GameObject[] wallParts;
	}
}