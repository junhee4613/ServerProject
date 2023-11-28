using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
                        //������ ���� ĳ���� ������ ĳ���� �����ʹ� DB�� �����س��� �ҷ��´�?? ���� �ڷ� "�˸��� ���� ���� 33Json ����"
public class MainButtonManager : MonoBehaviour
{
    public Button login;
    public Button send;
    public Button start;
    public Button close;
    public Button eixt;
    public GameObject mainScreen_Login_BackGround;

    public GameObject name_window;
    public InputField player_name_field;
    public Button name_decide;
    public Text player_name_status;
    GameManager GameManager => GameManager.instance;

    public Text loginStatuText;

    public string token;

    public const string apiUrl = "https://port-0-leehan-node-20231014-jvpb2alnb1xslw.sel5.cloudtype.app";
    //public const string apiUrl = "http://localhost:3000";

    public InputField[] inputField;
    // Start is called before the first frame update
    void Start()
    {
        if (mainScreen_Login_BackGround.activeSelf)
        {
            mainScreen_Login_BackGround.SetActive(false);
        }
        name_decide.onClick.AddListener(() =>
        {
            StartCoroutine(Name_decide());
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
    public IEnumerator Name_decide()
    {
        if (string.IsNullOrEmpty(player_name_field.text))
        {
            player_name_status.text = "����Ͻ� �̸��� �Է��Ͻ��� �����̽��ϴ�.";
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("id", inputField[0].text);
            form.AddField("player_name", player_name_field.text) ;

            using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl + "/LeeHan/name_decide", form))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                    player_name_status.text = res.message;
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                    var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                    GameManager.MyData res2 = JsonConvert.DeserializeObject<GameManager.MyData>(responseText);
                    GameManager.sendData = res2;
                    player_name_status.text = res.message;
                    name_window.SetActive(false);
                }
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
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.downloadHandler.text);
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(webRequest.downloadHandler.text);
                Debug.Log(res.name_is_null);
                GameManager.sendData.id = inputField[0].text;
                loginStatuText.text = res.message;
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                loginStatuText.text = "�α��� ����!";
                string responseText = webRequest.downloadHandler.text;
                var responseData = JsonConvert.DeserializeObject<ResponseData>(responseText);
                GameManager.MyData res = JsonConvert.DeserializeObject<GameManager.MyData>(responseText);
                token = responseData.token;
                GameManager.sendData = res;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                if (res.name_is_null)
                {
                    name_window.SetActive(true);
                }
            }
        }
    }
    private class ResponseData
    {
        public string token;
        public string id;
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
