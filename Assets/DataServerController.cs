using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DataServerController : MonoBehaviour
{
    string path = "https://file-examples.com/wp-content/storage/2017/02/zip_2MB.zip";
    private void Start()
    {
        StartCoroutine(DownloadZipFile(path));
    }
    private IEnumerator DownloadZipFile(string url)
    {
        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.Send();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string savePath = string.Format("{0}/{1}.zip", "~/Assets/Resources/", "zip_file_name_demo");
            System.IO.File.WriteAllBytes(savePath, www.downloadHandler.data);
            Debug.Log("anhnt: " + savePath);
        }
    }
}
