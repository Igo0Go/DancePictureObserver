using UnityEngine;
using UnityEngine.UI;

public class PictureTableItem : MonoBehaviour
{
    [SerializeField]
    private InputField timeCycleCountInputField = null;

    [SerializeField]
    private InputField descriptionInputField = null;

    [SerializeField]
    private RawImage miniature = null;

    private MainMenu menu;
    private PictureDataHolder dataHolder;

    public void SetUp(MainMenu mainMenu, PictureDataHolder pictureDataHolder)
    {
        menu = mainMenu;
        dataHolder = pictureDataHolder;
        timeCycleCountInputField.text = pictureDataHolder.timeCycleCount.ToString();
        descriptionInputField.text = pictureDataHolder.description;

        if(pictureDataHolder.miniaturePictureTexture != null)
        {
            Texture2D bufer = new Texture2D(256, 128);
            bufer.LoadImage(pictureDataHolder.miniaturePictureTexture);
            miniature.texture = bufer;
//            Graphics.CopyTexture(bufer, 0, 0, miniature.texture,0,0);
        }
    }

    public void MoveUp()
    {
        menu.MoveItemUp(dataHolder);
    }
    public void MoveDown()
    {
        menu.MoveItemDown(dataHolder);
    }
    public void Edit()
    {
        menu.EditItem(dataHolder);
    }
    public void Delete()
    {
        menu.DeleteItem(dataHolder);
    }
    public void CreateDuplicate()
    {
        menu.CreateDuplicate(dataHolder);
    }

    public void OnTimeCycleChanged()
    {
        if(int.TryParse(timeCycleCountInputField.text, out int bufer))
        {
            if(bufer > 0)
            {
                dataHolder.timeCycleCount = bufer;
            }
            else
            {
                timeCycleCountInputField.text = 8.ToString();
            }
        }
    }

    public void OnDescriptionChanged()
    {
        dataHolder.description = descriptionInputField.text;
    }
}
