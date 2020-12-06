using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

        string content = JsonHelper.ToJson<ActorJSONHolder>(field.GetHoldersOnField().ToArray());

        File.WriteAllText(path, content);

    }

    public void Load()
    {
        string path = Path.Combine(saveFilesDirectory, "test.json");
        string content = File.ReadAllText(path);
        List<ActorJSONHolder> result = new List<ActorJSONHolder>(JsonHelper.FromJson<ActorJSONHolder>(content));
        field.InstenceActorsWithSettings(result);
    }
}
