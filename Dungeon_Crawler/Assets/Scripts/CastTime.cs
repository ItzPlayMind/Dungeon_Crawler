using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastTime : MonoBehaviour
{
    #region Singleton
    public static CastTime instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] TMPro.TextMeshProUGUI castTimeText;
    [SerializeField] Image fill;
    [SerializeField] GameObject GFX;
    Action<object[]> onFinish;
    object[] objects;

    float time;
    float maxTime;

    public bool Running { get; private set; }

    public void Set(float time, string text, Action<object[]> onFinish, params object[] objects)
    {
        castTimeText.text = text;
        maxTime = time;
        this.time = time;
        this.onFinish = onFinish;
        this.objects = objects;
        GFX.SetActive(true);
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
            if(onFinish != null)
                onFinish.Invoke(objects);
            Abort();
        }
        fill.fillAmount = time / maxTime;
    }

    public void Abort()
    {
        GFX.SetActive(false);
        onFinish = null;
        time = 0;
        Running = false;
    }
}
