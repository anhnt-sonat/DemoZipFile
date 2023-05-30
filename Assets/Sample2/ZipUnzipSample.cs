using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ZipUnzipSample : MonoBehaviour
{
    [SerializeField]
    string baseDirectryPath = "ZipPath";

    [SerializeField]
    string zipName = "zipfiles.zip";

    [SerializeField]
    private List<string> files = new List<string>();

    string zipPath
    {
        get
        {
            Directory.CreateDirectory(baseDirectryPath);
            return Path.Combine(baseDirectryPath, zipName);
        }
    }

    public void DownloadFile()
    {
        StartCoroutine(DownloadZipFile(Link));
    }


    public string Link = "https://dl.dropboxusercontent.com/u/56297224/twitter_icon.png.zip";

    private IEnumerator DownloadZipFile(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string savePath = "Assets/Resources/zip_file_name_demo.zip";

                System.IO.File.WriteAllBytes(savePath, www.downloadHandler.data);
                Debug.Log("anhnt: " + savePath);
            }
        }
    }


    public void Zip()
    {
       // files.Clear();
        //var fileOpen = File.ReadAllBytes(baseDirectryPath, );


        string path = "Assets/Resources/sample.txt";

        string[] lines = System.IO.File.ReadAllLines(path);

        //c.name = lines[Random.Range(0, lines.Length)];

        foreach (var item in lines)
        {
            Debug.Log(item);
        }

        foreach (var file in files)
        {
            if (File.Exists(file) == false)
            {
                Debug.LogError(file + "is not found!");
                System.Diagnostics.Process.Start(Path.GetDirectoryName(file));
                return;
            }
        }

        ZipUtil.Zip(zipPath, files.ToArray());
        System.Diagnostics.Process.Start(Path.GetDirectoryName(zipPath));
    }

    public void Unzip()
    {
        if (File.Exists(zipPath) == false)
        {
            Debug.LogError(zipPath + "is not found!");
            System.Diagnostics.Process.Start(Path.GetDirectoryName(zipPath));
            return;
        }

        ZipUtil.Unzip(zipPath, baseDirectryPath);
        System.Diagnostics.Process.Start(Path.GetDirectoryName(zipPath));
    }

    public void OpenDir()
    {
        Directory.CreateDirectory(baseDirectryPath);
        System.Diagnostics.Process.Start(baseDirectryPath);
    }
}
