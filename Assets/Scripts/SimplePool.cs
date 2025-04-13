using UnityEngine;
using UnityEngine.Pool;

public class SimplePoolFactory<T> where T : MonoBehaviour
{
    private ObjectPool<T> _pool;
    private Transform _poolHolder;

    public ObjectPool<T> CreatePool(T poolPrefab, Transform poolParent, int startSize)
    {
        if (poolParent != null)
        {
            _poolHolder = poolParent;
        }
        else
        {
            string poolName = $"{poolPrefab.name} Pool";
            _poolHolder = new GameObject(poolName).transform;
        }
        
        _pool = new ObjectPool<T>(
            () => CreatePoolObj(poolPrefab),
            GetFromPool,
            ReleaseToPool,
            DestroyFromPool,
            false,
            startSize * 2,
            999
            );

        InitializePool(startSize);
        return _pool;
    }

    private void InitializePool(int size)
    {
        T [] pooledObjs = new T[size];
        for (int i = 0; i < size; i++)
            pooledObjs[i] = _pool.Get();
        for (int i = 0; i < pooledObjs.Length; i++)
            _pool.Release(pooledObjs[i]);
    }

    private T CreatePoolObj(T pooledObject)
    {
        T obj = GameObject.Instantiate(pooledObject, _poolHolder);
        return obj;
    }

    private void GetFromPool(T pooledObject) => pooledObject.gameObject.SetActive(true);
    private void ReleaseToPool(T pooledObject) => pooledObject.gameObject.SetActive(false);
    private void DestroyFromPool(T pooledObject) => GameObject.Destroy(pooledObject.gameObject);

}
