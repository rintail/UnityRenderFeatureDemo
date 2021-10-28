using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveRenderTextureToFile : MonoBehaviour
{
    [MenuItem("Assets/Save RenderTexture to file")]
    public static void SaveRTToFile()
    {
        RenderTexture rt = Selection.activeObject as RenderTexture;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes;
        bytes = tex.EncodeToPNG();

        string path = AssetDatabase.GetAssetPath(rt) + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to " + path);
    }

    [MenuItem("Assets/Save RenderTexture to file", true)]
    public static bool SaveRTToFileValidation()
    {
        return Selection.activeObject is RenderTexture;
    }

    public static void DumpRenderTexture(RenderTexture rt)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(Application.dataPath + "/render/a.png", tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }

    public static IEnumerator SavePng(Camera camera)
    {
        int width = Screen.width;
        int height = Screen.height;
        RenderTexture rt = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        camera.targetTexture = rt;
        camera.Render();
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;
        Rect rect = new Rect(0, 0, width, height);
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        yield return new WaitForEndOfFrame();
        camera.targetTexture = null;
        RenderTexture.active = currentActiveRT;
        byte[] pngBytes = screenShot.EncodeToPNG();
        string filePath = findFildName();
        File.WriteAllBytes(filePath, pngBytes);
        RenderTexture.ReleaseTemporary(rt);
        //释放 screenShot
        Destroy(screenShot);
    }

    private static string findFildName()
    {
        int index = 1;
        string folder = "render/";

        while (File.Exists(string.Format("{0}{1}.png", folder, index++))) continue;
        return string.Format("{0}{1}.png", folder, index);
    }

}