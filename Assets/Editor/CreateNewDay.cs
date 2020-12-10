using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.SceneManagement;
using System;

public class CreateNewDay : ScriptableWizard
{
    public int DayID = 0;

    [MenuItem("Tools/Create New Day")]
    private static void CreateWizard()
    {
        DisplayWizard<CreateNewDay>("Create New Day", "Create");
    }

    private void OnWizardCreate()
    {
        string dayString = "Day" + DayID;

        DirectoryInfo folder = new DirectoryInfo(Path.Combine(Application.dataPath, dayString));
        
        if (folder.Exists)
        {
            Debug.LogError("Day already exists : " + DayID);
            return;
        }

        Directory.CreateDirectory(folder.ToString());

        FileInfo sceneFile = new FileInfo(Path.Combine(folder.ToString(), dayString + ".unity"));
        FileInfo templateScene = new FileInfo(Path.Combine(Application.dataPath, "Resources", "TemplateScene.unity"));
        templateScene.CopyTo(sceneFile.ToString());

        FileInfo scriptInfo = new FileInfo(Path.Combine(folder.ToString(), dayString + ".cs"));
        TextAsset templateScript = Resources.Load("Template") as TextAsset;
        string script = templateScript.text.Replace("Template", dayString);
        File.WriteAllText(scriptInfo.ToString(), script);

        AssetDatabase.Refresh();
    }
}
