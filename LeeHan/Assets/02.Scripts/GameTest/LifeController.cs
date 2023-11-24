using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public static LifeController instance;

    private void Awake()
    {
        instance = this;
    }

    private PlayerController thePlayer;
    // Start is called before the first frame update
    void Start()
    {
        thePlayer = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        thePlayer.transform.position = FindAnyObjectByType<CheckPointManager>().respawnPosition;  //�÷��̾ üũ����Ʈ�� ���ư��� �Ѵ�.
    }
}
