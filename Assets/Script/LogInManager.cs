using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LogInManager : MonoBehaviour
{
    public GameObject LogInPanel;
    public Button LogInButton;
    public TMP_InputField inputID;
    public TMP_InputField inputPW;


    public void LogIn()
    {
        string ID = inputID.text;
        string PW = inputPW.text;

        Debug.Log($"로그인 시도: ID = {ID}, PW = {PW}");

        inputID.text = "";
        inputPW.text = "";

        LogInPanel.SetActive(false);
    }
}
