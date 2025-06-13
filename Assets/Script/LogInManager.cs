using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LogInManager : MonoBehaviour
{
    [Header("LogIn")]
    public GameObject LogInPanel;
    public Button LogInButton;
    public Button SignUpStartButton;
    public TMP_InputField L_inputID;
    public TMP_InputField L_inputPW;

    [Header("SignUp")]
    public GameObject SignUpPanel;
    public Button SignUpButton;
    public Button BackButton;
    public TMP_InputField S_inputID;
    public TMP_InputField S_inputPW;

    [SerializeField]
    private GameObject MenuUI;

    [SerializeField]
    private GameManager gameManager;


    public void LogIn()
    {
        string ID = L_inputID.text;
        string PW = L_inputPW.text;

        if (DBManager.LogIn(ID, PW))
        {
            gameManager.currentUserName = ID;

            RunTimeTracker.Instance.userId = ID;

            LogInPanel.SetActive(false);
            MenuUI.SetActive(true);
        }

        L_inputID.text = "";
        L_inputPW.text = "";
    }

    public void SignUpStart()
    {
        LogInPanel.SetActive(false);
        SignUpPanel.SetActive(true);
    }

    public void BackLogInPanel()
    {
        LogInPanel.SetActive(true);
        SignUpPanel.SetActive(false);
    }

    public void SignUp()
    {
        string ID = S_inputID.text;
        string PW = S_inputPW.text;

        DBManager.SignUp(ID, PW);

        S_inputID.text = "";
        S_inputPW.text = "";

        LogInPanel.SetActive(true);
        SignUpPanel.SetActive(false);
    }
}
