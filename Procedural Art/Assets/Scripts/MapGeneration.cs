using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    [SerializeField]
    public int mapWidth = 10, mapHeight = 10;

    [SerializeField]
    private float size = 0.25f;

    private int[,] grid;

    public List<Agent> agents;
    

    void Start()
    {
        grid = new int[mapHeight, mapWidth];

        agents = new List<Agent>();

        agents.Add(new Agent(mapWidth / 2, mapHeight / 2,0, this));
        agents.Add(new Agent(mapWidth / 2, mapHeight / 2, 1, this));
        agents.Add(new Agent(mapWidth / 2, mapHeight / 2, 2, this));
        agents.Add(new Agent(mapWidth / 2, mapHeight / 2, 3, this));


        agents.Add(new Agent(10, 10, 1, this));
        agents.Add(new Agent(mapWidth - 10, mapHeight - 10, 3, this));
        agents.Add(new Agent(10, mapHeight - 10, 1, this));
        agents.Add(new Agent(mapWidth - 10, 10, 3, this));

        ClearMap();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Generate();
    }

    //add a new agent to the list (triggered by the agent class)
    public void createAgent(int x,int y, int direction)
    {
        agents.Add(new Agent(x, y, direction, this));
    }


    //clears the entire map (set everything to 0)
    private void ClearMap()
    {
        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                grid[i, j] = 0;
            }
        }
    }
    

    //iterate through the generation algorithm once
    [ContextMenu("Generate")]
    private void Generate()
    {
        int step = (int)(Time.time * 10000);
            Random.InitState(step);
            foreach (var agent in agents.ToArray())
            {
                step++;
                agent.UpdateAgent();
            }
        
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = new Vector3();

        for (int i = 0; i < mapHeight; i++)
        {
            for (int j = 0; j < mapWidth; j++)
            {
                pos.x = i * size;
                pos.y = j * size;
                Gizmos.color = (grid[i, j] >0) ? Color.black : Color.white;

                if (grid[i, j] == 2) Gizmos.color = Color.red;

                if (grid[i, j] == 3) Gizmos.color = Color.blue;

                if (grid[i, j] == 4) Gizmos.color = Color.green;

                if (grid[i, j] == 5) Gizmos.color = Color.yellow;

                Gizmos.DrawCube(pos, Vector3.one * size);
            }
        }

    }

    public int[,] GetGrid()
    {
        return grid;
    }

}
