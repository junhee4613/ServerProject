using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;  // UI 화면을 담을 GameObject

    void Start()
    {
        // 초기에는 UI를 비활성화
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }

    void Update()
    {
        // ESC 키를 누르면 UI 화면을 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    void ToggleUI()
    {
        // UI 화면을 활성화 또는 비활성화
        if (uiPanel != null)
        {
            uiPanel.SetActive(!uiPanel.activeSelf);
        }
    }
}