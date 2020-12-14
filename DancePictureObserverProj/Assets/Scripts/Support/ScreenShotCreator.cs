using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenShotCreator : MonoBehaviour
{
    [SerializeField]
    private RenderTexture texture = null;
    [SerializeField]
    private Camera cam = null;


    public void ShowScreen()
    {
        texture.Release();
        var image = CreateScreenshot();

        //texture.width = image.width;
        //texture.height = image.height;

        //Graphics.CopyTexture(texture, image);
    }

    public Texture2D CreateScreenshot()
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        // Render the camera's view.
        cam.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
}
