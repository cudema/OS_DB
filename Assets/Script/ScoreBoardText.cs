using TMPro;
using UnityEngine;

public class ScoreBoardText : MonoBehaviour
{
    [SerializeField]
    TMP_Text textObj;

    public void SetText(string[] textData)
    {
        if (textData == null)
        {
            Destroy(gameObject);
            return;
        }
        textObj.text = $"{textData[0]}/{textData[1]}/{textData[2]}/{textData[3]}/{textData[4]}";
    }
}
