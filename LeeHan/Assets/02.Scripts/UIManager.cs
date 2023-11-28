using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;  // UI ȭ���� ���� GameObject

    void Start()
    {
        // �ʱ⿡�� UI�� ��Ȱ��ȭ
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        // ESC Ű�� ������ UI ȭ���� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    void ToggleUI()
    {
        // UI ȭ���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    }
}