using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;

public class ZipUnzipSample : MonoBehaviour
{
    [SerializeField]
    string baseDirectryPath = "Assets/Resources/";

    [SerializeField]
    string zipName = "zipfiles.zip";

    [SerializeField]
    private List<string> files = new List<string>();

    [SerializeField]
    private Text txtCurrentVersion;

    [SerializeField] private int VersionDownload = 0;

    private int CurrentVersion
    {
        get => PlayerPrefs.GetInt("CurrentVersion");
        set
        {
            PlayerPrefs.SetInt("CurrentVersion", value);
            txtCurrentVersion.text = value.ToString();
        }
    }

    private void Awake()
    {
        txtCurrentVersion.text = CurrentVersion.ToString();
    }

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
        if (VersionDownload <= CurrentVersion)
        {
            Debug.Log("Version download <= current version");
            yield return null;
        }
        else
        {
            Debug.Log("downloading...");

            using UnityWebRequest www = UnityWebRequest.Get(url);

            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                DeleteFile();

                System.IO.File.WriteAllBytes(baseDirectryPath + zipName, www.downloadHandler.data);

                Debug.Log("anhnt: write file ok" + baseDirectryPath);

                Invoke(nameof(Unzip), 0.1f);

                CurrentVersion = VersionDownload;
                //Invoke(nameof(OpenDir), 1f);
            }
        }

    }


    public void Zip()
    {
        // files.Clear();
        //var fileOpen = File.ReadAllBytes(baseDirectryPath, );

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

        Debug.Log("anhnt: unzip file ok" + baseDirectryPath);

    }

    public void OpenDir()
    {
        Directory.CreateDirectory(baseDirectryPath);
        System.Diagnostics.Process.Start(baseDirectryPath);
        Debug.Log("anhnt: open dir ok" + baseDirectryPath);

    }

    public void DeleteFile()
    {
        if (Directory.Exists(baseDirectryPath)) { Directory.Delete(baseDirectryPath, true); }
        Directory.CreateDirectory(baseDirectryPath);

        Debug.Log("anhnt: delete file ok" + baseDirectryPath);

    }
}
