using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Demo;

[ExecuteInEditMode]
public class GenerationPlane : MonoBehaviour
{

    public GameObject TriggerObject;

    public bool GenerateWhileEditing = false;
    private BuildTrigger triggerScript;

    [HideInInspector]
    public int depth, width, height = 5;

    Vector3 keeper;

    // Start is called before the first frame update
    void Start()
    {
        triggerScript = TriggerObject.GetComponent<BuildTrigger>();

        if (triggerScript == null)
        {
            Debug.Log("trigger script does not exist");
        }

        keeper = TriggerObject.transform.localScale;
    }

    public void Generate()
    {
        TriggerObject.GetComponent<BuildingParameters>().maxHeight = height-1;
        TriggerObject.GetComponent<Stock>().Width = width;
        TriggerObject.GetComponent<Stock>().Depth = depth;
        triggerScript.Generate();
    }

    // Update is called once per frame


    void Update()
    {

        Vector3 scale = transform.localScale;
        scale.Set((int)scale.x, 0.1f, (int)scale.z);

        if (GenerateWhileEditing == true)
        {
            if (keeper.magnitude != scale.magnitude)
            {
                keeper.Set((int)scale.x, 0.1f, (int)scale.z);
                Generate();
            }
        }

        if (scale.z <= 0 || scale.x <= 0)
        {
            scale = Vector3.one;
            scale.y = 0.1f;
        }

        transform.localScale = scale;

        width = (int)transform.localScale.x;
        depth = (int)transform.localScale.z;



        TriggerObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

        if (Input.GetKeyDown(KeyCode.L))
        {
            Generate();
        }
        // triggerScript.
    }
}


