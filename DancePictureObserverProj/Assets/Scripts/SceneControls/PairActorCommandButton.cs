using UnityEngine;

/// <summary>
/// Класс исполнителя-пары
/// </summary>
public class PairActorCommandButton : ActorCommandButton
{
    [SerializeField]
    private Transform girlActor = null;
    [SerializeField]
    private Transform manActor = null;

    [SerializeField]
    private GameObject pairArmsDownIcon = null;
    [SerializeField]
    private GameObject pairArmsUpIcon = null;

    /// <summary>
    /// Поменять исполнителей в паре местами
    /// </summary>
    public void ChangePairActorsPositions()
    {
        girlActor.localPosition = new Vector3(girlActor.localPosition.x * -1, girlActor.localPosition.y, girlActor.localPosition.z);
        manActor.localPosition = new Vector3(manActor.localPosition.x * -1, manActor.localPosition.y, manActor.localPosition.z);
    }

    /// <summary>
    /// Подняь/опустить руки
    /// </summary>
    public void ChangePairArmsConfiguration()
    {
        pairArmsDownIcon.SetActive(!pairArmsDownIcon.activeSelf);
        pairArmsUpIcon.SetActive(!pairArmsUpIcon.activeSelf);
    }
}
