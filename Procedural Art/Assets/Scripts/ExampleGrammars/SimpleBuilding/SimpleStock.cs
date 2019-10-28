using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
	public class SimpleStock : Shape {
		// grammar rule probabilities:
		const float stockContinueChance = 0.5f;

		// shape parameters:
		[SerializeField]
		int Width;
		[SerializeField]
		int Depth;

		[SerializeField]
		GameObject[] wallStyle;
		[SerializeField]
		GameObject[] roofStyle;

		const float buildDelay = 0.1f;
		
		public void Initialize(int Width, int Depth, GameObject[] wallStyle, GameObject[] roofStyle) {
			this.Width=Width;
			this.Depth=Depth;
			this.wallStyle=wallStyle;
			this.roofStyle=roofStyle;
		}

		protected override void Execute() {
			// Create four walls:
			for (int i = 0; i<4; i++) {
				Vector3 localPosition = new Vector3();
				switch (i) {
					case 0:
						localPosition = new Vector3(-(Width-1)*0.5f, 0, 0); // left
						break;
					case 1:
						localPosition = new Vector3(0, 0, (Depth-1)*0.5f); // back
						break;
					case 2:
						localPosition = new Vector3((Width-1)*0.5f, 0, 0); // right
						break;
					case 3:
						localPosition = new Vector3(0, 0, -(Depth-1)*0.5f); // front
						break;
				}
				SimpleRow newRow = CreateSymbol<SimpleRow>("wall", localPosition, Quaternion.Euler(0, i*90, 0));
				newRow.Initialize(i%2==1 ? Width : Depth,wallStyle);
				newRow.Generate();
			}		

			// Continue with a stock or with a roof (random choice):
			float randomValue = Random.value;
			if (randomValue < stockContinueChance) {
				SimpleStock nextStock = CreateSymbol<SimpleStock>("stock", new Vector3(0, 1, 0));
				nextStock.Initialize(Width, Depth, wallStyle, roofStyle);
				nextStock.Generate(buildDelay);
			} else {
				SimpleRoof nextRoof = CreateSymbol<SimpleRoof>("roof", new Vector3(0, 1, 0));
				nextRoof.Initialize(Width, Depth, roofStyle,wallStyle);
				nextRoof.Generate(buildDelay);
			}
		}
	}
}
