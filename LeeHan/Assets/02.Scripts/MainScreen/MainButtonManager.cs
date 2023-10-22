using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;                  
                        //계정에 따른 캐릭터 보유나 캐릭터 데이터는 DB에 저장해놓고 불러온다?? 참고 자료 "알만툴 기초 강의 33Json 파일"
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
            loginStatuText.text = "아이디 또는 비밀번호를 입력하지 않았습니다.";
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
                    loginStatuText.text = "아이디 혹은 비밀번호가 틀렸습니다.";
            }
            else
            {
                loginStatuText.text = "로그인 성공!";
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
            Debug.Log("서버 통신 에러" + webRequest.downloadHandler.text);
        }
        else
        {
            Debug.Log("Request 성공" + webRequest.downloadHandler.text);
        }
    }
    public void SignUp()
    {
        if(SignUpCondition())       //집가서 아이디 또는 비밀번호 생성 조건을 만들자
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
