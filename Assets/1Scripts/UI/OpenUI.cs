using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUI : MonoBehaviour
{
    public GameObject button;

    public void ToggleUI()
    {
        if(button.activeSelf)
        {
            button.SetActive(false);
        }
        else
        {
            button.SetActive(true);
        }
    }


}
