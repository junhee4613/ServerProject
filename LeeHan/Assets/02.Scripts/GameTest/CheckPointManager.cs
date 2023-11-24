using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckPoint[] allCP;

    private CheckPoint activeCP;

    public Vector3 respawnPosition; //플레이어 리스폰
    // Start is called before the first frame update
    void Start()
    {
        allCP = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);

        foreach (CheckPoint cp in allCP)
        {
            cp.cpMan = this;
        }

        respawnPosition = FindFirstObjectByType<PlayerController>().transform.position;
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

        respawnPosition = newActiveCP.transform.position;
    }

}
