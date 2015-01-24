using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

public interface IQuestSet
{
    #region Properties
    string Name 
    {
        get;
    }
    #endregion

    List<QuestTracker> GetQuests();
}

public sealed class FirstMapQuestSet : IQuestSet
{
    #region Properties
    public string Name
    {
        get
        {
            return "3DQuestsMapQuests";
        }
    }
    #endregion

    public List<QuestTracker> GetQuests()
    {
        List<QuestTracker> quests = new List<QuestTracker>();

        return quests;
    }
}

public struct QuestLabelModel
{
    public readonly string text;
    public readonly Color color;
    public readonly bool child;
    public readonly Rect rect;

    public QuestLabelModel(string text, Color color, bool child, Rect rect)
    {
        this.text = text;
        this.color = color;
        this.child = child;
        this.rect = rect;
    }
}


public sealed class QuestHUD : MonoBehaviour
{
    #region Static vars
    private static readonly List<IQuestSet> questsSets;
    #endregion

    #region Vars
    private GUIStyle textStyle;
    private Camera playerCamera;
    private QuestLog questLog;

    public string questSetName;
    #endregion

    static QuestHUD()
    {
        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        List<Type> types = new List<Type>();

        assemblies.ForEach(a => types.AddRange(a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IQuestSet)))));

        questsSets = types
            .Select<Type, IQuestSet>(t => Activator.CreateInstance(t) as IQuestSet)
            .ToList();

        Debug.Log("Sets: " + questsSets.Count);
        Debug.Log("Asms: " + assemblies.Count);
        Debug.Log("Types: " + types.Count);

        //questsSets.Add(new FirstMapQuestSet());
    }

    private void AddQuests()
    {
        IQuestSet set = questsSets.Find(c => c.Name == questSetName);

        if (set == null)
        {
            Debug.LogError("Invalid quest set name.");
            
            Application.Quit();

            return;
        }

        List<QuestTracker> quests = new List<QuestTracker>();

        for (int i = 0; i < quests.Count; i++)
        {
            questLog.AddQuest(quests[i]);
        }
    }

	// Use this for initialization
    private void Start()
    {
        questLog = new QuestLog();

        playerCamera = GameObject.Find("PlayerCamera").camera;

        AddQuests();

        if (playerCamera == null)
        {
            Debug.LogError("Camera cant be null!");
            
            Application.Quit();

            return;
        }
    }
    private void OnGUI()
    {
        // Trans where we want to draw our gui.
        Transform transForm = playerCamera.transform;
    }
	
	// Update is called once per frame
	private void Update() 
    {
	    
	}
}
