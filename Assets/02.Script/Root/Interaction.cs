using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    public GameObject InteractionBtnImg;

    public void InteractionOnOff(bool isActive)
    {
        InteractionBtnImg.SetActive(isActive);
    }

    public abstract void InteractionFunc();
}