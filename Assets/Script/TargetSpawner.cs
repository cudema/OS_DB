using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Configuration;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    List<Vector3> targetVectorList = new List<Vector3>();   //Ÿ�� ��ġ ����
    List<Vector3> SpawndTargetList = new List<Vector3>();   //Ÿ�� ��ġ ���纻
    [SerializeField]
    GameObject targetPrefap;
    [SerializeField]
    [Range(0, 8)]
    int spawnCount = 1;
    [SerializeField]
    public float EtinctionTime = 4;
    [SerializeField]
    public GameManager manager;

    MemoryPool memoryPool;

    private void Awake()
    {

    }

    //���۽� ������ �� ��ŭ�� Ÿ�� ��ȯ
    public void StartShootTarget()
    {
        memoryPool = new MemoryPool(targetPrefap, spawnCount);
        SpawndTargetList = targetVectorList.ToList();
        SpawnTarget();
    }

    //���� �� Ÿ�� ����
    public void EndShootTarget()
    {
        SpawndTargetList.Clear();
        memoryPool.DestroyObjects();
    }

    //Ÿ���� ��ȯ
    void SpawnTarget()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int index = Random.Range(0, SpawndTargetList.Count);

            GameObject cloen = memoryPool.ActivatePoolItem();
            cloen.transform.position = SpawndTargetList[index];
            SpawndTargetList.RemoveAt(index);
        }
    }

    //Ÿ���� ������� �ٽ� ��Ÿ�� �� ��ġ ����
    public void ChangeTargetVector(Target target)
    {
        int index = Random.Range(0, SpawndTargetList.Count);
        Vector3 temp = target.transform.position;

        target.transform.position = SpawndTargetList[index];
        SpawndTargetList.Add(temp);
        SpawndTargetList.RemoveAt(index);
    }
}