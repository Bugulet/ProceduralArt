using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
	public class Roof : Shape {
		public int Width;
		public int Depth;
		public int HeightRemaining;

		public void Initialize(int Width, int Depth, int HeightRemaining) {
			this.Width=Width;
			this.Depth=Depth;
			this.HeightRemaining=HeightRemaining;
		}

		// (offset) values for the next layer:
		int newWidth;
		int newDepth;

		protected override void Execute() {
			if (Width==0 || Depth==0)
				return;

			newWidth=Width;
			newDepth=Depth;

			CreateFlatRoofPart();
			CreateNextPart();
		}

		void CreateFlatRoofPart() {
			BuildingParameters param = (BuildingParameters)parameters;

			int side = param.Rand.Next(2);
			Row flatRoof;

			switch (side) {
				// Add two roof strips in depth direction
				case 0:
					for (int i = 0; i<2; i++) {
						flatRoof = CreateSymbol<Row>("roofStrip",
							new Vector3((Width-1)*(i-0.5f), 0, 0),
							Quaternion.identity,
							transform
						);
						flatRoof.Initialize(Depth, param.roofStyle);
						flatRoof.Generate();
					}
					newWidth-=2;
					break;
				// Add two roof strips in width direction
				case 1:
					for (int i = 0; i<2; i++) {
						flatRoof = CreateSymbol<Row>("roofStrip",
							new Vector3(0,0,(Depth-1)*(i-0.5f)),
							Quaternion.Euler(0,0,0),
							transform
						);
						flatRoof.Initialize(Width, param.roofStyle,null,new Vector3(1,0,0));
						flatRoof.Generate();
					}
					newDepth-=2;
					break;
			}
		}

		void CreateNextPart() {
			if (newWidth<=0 || newDepth<=0)
				return;
			BuildingParameters param = (BuildingParameters)parameters;

			double randomValue = param.Rand.NextDouble();
			if (randomValue<param.RoofContinueChance || HeightRemaining <= 0) { // continue with the roof
				Roof nextRoof = CreateSymbol<Roof>("roof");
				nextRoof.Initialize(newWidth, newDepth, HeightRemaining);
				nextRoof.Generate(param.buildDelay);
			} else { // continue with a stock
				Stock nextStock = CreateSymbol<Stock>("stock");
				nextStock.Initialize(newWidth, newDepth, HeightRemaining);
				nextStock.Generate(param.buildDelay);
			}
		}
	}
}