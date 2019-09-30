using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Agent
{
    

    private int x=0, y=0, direction=0;

    private int[,] grid;

    private MapGeneration parent;

    

    public Agent(int _x,int _y,int _direction,MapGeneration _parent)
    {
        parent = _parent;

        grid = parent.GetGrid();

        x = _x;
        y = _y;
        direction = _direction;

    }


    public void UpdateAgent()
    {
        Profiler.BeginSample("agent sample");

        //Random.InitState(x+y+direction);

        int change = Random.Range(1, 100);
        

        //Debug.Log(change);

        if (change <= 5)
        {
            createAgent();
           
        }

        else if (change==10)
        {

            Debug.Log(change);
            turnLeft();
        }

        else if (change==20)
        {

            Debug.Log(change);
            turnRight();
        }
        else moveForward();
        
        //if (grid[x, y] == 0)
            grid[x, y] = direction+2;
        //else grid[x, y] = 0;
        

        Profiler.EndSample();

    }

    private void destroyAgent()
    {
        Debug.Log("destroy triggered");
        for (int i = 0; i < parent.agents.Count; i++)
        {
            Agent agent =parent.agents[i];
            if (agent == this)
                parent.agents.RemoveAt(i);
                
        }
    }

    private void createAgent()
    {

        parent.createAgent(x, y, 1);
    }

    private int turnLeft()
    {
        direction--;
        if (direction < 0)
            direction = 3;
       

        return direction;
    }

    private int turnRight()
    {
        direction++;
        if (direction > 3)
            direction = 0;
       

        return direction;
    }

    private void moveForward()
    {
        switch (direction)
        {
            case 0: y++; break;
            case 1: x++; break;
            case 2: y--; break;
            case 3: x--; break;

            default:
                break;
        }

        if (y < 0 || y > parent.mapHeight || x < 0 || x > parent.mapWidth)
            destroyAgent();

        switch (direction)
        {
            case 0:
                if (y == parent.mapHeight || grid[x, y + 1] >0)
                    destroyAgent();
                break;
            case 1:
                if (x > parent.mapWidth || grid[x + 1, y] >0)
                    destroyAgent();
                break;
            case 2:
                if (y == 0 || grid[x, y - 1] >0)
                    destroyAgent();
                break;
            case 3:
                if (x == 0 || grid[x - 1, y] > 0)
                    destroyAgent();

                break;

            default:
                break;
        }

    }


}
