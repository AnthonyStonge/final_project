using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private int objToPreAllocate;
    private int maxNumberOfObjects;
    private int extraObjCount;
    private Queue<T> pooledObjects;
    private Func<T> factoryMethod;

    public ObjectPool(int _objectsToPreAllocate, int _maxNumberOfObjects, Func<T> func)
    {
        objToPreAllocate = _objectsToPreAllocate;
        SetMaxNumberOfObjects(_maxNumberOfObjects);
        factoryMethod = func;

        // Queue which keeps all the queued objects
        pooledObjects = new Queue<T>();
        T genericObject;
        for (int i = 0; i < _objectsToPreAllocate; i++)
        {
            genericObject = factoryMethod.Invoke();
            if (!genericObject.Equals(default(T)))
            {
                pooledObjects.Enqueue(genericObject);
            }
        }
    }

    public T GetObjectFromPool()
    {
        T genericObject = default;
        //Pool already has free objects
        if (GetNumberOfObjectsInPool() > 0)
        {
            genericObject = pooledObjects.Dequeue();
        }
        // Pool doesnt have free objects but it can grow as _maxNumberOfObjects > _objectsToPreAllocate
        else if (extraObjCount > 0)
        {
            genericObject = factoryMethod.Invoke();
            extraObjCount--;
        }
        return genericObject;
    }

    public void AddBackToPool(T returningObject)
    {
        // Checking if returning object is not null, current size of pool not greater than_maxNumberOfObjects and returning object type is same as pool type
        if (!returningObject.Equals(default(T)) && GetNumberOfObjectsInPool() < maxNumberOfObjects && returningObject.GetType().Equals(typeof(T)))
        {
            pooledObjects.Enqueue(returningObject);
        }
    }

    public int GetNumberOfObjectsInPool()
    {
        return pooledObjects.Count;
    }

    public void DestroyObjectsInPool()
    {
        pooledObjects.Clear();
    }

    public void SetMaxNumberOfObjects(int maxObjects)
    {
        maxNumberOfObjects = maxObjects;
        extraObjCount = maxNumberOfObjects - objToPreAllocate;
    }
}