using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckPoint[] allCP;

    private CheckPoint activeCP;
    // Start is called before the first frame update
    void Start()
    {
        allCP = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);

        foreach (CheckPoint cp in allCP)
        {
            cp.cpMan = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeactivateAllCheckpoints();
        }
    }

    public void DeactivateAllCheckpoints()
    {
        foreach (CheckPoint cp in allCP)
        {
            cp.DeactivateCheckpoint();
        }
    }

    public void SetActiveCheckpoint(CheckPoint newActiveCP)
    {
        DeactivateAllCheckpoints();

        activeCP = newActiveCP;
    }

}
