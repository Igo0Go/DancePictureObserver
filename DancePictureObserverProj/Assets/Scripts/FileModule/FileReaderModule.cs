using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileReaderModule : MonoBehaviour
{
    private string saveFilesDirectory = Path.Combine(Application.streamingAssetsPath, "Saves");

    public GameObject saveFilePrefab;
    public Transform filesPanel;

    private void Start()
    {
        Debug.Log(saveFilesDirectory);
    }
}
