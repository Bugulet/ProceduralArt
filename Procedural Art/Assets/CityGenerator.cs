using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGenerator : MonoBehaviour
{

    public int width = 30, height = 30;

    public Texture2D inputMap;

    private int houseWidth = 5, houseHeight = 5;

    public GameObject houseBase;

    private Demo.Stock stockComponent;
    private Demo.BuildingParameters parameComponent;

    float scale = 0.1f;

    private int[,] map;

    void Start()
    {
        map = new int[width, height];
        generateCity();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void generateCity()
    {



        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                map[i, j] = (int)(Mathf.PerlinNoise(i * scale, j * scale) * 10);
                generateBuilding(map[i, j], i, j);
            }
        }
    }

    public void generateBuilding(int index, int x, int y)
    {
        Color c = inputMap.GetPixel(x, y);

        if (c.grayscale != 0)
        {
            GameObject house = Instantiate(houseBase, new Vector3(x * houseWidth + 1, 0, y * houseHeight + 1), Quaternion.identity);

            stockComponent = house.GetComponent<Demo.Stock>();
            parameComponent = house.GetComponent<Demo.BuildingParameters>();


            stockComponent.Width = houseWidth - 1;
            stockComponent.Depth = houseHeight - 1;

            parameComponent.maxHeight = (int)(c.grayscale * 256f) / 20;

            stockComponent.Generate();
        }
    }
}
