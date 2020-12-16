using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileReaderModule : MonoBehaviour
{
    [SerializeField]
    private DanceField field = null;

    private string saveFilesDirectory = Path.Combine(Application.streamingAssetsPath, "Saves");

    public PictureDataHolder buferHolder;

    public void SavePictureConfiguration()
    {
        buferHolder.configuration = string.Join("#", field.GetSaveStringsOnField().ToArray());
    }

    public void LoadPictureConfiguration()
    {
        char[] separator = { '#' };
        List<string> result = new List<string>(buferHolder.configuration.Split(separator, StringSplitOptions.RemoveEmptyEntries));
        field.InstenceActorsWithSettings(result);
    }

    public void SavePictureMiniature(Texture2D texture)
    {
        buferHolder.miniaturePictureTexture = texture.EncodeToPNG();
    }

    public void Save(List<PictureDataHolder> pictureDataHolders, string danceName)
    {
        string saveString = string.Empty;
        saveString += "DanceName: " + danceName + "\r\n=======================\n\r";
        foreach (var item in pictureDataHolders)
        {
            saveString += item.GetSaveString() + "\r\n=======================\n\r";
        }
        string path = Path.Combine(saveFilesDirectory, "test.dance");

        if (!Directory.Exists(saveFilesDirectory))
        {
            Directory.CreateDirectory(saveFilesDirectory);
        }

        if (!File.Exists(path))
        {
            File.Create(path);
        }

        File.WriteAllText(path, saveString);
    }

    public (string danceName, List<PictureDataHolder> dataHolders) Load()
    {
        string[] separator = { "\r\n=======================\n\r" };

        string path = Path.Combine(saveFilesDirectory, "test.dance");
        string content = File.ReadAllText(path);

        List<PictureDataHolder> holders = new List<PictureDataHolder>();

        string[] items = content.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < items.Length; i++)
        {
            holders.Add(PictureDataHolder.GetLoadHolder(items[i]));
        }

        return (items[0].Replace("DanceName: ", string.Empty), holders);
    }
}
