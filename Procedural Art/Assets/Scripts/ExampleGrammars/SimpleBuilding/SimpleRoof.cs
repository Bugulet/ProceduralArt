using UnityEngine;

namespace Demo {
	public class SimpleRoof : Shape {
		// grammar rule probabilities:
		const float roofContinueChance = 0.5f;

		// shape parameters:
		int Width;
		int Depth;

		GameObject[] roofStyle;
		GameObject[] wallStyle;

		// (offset) values for the next layer:
		int newWidth;
		int newDepth;

		const float buildDelay = 0.1f;

		public void Initialize(int Width, int Depth, GameObject[] roofStyle, GameObject[] wallStyle) {
			this.Width=Width;
			this.Depth=Depth;
			this.roofStyle=roofStyle;
			this.wallStyle=wallStyle;
		}


		protected override void Execute() {
			if (Width==0 || Depth==0)
				return;

			newWidth=Width;
			newDepth=Depth;

			CreateFlatRoofPart();
			CreateNextPart();
		}

		void CreateFlatRoofPart() {
			// Randomly create two roof strips in depth direction or in width direction:
			int side = (int)(Random.value*2);
			SimpleRow flatRoof;

			switch (side) {
				// Add two roof strips in depth direction
				case 0:
					for (int i = 0; i<2; i++) {
						flatRoof = CreateSymbol<SimpleRow>("roofStrip",new Vector3((Width-1)*(i-0.5f), 0, 0));
						flatRoof.Initialize(Depth, roofStyle);
						flatRoof.Generate();
					}
					newWidth-=2;
					break;
				// Add two roof strips in width direction
				case 1:
					for (int i = 0; i<2; i++) {
						flatRoof = CreateSymbol<SimpleRow>("roofStrip",new Vector3(0,0,(Depth-1)*(i-0.5f)));
						flatRoof.Initialize(Width, roofStyle,new Vector3(1,0,0));
						flatRoof.Generate();
					}
					newDepth-=2;
					break;
			}
		}

		void CreateNextPart() {
			// randomly continue with a roof or a stock:
			if (newWidth<=0 || newDepth<=0)
				return;

			float randomValue = Random.value;
			if (randomValue<roofContinueChance) { // continue with the roof
				SimpleRoof nextRoof = CreateSymbol<SimpleRoof>("roof");
				nextRoof.Initialize(newWidth, newDepth, roofStyle, wallStyle);
				nextRoof.Generate(buildDelay);
			} else { // continue with a stock
				SimpleStock nextStock = CreateSymbol<SimpleStock>("stock");
				nextStock.Initialize(newWidth, newDepth, wallStyle, roofStyle);
				nextStock.Generate(buildDelay);
			}
		}
	}
}