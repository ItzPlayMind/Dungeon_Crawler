using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTimer : MonoBehaviour
{
    public static NetworkTimer Create(float time, Action<object[]> onFinished, params object[] objects)
    {
        GameObject gb = new GameObject("Timer");
        gb.AddComponent<NetworkTimer>().Set(time, onFinished, objects);
        return gb.GetComponent<NetworkTimer>();
    }

    Action<object[]> onFinish;
    object[] objects;

    float time;
    float maxTime;
    public bool Running { get; private set; }

    void Set(float time, Action<object[]> onFinish, params object[] objects)
    {
        maxTime = time;
        this.time = time;
        this.onFinish = onFinish;
        this.objects = objects;
        Running = true;
    }

    private void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if (onFinish != null)
                onFinish.Invoke(objects);
            Abort();
        }
    }

    public void Abort()
    {
        onFinish = null;
        time = 0;
        Running = false;
        Destroy(gameObject);
    }
}
