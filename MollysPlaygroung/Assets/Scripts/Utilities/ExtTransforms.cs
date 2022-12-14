using UnityEngine;

public static class Transforms
{
    public static void DestroyChildren(this Transform t, bool destroyImmediatley = false)
    {
        foreach (Transform child in t)
        {
            if (destroyImmediatley)
                MonoBehaviour.DestroyImmediate(child.gameObject);
            else
                MonoBehaviour.Destroy(child.gameObject);
        }
    }
}