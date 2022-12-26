using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckForEntering : MonoBehaviour
{
    public SceneController thisGame;
    public Text editingText;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (editingText.text != "")
            {
                Debug.Log(editingText.text);
                thisGame.SaveThisSceneAs(editingText.text);
            }
        }
    }
}
