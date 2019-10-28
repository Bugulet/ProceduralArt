using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo {
	public class MarchingSquares : Shape {
		public GameObject[] cornerPrefabs;
		public int[] prefabRotations;

		public int gridSize = 30;

		public int cellSize = 1;

		public int[,] grid;

		// Basically, in the next table we count the number of ones in the binary representation.
		// However when there are two ones, we distinguish two cases: adjacent or not.
		int[] prefabIndex = new int[] {
			0,	// 0 = 0000		case 0: no ones
			1,	// 1 = 0001		case 1: a single one
			1,	// 2 = 0010
			2,	// 3 = 0011		case 2: two adjacent ones
			1,	// 4 = 0100
			3,	// 5 = 0101		case 3: two nonadjacent ones
			2,	// 6 = 0110
			4,	// 7 = 0111		case 4: three ones
			1,	// 8 = 1000
			2,	// 9 = 1001		case 2 again: counting cyclically, these are adjacent!
			3,	// 10= 1010
			4,	// 11= 1011
			2,	// 12= 1100
			4,	// 13= 1101
			4,	// 14= 1110
			5   // 15= 1111		case 5: four ones
		};
		// The next table contains the rotation (which should be multiplied by 90 degrees)
		// compared to the default prefab rotation.	
		int[] rotationIndex = new int[] {
			0,	// 0 = 0000		
			0,	// 1 = 0001		
			1,	// 2 = 0010
			0,	// 3 = 0011
			2,	// 4 = 0100
			0,	// 5 = 0101		
			1,	// 6 = 0110
			3,	// 7 = 0111		
			3,	// 8 = 1000
			3,	// 9 = 1001		
			1,	// 10= 1010
			2,	// 11= 1011
			2,	// 12= 1100
			1,	// 13= 1101
			0,	// 14= 1110
			0	// 15= 1111	
		};

		public void FillGrid() {
			// Here: add some of your own code to fill the grid in interesting ways...
			
			float xOffset = Random.value;
			float yOffset = Random.value;

			/**/
			for (int i = 0; i<grid.GetLength(0); i++) {
				for (int j = 0; j<grid.GetLength(1); j++) {
					// initialize the grid with 0:					
					//grid[i, j]=0;

					// randomize the grid:
					//grid[i, j]=(int)(Random.value*2); 

					// Perlin noise:
					grid[i, j]=(int)Mathf.Round(Mathf.PerlinNoise(i*0.1f + xOffset+Time.frameCount, j*0.1f + yOffset + Time.frameCount));
				}
			}

			/**/
			// Draw a road:
			for (int i=0;i<grid.GetLength(0);i++) {
				grid[i, grid.GetLength(1)/2]=1;
			}
			/**/
		}

		void Update() {
			if (Input.GetKeyDown(KeyCode.G)) { // create the grid
				if (grid==null) {
					grid=new int[gridSize, gridSize];
				}
				FillGrid();
				Generate();
			}
		}

		public int GetBitMask(int i, int j) {
			int bitMask = 0;
			// Loop over all corners, in cyclic order:
			for (int k = 0; k<4; k++) { 
				// The formula in the line below maps...
				//  k:	to:
				//  0	0,0
				//  1	0,1
				//  2	1,1
				//  3	1,0
				int x = i + (k/2);
				int y = j + ((k+1)/2)%2;
				if (x>=0 && x<grid.GetLength(0) && y>=0 && y<grid.GetLength(1) &&
					grid[x, y]>0) { // If this corner's grid cell is filled...
					bitMask |= (1<<k); // ...set bit k of bitmask to 1.

					// Explanation:
					//  b<<k means shift all bits in the binary representation of b k positions to the left, inserting 0 on the right.
					// So
					//  1<<3 = 1000 (in binary) = 8 (in decimal)
					// and
					//  1111<<2 = 111100 (in binary). => In decimal: 15 becomes 60.
					// In other words: b<<k means multiply b by 2 to the power of k.
					//
					// a|b means take the "bitwise or".
					// So
					//  1100 | 1001 = 1101  (in binary), or
					//    12 | 9    = 13    (in decimal)
					//
					// End result: after this loop is done, bitMask has a value from 0000 to 1111, indicating which of the 
					// four grid corners are non-zero.
				}
			}
			return bitMask;
		}

		public GameObject SpawnPrefab(int i, int j, int bitMask) {
			return SpawnPrefab(
				cornerPrefabs[prefabIndex[bitMask]],
				new Vector3(i, 0, j) * cellSize,
				Quaternion.Euler(0, 90*(rotationIndex[bitMask] + prefabRotations[prefabIndex[bitMask]]), 0)
			);
		}

		protected override void Execute() {
			if (grid==null) {
				Debug.Log("Press G to initialize the grid first");
				return;
			}
			// Generate the game objects from the grid
			// Loop over all square cells between the grid sample points:
			// (If grid is n x m, the number of square cells is (n-1) x (m-1).)
			for (int i = 0; i<grid.GetLength(0)-1; i++) {
				for (int j = 0; j<grid.GetLength(1)-1; j++) {
					int bitMask = GetBitMask(i, j);

					// The result is a bitmask value between 0 (=0000) and 15 (=1111).
					// Use the lookup tables to match this to a prefab and rotation:
					SpawnPrefab(i, j, bitMask);
				}
			}
		}
	}
}