using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileReaderModule : MonoBehaviour
{
    [SerializeField]
    private DanceField field = null;


    private string saveFilesDirectory = Path.Combine(Application.streamingAssetsPath, "Saves");

    public GameObject saveFilePrefab;
    public Transform filesPanel;

    private void Start()
    {
        Debug.Log(saveFilesDirectory);
    }

    public void Save()
    {
        string path = Path.Combine(saveFilesDirectory, "test.json");

        if(!Directory.Exists(saveFilesDirectory))
        {
            Directory.CreateDirectory(saveFilesDirectory);
        }

        if(!File.Exists(path))
        {
            File.Create(path);
        }

        string content = string.Join("#", field.GetSaveStringsOnField().ToArray());

        File.WriteAllText(path, content);

    }

    public void Load()
    {
        char[] separator = { '#' };

        string path = Path.Combine(saveFilesDirectory, "test.json");
        string content = File.ReadAllText(path);

        List<string> result = new List<string>(content.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        field.InstenceActorsWithSettings(result);
    }
}
