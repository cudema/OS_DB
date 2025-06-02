using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    TargetSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
    }

    private void Start()
    {
        StartCoroutine(EtinctionTarget());
    }

    public void Hit()
    {
        spawner.ChangeTargetVector(this);
        spawner.manager.AddScore();
        StopAllCoroutines();
        StartCoroutine(EtinctionTarget());
    }

    IEnumerator EtinctionTarget()
    {
        yield return new WaitForSeconds(spawner.EtinctionTime);

        spawner.ChangeTargetVector(this);
        StartCoroutine(EtinctionTarget());
    }
}
