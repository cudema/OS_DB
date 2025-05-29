using UnityEngine;
using System.Collections.Generic;

public class MemoryPool
{
    class PoolItem
    {
        public bool isActive;
        public GameObject gameObject;
    }

    int increaseCount = 5;
    int maxCount;
    int activeCount;

    GameObject poolObject;
    List<PoolItem> poolItemList;

    public int MaxCount => maxCount;
    public int ActiveCount => activeCount;

    public MemoryPool(GameObject poolObject, int increaseCount)
    {
        maxCount = 0;
        activeCount = 0;
        this.poolObject = poolObject;
        this.increaseCount = increaseCount;

        poolItemList = new List<PoolItem>();

        InstantiateObject();
    }

    public void InstantiateObject()
    {
        maxCount += increaseCount;

        for (int i = 0; i < increaseCount; i++)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false;
            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.SetActive(false);

            poolItemList.Add(poolItem);
        }
    }

    public void DestroyObjects()
    {
        if (poolItemList == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    public GameObject ActivatePoolItem()
    {
        if (poolItemList == null)
        {
            return null;
        }

        if (maxCount == activeCount)
        {
            InstantiateObject();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.isActive == false)
            {
                activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    public GameObject DeactivatePoolItem()
    {
        if (poolItemList == null)
        {
            return null;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject.activeSelf == true)
            {
                activeCount--;

                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);

                return poolItem.gameObject;
            }
        }

        return null;
    }

    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }

        activeCount = 0;
    }
}
