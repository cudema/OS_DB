using UnityEngine;


public class OpenRankingBeard : MonoBehaviour
{
    [SerializeField]
    GameObject textPrefab;
    [SerializeField]
    Transform[] parentObject;

    public void SetScoreBeard(GameMode mode)
    {
        string[][] ranking = DBManager.GetRankingBoard(mode.ToString());
        for (int i = 0; i < 5; i++)
        {
            GameObject clon = Instantiate(textPrefab, parentObject[(int)mode]);
            clon.GetComponent<ScoreBoardText>().SetText(ranking[i]);
        }
    }

    public void DestroyTextPrefab()
    {
        for (int i = 0; i < parentObject[0].childCount; i++)
        {
            Destroy(parentObject[0].GetChild(i).gameObject);
        }

        for (int i = 0; i < parentObject[1].childCount; i++)
        {
            Destroy(parentObject[1].GetChild(i).gameObject);
        }
    }
}
