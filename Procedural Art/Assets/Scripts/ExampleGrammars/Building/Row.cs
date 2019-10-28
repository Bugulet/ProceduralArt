using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
	public class Row : Shape {
		public int Number;
		public GameObject[] prefabs=null;
        public GameObject[] doorPrefabs = null;
        public int[] pattern=null;
		public Vector3 direction;
        

		public void Initialize(int Number, GameObject[] prefabs, int[] pattern=null, Vector3 dir=new Vector3()) {
            
			this.Number=Number;
			// All public reference types must be cloned, to avoid unexpected shared reference errors when changing values in inspector:
			this.prefabs=(GameObject[])prefabs.Clone();
			if (pattern!=null)
				this.pattern=(int[])pattern.Clone();

			if (dir.magnitude!=0)
				direction=dir;
			else
				direction=new Vector3(0, 0, 1);
		}

        public void Initialize(int Number, GameObject[] prefabs,GameObject[] doors, int[] pattern = null, Vector3 dir = new Vector3())
        {

            this.Number = Number;

            doorPrefabs = doors;

            // All public reference types must be cloned, to avoid unexpected shared reference errors when changing values in inspector:
            this.prefabs = (GameObject[])prefabs.Clone();
            if (pattern != null)
                this.pattern = (int[])pattern.Clone();

            if (dir.magnitude != 0)
                direction = dir;
            else
                direction = new Vector3(0, 0, 1);
        }

        protected override void Execute() {
			if (Number<=0)
				return;
			BuildingParameters param = (BuildingParameters)parameters;
			if (pattern==null || pattern.Length==0) { // generate a pattern if needed
				pattern=new int[Number];
				for (int i=0;i<Number;i++) {
					if (param!=null && param.Rand!=null) {
						pattern[i]=param.Rand.Next(0, prefabs.Length);
					} else {
						pattern[i]=(int)(Random.value*prefabs.Length);
					}
				}
			}
            
			for (int i=0;i<Number;i++) {			// spawn the prefabs
				int index = pattern[i % pattern.Length] % prefabs.Length;

                //if (i == 0 || i==Number-1)
                //{
                //    continue;
                //}
                if (i == Number / 2 && doorPrefabs != null)
                {
                    int randomN = param.Rand.Next(0, doorPrefabs.Length);
                    SpawnPrefab(doorPrefabs[randomN],
                                            direction * (i - (Number - 1) / 2f),
                                            Quaternion.identity,
                                            transform
                                        );
                }
                else
                {
                    SpawnPrefab(prefabs[index],
                        direction * (i - (Number - 1) / 2f),
                        Quaternion.identity,
                        transform
                    );
                }
			}
		}
	}
}
