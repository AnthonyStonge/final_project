using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenuScript : MonoBehaviour
{
    public void QuitApplication()
    {
        Application.Quit();
        #if UNITY_EDITOR
        Debug.Log("Quit Application");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
