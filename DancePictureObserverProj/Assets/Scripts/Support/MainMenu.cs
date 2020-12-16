using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel = null;

    [SerializeField]
    private FileReaderModule readerModule = null;

    [SerializeField]
    private InputField danceNameInputField = null;

    [SerializeField]
    private GameObject tableItemPrefab = null;

    [SerializeField]
    private Transform tableContent = null;

    private List<PictureDataHolder> holders = new List<PictureDataHolder>();

    #region Основные функции

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        StartCoroutine(Redraw());
    }

    public void ClearAll()
    {
        holders.Clear();
        StartCoroutine(Redraw());
    }

    public void CreateNew()
    {
        var newHolder = new PictureDataHolder(holders.Count);
        holders.Add(newHolder);
        Instantiate(tableItemPrefab, tableContent).GetComponent<PictureTableItem>().SetUp(this, newHolder);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SaveAll()
    {
        readerModule.Save(holders, danceNameInputField.text);
    }

    public void Load()
    {
        var data = readerModule.Load();
        holders = data.dataHolders;
        danceNameInputField.text = data.danceName;
        StartCoroutine(Redraw());
    }

    #endregion

    #region Управление элементами

    public void MoveItemUp(PictureDataHolder holder)
    {
        if(holder.index > 0)
        {
            int index = holder.index;
            holders[index] = holders[index - 1];
            holders[index - 1] = holder;
            holders[index - 1].index--;
            holders[index].index++;
            StartCoroutine(Redraw());
        }
    }

    public void MoveItemDown(PictureDataHolder holder)
    {
        if(holder.index < holders.Count-1)
        {
            int index = holder.index;
            holders[index] = holders[index + 1];
            holders[index + 1] = holder;
            holders[index + 1].index++;
            holders[index].index--;
            StartCoroutine(Redraw());
        }
    }

    public void DeleteItem(PictureDataHolder holder)
    {
        holders.Remove(holder);
        CheckIndexes();
        StartCoroutine(Redraw());
    }

    public void CreateDuplicate(PictureDataHolder holder)
    {
        var newHolder = PictureDataHolder.GetCopy(holder);
        newHolder.index = holders.Count;
        holders.Add(newHolder);
        Instantiate(tableItemPrefab, tableContent).GetComponent<PictureTableItem>().SetUp(this, newHolder);
    }

    public void EditItem(PictureDataHolder holder)
    {
        readerModule.buferHolder = holder;
        menuPanel.SetActive(false);
        readerModule.LoadPictureConfiguration();
    }

    #endregion

    #region Вспомогательные

    private IEnumerator Redraw()
    {
        int count = tableContent.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(tableContent.GetChild(i).gameObject, Time.deltaTime);
        }
        yield return null;

        foreach (var item in holders)
        {
            Instantiate(tableItemPrefab, tableContent).GetComponent<PictureTableItem>().SetUp(this, item);
        }
    }

    private void CheckIndexes()
    {
        for (int i = 0; i < holders.Count; i++)
        {
            holders[i].index = i;
        }
    }

    #endregion
}
