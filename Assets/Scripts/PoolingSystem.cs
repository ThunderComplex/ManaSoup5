using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingSystem : MonoBehaviour
{
    public static PoolingSystem Instance;
    public List<PoolObject> PoolObjects = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var poolObj = PoolObjects.Find(po => po.Name == prefab.name);

        if (poolObj == null)
        {
            poolObj = new PoolObject { Name = prefab.name };
            PoolObjects.Add(poolObj);
        }

        var obj = poolObj.InactiveObjects.FirstOrDefault();

        if (obj == null)
        {
            obj = Instantiate(prefab, position, rotation);
        }
        else
        {
            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            poolObj.InactiveObjects.Remove(obj);
        }

        obj.transform.parent = gameObject.transform;

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        var objName = obj.name.Replace("(Clone)", "").Trim();
        var poolObj = PoolObjects.Find(po => po.Name == objName);

        if (poolObj == null)
        {
            Debug.Log("Pool Object not found: " + objName);
            Destroy(obj);
            return;
        }

        obj.SetActive(false);

        if (poolObj.InactiveObjects.Contains(obj))
        {
            Debug.Log("Object already in pool: " + objName);
            return;
        }

        poolObj.InactiveObjects.Add(obj);
    }

}

public class PoolObject
{
    public string Name;
    public List<GameObject> InactiveObjects = new();
}
