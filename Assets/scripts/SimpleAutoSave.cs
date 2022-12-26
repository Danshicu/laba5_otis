using UnityEngine;
using UnityEditor;

public class SimpleAutoSave : EditorWindow
{
    float saveTime = 20f;
    float nextSave = 0f;

    [MenuItem("Example/Simple autoSave")]
    static void Init()
    {
        SimpleAutoSave window = EditorWindow.GetWindowWithRect<SimpleAutoSave>(new Rect(0, 0, 200, 40), true);
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("Save Each:", saveTime + " Secs");
        int timeToSave = (int)(nextSave - EditorApplication.timeSinceStartup);
        EditorGUILayout.LabelField("Next Save:", timeToSave.ToString() + " Sec");
        this.Repaint();

        if (EditorApplication.timeSinceStartup > nextSave)
        {
            string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
            path[path.Length - 1] = "AutoSave_" + path[path.Length - 1];
            EditorApplication.SaveScene(string.Join("/", path), true);
            Debug.Log("Saved Scene");
            nextSave = (int)(EditorApplication.timeSinceStartup + saveTime);
        }
    }

    public void OnDestroy()
    {
        string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
        path[path.Length - 1] = "AutoSave_" + path[path.Length - 1];
        EditorApplication.SaveScene(string.Join("/", path), true);
        Debug.Log("Saved Scene");
        nextSave = (int)(EditorApplication.timeSinceStartup + saveTime);
    }
}