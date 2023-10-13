using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class MainButtonManager : MonoBehaviour
{
    public Button login;
    public Button send;
    public Button start;
    public Button close;
    public Button eixt;
    public GameObject mainScreen_Login_BackGround;


    public InputField id_input;
    public InputField password_input;
    public Text loginStatuText;

    public string token;

    public const string apiUrl = "노드 주소 이따가 써야됨";

    public InputField[] inputField;
    // Start is called before the first frame update
    void Start()
    {
        if (mainScreen_Login_BackGround.activeSelf)
        {
            mainScreen_Login_BackGround.SetActive(false);
        }
        this.send.onClick.AddListener(() =>
        {

        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
            if(currentInputField != null)
            {
                int currentIndex = System.Array.IndexOf(inputField, currentInputField);
                int nextIndex = (currentIndex + 1) % inputField.Length;

                inputField[nextIndex].Select();
                inputField[nextIndex].ActivateInputField();
            }
        }
    }
    public void Login()
    {
        StartCoroutine(AttempLogin(id_input.text, password_input.text));
    }
    IEnumerator AttempLogin(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);

        using(UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl + "/LeeHan/login", form))
        {
            yield return webRequest.SendWebRequest();

            if(webRequest.result != UnityWebRequest.Result.Success)
            {
                loginStatuText.text = "로그인에 실패 : " + webRequest.error;
            }
            else
            {
                loginStatuText.text = "로그인 성공!";
                string responseText = webRequest.downloadHandler.text;
                var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                token = responseData.token;

            }
        }
    }
    private class ResponseData
    {
        public string token;
    }
    public void StartOrClose()
    {
        if (mainScreen_Login_BackGround.activeSelf)
        {
            mainScreen_Login_BackGround.SetActive(false);
        }
        else
        {
            mainScreen_Login_BackGround.SetActive(true);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
