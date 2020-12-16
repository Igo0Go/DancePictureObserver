using System;

public class PictureDataHolder
{
    public int index;
    public string description;
    public string configuration;
    public int timeCycleCount;
    public byte[] miniaturePictureTexture;

    private PictureDataHolder()
    {
        description = string.Empty;
        configuration = string.Empty;
        timeCycleCount = 8;
    }

    public PictureDataHolder(int number)
    {
        index = number;
        description = string.Empty;
        configuration = string.Empty;
        timeCycleCount = 8;
    }

    public string GetSaveString()
    {
        return string.Format("Configuration: {0}\n\r" +
            "Texture: {1}\n\r" +
            "Description: {2}\n\r" +
            "TimeCycleCount: {3}",
            configuration, GetMiniatureDataAsString(), description, timeCycleCount);
    }

    public static PictureDataHolder GetLoadHolder(string data)
    {
        string[] separator = { "\n\r" };
        string[] options = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        PictureDataHolder result = new PictureDataHolder()
        {
            configuration = options[0].Replace("Configuration: ", string.Empty),
            miniaturePictureTexture = GetTextureDataFromString(options[1].Replace("Texture: ", string.Empty)),
            description = options[2].Replace("Description: ", string.Empty),
            timeCycleCount = int.Parse(options[3].Replace("TimeCycleCount: ", string.Empty))
        };

        return result;
    }

    public static PictureDataHolder GetCopy(PictureDataHolder input)
    {
        return new PictureDataHolder()
        {
            configuration = input.configuration,
            description = input.description,
            timeCycleCount = input.timeCycleCount,
            miniaturePictureTexture = input.miniaturePictureTexture
        };
    }

    private static byte[] GetTextureDataFromString(string data)
    {
        char[] separator = { '|' };
        string[] items = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        byte[] result = new byte[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            result[i] = byte.Parse(items[i]);
        }

        return result;
    }

    private string GetMiniatureDataAsString()
    {
        string result = string.Empty;

        for (int i = 0; i < miniaturePictureTexture.Length; i++)
        {
            result += miniaturePictureTexture[i] + "|";
        }

        return result;
    }
}
