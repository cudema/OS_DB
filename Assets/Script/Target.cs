using UnityEngine;

public class Target : MonoBehaviour
{
    TargetSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
    }

    public void Hit()
    {
        spawner.ChangeTargetVector(this);
    }
}
