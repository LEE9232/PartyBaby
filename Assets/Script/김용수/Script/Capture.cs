using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Capture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;
    public Image bg;

    public GameObject[] obj;

    int count = 0;

    private void Start()
    {
        cam = Camera.main;
        cam.backgroundColor = new Color32(116,185,233,255);
    }

    public void Create()
    {
        StartCoroutine(Captureimage());
       
    }

    public void AllCreate()
    {
        StartCoroutine(AllCaptureImage());
    }

    IEnumerator Captureimage()
    {
        yield return null;

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

        yield return null;

        var data = tex.EncodeToPNG();
        string name = "Thumnail";
        string extention = ".png";
        string path = Application.persistentDataPath + "/Thumnail/";

        Debug.Log(path);

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        File.WriteAllBytes(path + name + extention, data);

        yield return null;


    }

    IEnumerator AllCaptureImage()
    {
        while(count < obj.Length)
        {
            var nowObj = Instantiate(obj[count].gameObject);


            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false, true);
            RenderTexture.active = rt;
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);

            yield return null;

            var data = tex.EncodeToPNG();
            string name = $"Thumnail_{ obj[count].gameObject.name}";
            string extention = ".png";
            string path = Application.persistentDataPath + "/Thumnail/";

            Debug.Log(path);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            File.WriteAllBytes(path + name + extention, data);

            yield return null;

            DestroyImmediate(nowObj);
            count++;

            yield return null;
        }
    }

   



}
