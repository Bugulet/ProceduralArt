using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class Stock : Shape
    {
        public int Width;
        public int Depth;
        public int HeightRemaining =-1;
        protected BuildingParameters param;

        public virtual void Initialize(int Width, int Depth, int HeightRemaining)
        {

            if (parameters == null)
            {
                parameters = GetComponent<BuildingParameters>();
            }
            param = (BuildingParameters)parameters;
            
            this.Width = Width;
            this.Depth = Depth;

            if (this.HeightRemaining == -1)
            {
                this.HeightRemaining = param.maxHeight;
            }
            else
            {
                this.HeightRemaining = HeightRemaining;
            }

        }

        protected override void Execute()
        {
            

            
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

                if (HeightRemaining != param.maxHeight)
                {
                    newRow.Initialize(
                        i % 2 == 1 ? Width : Depth,
                        param.wallStyle,
                        param.wallPattern
                    );

                    Debug.Log("initializing normal wall");
                }
                else
                {
                    newRow.Initialize(
                    i % 2 == 1 ? Width : Depth,
                    param.wallStyle, param.doorStyle,
                    param.wallPattern
                );
                    Debug.Log("initializing doors");
                }

                newRow.Generate();
            }

            double randomValue = param.Rand.NextDouble();

            if (HeightRemaining > 0 && randomValue < param.StockContinueChance)
            {
                Stock nextStock = CreateSymbol<Stock>("stock", new Vector3(0, 1, 0), Quaternion.identity, transform);
                nextStock.Initialize(Width, Depth, HeightRemaining - 1);
                nextStock.Generate(param.buildDelay);
            }
            else
            {
                Roof nextRoof = CreateSymbol<Roof>("roof", new Vector3(0, 1, 0), Quaternion.identity, transform);
                nextRoof.Initialize(Width, Depth, HeightRemaining - 1);
                nextRoof.Generate(param.buildDelay);
            }
        }
    }
}
