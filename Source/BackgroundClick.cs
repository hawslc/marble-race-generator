using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClick : MonoBehaviour
{
    void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)
        {
            FindObjectOfType<CreateMaster>().BackgroundClick();
        }
    }
}
