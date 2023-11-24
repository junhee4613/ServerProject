using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckPoint[] allCP;          //체크포인트 배열 생성

    private CheckPoint activeCP;        //활성화된 체크포인트

    public Vector3 respawnPosition; //플레이어 리스폰
    // Start is called before the first frame update
    void Start()
    {
        allCP = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);    //체크포인트 배열에 자동 정렬

        foreach (CheckPoint cp in allCP)            //체크포인트매니저를 none 으로 변경
        {
            cp.cpMan = this;
        }

        respawnPosition = FindFirstObjectByType<PlayerController>().transform.position;
    }

    // Update is called once per frame
    void Update()           // C누르면 배열에 있는 체크포인트 비활성화 
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeactivateAllCheckpoints();
        }
    }

    public void DeactivateAllCheckpoints()      //배열에 있는 체크포인트 비활성화
    {
        foreach (CheckPoint cp in allCP)
        {
            cp.DeactivateCheckpoint();
        }
    }

    public void SetActiveCheckpoint(CheckPoint newActiveCP)     //새로운 체크포인트 활성화 
    {
        DeactivateAllCheckpoints();     //기존에 있던 체크포인트 비활성화

        activeCP = newActiveCP;

        respawnPosition = newActiveCP.transform.position;       //체크포인트로 리스폰
    }

}
