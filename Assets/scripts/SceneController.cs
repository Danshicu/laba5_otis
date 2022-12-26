using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneController : MonoBehaviour
{
    //public SimpleAutoSave saver;
    public GameObject startCanvas;
    public bool IsActive = false;
    public EntryPoint AllInOne;

    private void Awake()
    {
        startCanvas.SetActive(false);
        AllInOne = this.gameObject.GetComponent<EntryPoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsActive)
            {
                startCanvas.SetActive(false);
            }
            else
            {
                startCanvas.SetActive(true);
            }
        IsActive=!IsActive;
        }
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void SaveThisSceneAs(string name)
    {
        string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
        path[path.Length - 1] = name + ".txt";
        string newpath = string.Join("/", path);
        Debug.Log(newpath);
        AllInOne.SaveAll(newpath);
    }
}
