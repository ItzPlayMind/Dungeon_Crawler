using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static JSONObject convertToJson(this Vector3 pos)
    {
        JSONObject obj = new JSONObject();
        obj.AddField("x", Mathf.Round(pos.x* 1000f));
        obj.AddField("y", Mathf.Round(pos.y * 1000f));
        obj.AddField("z", Mathf.Round(pos.z * 1000f));
        return obj;
    }
}
