using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum GameMode { HitScan = 0, Traking }

public class TargetSpawner : MonoBehaviour
{
    [System.Serializable]
    class StageData
    {
        [SerializeField]
        public List<Vector3> targetVectorList = new List<Vector3>();   //Ÿ�� ��ġ ����
        [SerializeField]
        public GameObject targetPrefap;
        [SerializeField]
        [Range(0, 8)]
        public int spawnCount = 1;
    }

    [SerializeField]
    StageData[] stageData;
    StageData currentStageData = null;

    [SerializeField]
    public float EtinctionTime = 4;

    public MoveType moveType;
    public SizeType sizeType;

    [SerializeField]
    TMP_Dropdown[] moveTypeDropdown = new TMP_Dropdown[2];
    [SerializeField]
    TMP_Dropdown[] sizeTypeDropdown = new TMP_Dropdown[2];

    List<Vector3> SpawndTargetList = new List<Vector3>();   //Ÿ�� ��ġ ���纻

    [SerializeField]
    public GameManager manager;

    MemoryPool memoryPool;

    private void Awake()
    {

    }

    //���۽� ������ �� ��ŭ�� Ÿ�� ��ȯ
    public void StartShootTarget()
    {
        if (currentStageData == null)
        {
            Debug.Log("���� ��� ����");
            return;
        }

        memoryPool = new MemoryPool(currentStageData.targetPrefap, currentStageData.spawnCount);
        SpawndTargetList = currentStageData.targetVectorList.ToList();
        SpawnTarget();
    }

    //���� �� Ÿ�� ����
    public void EndShootTarget()
    {
        SpawndTargetList.Clear();
        memoryPool.DestroyObjects();
    }

    void SpawnTarget()
    {
        ChangeMoveMode();
        for (int i = 0; i < currentStageData.spawnCount; i++)
        {
            int index = Random.Range(0, SpawndTargetList.Count);

            GameObject cloen = memoryPool.ActivatePoolItem();
            cloen.transform.position = SpawndTargetList[index];
            cloen.GetComponent<Target>().resetPosition = SpawndTargetList[index];
            cloen.GetComponent<Target>().SetMode(moveType, sizeType);
            SpawndTargetList.RemoveAt(index);
        }
    }

    //Ÿ���� ������� �ٽ� ��Ÿ�� �� ��ġ ����
    public void ChangeTargetVector(Target target)
    {
        int index = Random.Range(0, SpawndTargetList.Count);
        Vector3 temp = target.resetPosition;

        target.transform.position = SpawndTargetList[index];
        target.resetPosition = SpawndTargetList[index];
        SpawndTargetList.RemoveAt(index);
        SpawndTargetList.Add(temp);
    }

    public void ChangeGameMode(GameMode mode)
    {
        currentStageData = stageData[(int)mode];
        manager.gameMode = mode;
    }

    public void ChangeMoveMode()
    {
        moveType = (MoveType)moveTypeDropdown[(int)manager.gameMode].value;
        sizeType = (SizeType)sizeTypeDropdown[(int)manager.gameMode].value;
    }
}