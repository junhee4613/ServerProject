using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public CheckPoint[] allCP;          //üũ����Ʈ �迭 ����

    private CheckPoint activeCP;        //Ȱ��ȭ�� üũ����Ʈ

    public Vector3 respawnPosition; //�÷��̾� ������
    // Start is called before the first frame update
    void Start()
    {
        allCP = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);    //üũ����Ʈ �迭�� �ڵ� ����

        foreach (CheckPoint cp in allCP)            //üũ����Ʈ�Ŵ����� none ���� ����
        {
            cp.cpMan = this;
        }

        respawnPosition = FindFirstObjectByType<PlayerController>().transform.position;
    }

    // Update is called once per frame
    void Update()           // C������ �迭�� �ִ� üũ����Ʈ ��Ȱ��ȭ 
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            DeactivateAllCheckpoints();
        }
    }

    public void DeactivateAllCheckpoints()      //�迭�� �ִ� üũ����Ʈ ��Ȱ��ȭ
    {
        foreach (CheckPoint cp in allCP)
        {
            cp.DeactivateCheckpoint();
        }
    }

    public void SetActiveCheckpoint(CheckPoint newActiveCP)     //���ο� üũ����Ʈ Ȱ��ȭ 
    {
        DeactivateAllCheckpoints();     //������ �ִ� üũ����Ʈ ��Ȱ��ȭ

        activeCP = newActiveCP;

        respawnPosition = newActiveCP.transform.position;       //üũ����Ʈ�� ������
    }

}
