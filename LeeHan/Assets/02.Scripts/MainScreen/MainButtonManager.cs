using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;                  
                        //������ ���� ĳ���� ������ ĳ���� �����ʹ� DB�� �����س��� �ҷ��´�?? ���� �ڷ� "�˸��� ���� ���� 33Json ����"
public class MainButtonManager : MonoBehaviour
{
    public Button login;
    public Button send;
    public Button start;
    public Button close;
    public Button eixt;
    public GameObject mainScreen_Login_BackGround;

    public Text loginStatuText;

    public string token;

    public const string apiUrl = "http://localhost:3000";

    public InputField[] inputField;
    // Start is called before the first frame update
    void Start()
    {
        if (mainScreen_Login_BackGround.activeSelf)
        {
            mainScreen_Login_BackGround.SetActive(false);
        }
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
        if(inputField[0].text.Length == 0 || inputField[1].text.Length == 0)
        {
            loginStatuText.text = "���̵� �Ǵ� ��й�ȣ�� �Է����� �ʾҽ��ϴ�.";
            return;
        }
        StartCoroutine(AttempLogin(inputField[0].text, inputField[1].text));
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
                    loginStatuText.text = "���̵� Ȥ�� ��й�ȣ�� Ʋ�Ƚ��ϴ�.";
            }
            else
            {
                loginStatuText.text = "�α��� ����!";
                string responseText = webRequest.downloadHandler.text;
                var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                token = responseData.token;

                if (string.IsNullOrEmpty(token))
                {
                    Debug.LogError("Token is missing");
                    yield break;
                }

                SendAytienticatedRequest("/LeeHan/protected");

            }
        }
    }
    public void SendAytienticatedRequest(string endpoint)
    {
        if (string.IsNullOrEmpty(token))
        {
            Debug.LogError("Token is missing");
            return;
        }

        UnityWebRequest www = UnityWebRequest.Get(apiUrl + endpoint);
        www.SetRequestHeader("Authorization", token);

        StartCoroutine(SendRequest(www));
    }
    IEnumerator SendRequest(UnityWebRequest webRequest)
    {
        yield return webRequest.SendWebRequest();

        if(webRequest.result == UnityWebRequest.Result.ConnectionError ||
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("���� ��� ����" + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Request ����" + webRequest.downloadHandler.text);
        }
    }
    public void SignUp()
    {
        if(SignUpCondition())       //������ ���̵� �Ǵ� ��й�ȣ ���� ������ ������
        {
            StartCoroutine(AttempSignUp(inputField[0].text, inputField[1].text));
        }
    }
    IEnumerator AttempSignUp(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl + "/LeeHan/sign_up", form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                loginStatuText.text = res.message;
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                loginStatuText.text = res.message;
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
    public bool SignUpCondition()
    {
        return true;
    }
}
