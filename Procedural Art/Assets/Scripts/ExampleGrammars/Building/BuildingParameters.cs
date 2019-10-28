using UnityEngine;

namespace Demo {
	public class BuildingParameters : MonoBehaviour {
		public int seed;
		public float buildDelay;
		public float RoofContinueChance;
		public float StockContinueChance;

        public ParameterContainer seedContainter;

        private void Update()
        {
            if (seedContainter != null)
            {
                if (seedContainter.Seed == 0)
                    seed =UnityEngine.Time.frameCount;
                else
                    seed = seedContainter.Seed;
            }
                
        }

        public int maxHeight;
        
        public GameObject[] wallStyle;
        public GameObject[] doorStyle;
        public int[] wallPattern;

		public GameObject[] roofStyle;

		System.Random rand=null;

		public System.Random Rand {
			get {
				if (rand==null) {
					rand=new System.Random(seed);
				}
				return rand;
			}
		}

		public void ResetRandom() {
			rand=new System.Random(seed);
		}
	}
}
