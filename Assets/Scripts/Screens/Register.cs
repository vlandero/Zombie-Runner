using UnityEngine;
using TMPro;
using Proyecto26;
using System;
using UnityEngine.SceneManagement;

public class User
{
    public string username;
    public string password;
    public int highscore = 0;

    public User(string username, string password)
    {
        this.username = username;
        this.password = password;
    }
}

public class HttpRes
{
    public bool error;
    public string payload;
}

[Serializable]
public class UserDTO
{
    public string username;
    public int highscore;
}

public class Register : MonoBehaviour
{
    private TMP_InputField usernameInput;
    private TMP_InputField passwordInput;
    public static string installedVersion = "0.1";
    [SerializeField] private GameObject usernameObject;
    [SerializeField] private GameObject passwordObject;
    [SerializeField] private GameObject errorObject;

    UserError userError;

    [SerializeField] private GameObject RegisterButton;
    [SerializeField] private GameObject LoginButton;
    [SerializeField] private GameObject GoToLoginButton;
    [SerializeField] private GameObject GoToRegisterButton;

    

    private void Start()
    {
        RestClient.Get(Secrets.lambdaUrl + "/get-version").Then(response =>
        {
            HttpRes res = JsonUtility.FromJson<HttpRes>(response.Text);
            if (res.error)
            {
                throw new Exception(res.payload);
            }
            Debug.Log("Version: " + res.payload);
            if (res.payload != installedVersion)
            {
                SceneManager.LoadScene(6);
            }
        }).Catch(error =>
        {
            Debug.Log(error.Message);
        });
        if(PlayerPrefs.HasKey("username"))
        {
            SceneManager.LoadScene(1);
        }
        userError = errorObject.GetComponent<UserError>();
        usernameInput = usernameObject.GetComponent<TMP_InputField>();
        passwordInput = passwordObject.GetComponent<TMP_InputField>();
        passwordInput.contentType = TMP_InputField.ContentType.Password;
    }
    public void OnClickRegister()
    {
        if (usernameInput.text == "" || passwordInput.text == "")
        {
            userError.SetError("Username or password cannot be empty");
            return;
        }
        RestClient.Post(Secrets.lambdaUrl + "/create-user", new User(usernameInput.text, passwordInput.text)).Then(response =>
        {
            HttpRes res = JsonUtility.FromJson<HttpRes>(response.Text);
            if (res.error)
            {
                throw new Exception(res.payload);
            }
            UserDTO user = JsonUtility.FromJson<UserDTO>(res.payload);
            Debug.Log(user.username);
            userError.SetText("User created");
            OnClickGoToLogin();
        }).Catch(error =>
        {
            userError.SetError(error.Message);
        });
    }

    public void OnClickLogin()
    {
        if (usernameInput.text == "" || passwordInput.text == "")
        {
            userError.SetError("Username or password cannot be empty");
            return;
        }
        RestClient.Post(Secrets.lambdaUrl + "/login", new User(usernameInput.text, passwordInput.text)).Then(response =>
        {
            HttpRes res = JsonUtility.FromJson<HttpRes>(response.Text);
            if (res.error)
            {
                throw new Exception(res.payload);
            }
            UserDTO user = JsonUtility.FromJson<UserDTO>(res.payload);
            userError.SetText("User logged in");
            PlayerPrefs.SetString("username", user.username);
            PlayerPrefs.SetInt("highscore", user.highscore);
            PlayerPrefs.Save();
            SceneManager.LoadScene(1);
        }).Catch(error =>
        {
            userError.SetError(error.Message);
        });
        
    }

    public void OnClickGoToLogin()
    {
        RegisterButton.SetActive(false);
        LoginButton.SetActive(true);
        GoToLoginButton.SetActive(false);
        GoToRegisterButton.SetActive(true);
        userError.SetText("");
        usernameInput.text = "";
        passwordInput.text = "";
    }

    public void OnClickGoToRegister()
    {
        RegisterButton.SetActive(true);
        LoginButton.SetActive(false);
        GoToLoginButton.SetActive(true);
        GoToRegisterButton.SetActive(false);
        userError.SetText("");
        usernameInput.text = "";
        passwordInput.text = "";
    }
}
