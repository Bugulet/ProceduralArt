using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class ShotgunHouse : Stock
    {

        [SerializeField] private GameObject RoofPrefab;

        [SerializeField] private int DoorOrientation = 0;

        private int currentFloor = 0;
        private int maxHeight;

        //check to see if this is the first floor, so we can spawn a door
        public bool isFloor = true;
        

        public void Initialize(int Width, int Depth, int floorNow,int maxHeight , GameObject roof,bool floorValue)
        {
            isFloor = floorValue;

            RoofPrefab = roof;

            this.maxHeight = maxHeight;
            this.Width = Width;
            this.Depth = Depth;
            currentFloor = floorNow;
        }

        protected override void Execute()
        {
            
            // This is necessary for the start symbol of the grammar:
            if (parameters == null)
            {
                parameters = GetComponent<BuildingParameters>();
            }

            BuildingParameters param = (BuildingParameters)parameters;
            
            GameObject[] walls = new GameObject[4];



            for (int i = 0; i < 4; i++)
            {
                Vector3 localPosition = new Vector3();
                switch (i)
                {
                    case 0:
                        localPosition = new Vector3(-(Width - 1) * 0.5f, 0, 0); // left
                        break;
                    case 1:
                        localPosition = new Vector3(0, 0, (Depth - 1) * 0.5f); // back
                        break;
                    case 2:
                        localPosition = new Vector3((Width - 1) * 0.5f, 0, 0); // right
                        break;
                    case 3:
                        localPosition = new Vector3(0, 0, -(Depth - 1) * 0.5f); // front
                        break;
                }
                Row newRow = CreateSymbol<Row>("wall", localPosition, Quaternion.Euler(0, i * 90, 0), transform);

                if (isFloor == true && i==DoorOrientation)
                {
                    newRow.Initialize(
                      i % 2 == 1 ? Width : Depth,
                      param.wallStyle, param.doorStyle,
                      param.wallPattern
                  );
                    Debug.Log("initializing doors");
                    
                }
                else
                {
                    newRow.Initialize(
                         i % 2 == 1 ? Width : Depth,
                         param.wallStyle,
                         param.wallPattern
                     );

                    Debug.Log("initializing normal wall");
                }

                

                newRow.Generate();
            }

            double randomValue = param.Rand.NextDouble();

            if (param.maxHeight-currentFloor > 0)
            {
                ShotgunHouse nextStock = CreateSymbol<ShotgunHouse>("ShotgunStock", new Vector3(0, 1, 0), Quaternion.identity, transform);
                nextStock.Initialize(Width, Depth, currentFloor+1,param.maxHeight,RoofPrefab,false);
                nextStock.Generate(param.buildDelay);
            }
            else
            {
                //instantiate the roof
                GameObject tr= Instantiate(RoofPrefab, transform);


                //set a roof position offset
                Vector3 offset = new Vector3(-Width / 2f, 1, -Depth / 2f);

                //set a roof scale offset       average them for height
                Vector3 scale = new Vector3(Width, (Width + Depth) / 2f, Depth);

                tr.transform.localScale = scale;
                tr.transform.localPosition = offset;
            }
        }
    }
}
