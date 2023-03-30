using UnityEngine;
using System.Collections.Generic;

public static class SimplePool
{
    private const int DefaultPoolSize = 3;
    
    private class Pool
    {
        private int _nextId = 1;
        
        private readonly Stack<GameObject> _inactive;
        
        readonly GameObject _prefab;
        
        public Pool(GameObject prefab, int initialQty)
        {
            this._prefab = prefab;
            
            _inactive = new Stack<GameObject>(initialQty);
        }

        #region Changed
        
        public void SpawnPreloaded(GameObject go)
        {
            go.AddComponent<PoolMember>().myPool = this;
            
            go.GetComponent<IPoolNotifiable>()?.OnInstantiate();
        }

        #endregion
        
        public GameObject Spawn(Vector3 pos, Quaternion rot)
        {
            GameObject obj;
            if (_inactive.Count == 0)
            {
                obj = UnityEngine.Object.Instantiate(_prefab, pos, rot);
                obj.name = _prefab.name + " (" + (_nextId++) + ")";

                obj.GetComponent<IPoolNotifiable>()?.OnInstantiate();

                // Add a PoolMember component so we know what pool
                // we belong to.
                obj.AddComponent<PoolMember>().myPool = this;
            }
            else
            {
                obj = _inactive.Pop();

                if (obj == null)
                {

                    return Spawn(pos, rot);
                }
            }

            obj.transform.position = pos;
            obj.transform.rotation = rot;
            
            return obj;
        }
        
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);
            
            _inactive.Push(obj);
        }
    }
    
    private class PoolMember : MonoBehaviour
    {
        public Pool myPool;
    }
    
    private static Dictionary<GameObject, Pool> _pools;
    
    static void Init(GameObject prefab = null, int qty = DefaultPoolSize)
    {
        if (_pools == null)
        {
            _pools = new Dictionary<GameObject, Pool>();
        }
        if (prefab != null && _pools.ContainsKey(prefab) == false)
        {
            _pools[prefab] = new Pool(prefab, qty);
        }
    }
    
    public static void Preload(GameObject prefab, int qty = 1)
    {
        Init(prefab, qty);
        
        var obs = new GameObject[qty];
        for (var i = 0; i < qty; i++)
        {
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);
        }
        
        for (var i = 0; i < qty; i++)
        {
            Despawn(obs[i]);
        }
    }

    #region Changed
    
    public static void InitPreloaded(GameObject prefab, Transform parent)
    {
        Init(prefab, parent.childCount);

        for (int i = 0; i < parent.childCount; i++)
        {
            SpawnPreLoaded(prefab, parent.GetChild(i).gameObject);
        }
        for (int i = 0; i < parent.childCount; i++)
        {
            Despawn(parent.GetChild(i).gameObject);
        }

    }

    private static void SpawnPreLoaded(GameObject prefab, GameObject go)
    {
        _pools[prefab].SpawnPreloaded(go);
    }

    #endregion
    
    public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        Init(prefab);

        return _pools[prefab].Spawn(pos, rot);
    }
    
    public static void Despawn(GameObject obj)
    {
        PoolMember pm = obj.GetComponent<PoolMember>();
        if (pm == null)
        {
            Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
            UnityEngine.Object.Destroy(obj);
        }
        else
        {
            pm.myPool.Despawn(obj);
        }
    }
}