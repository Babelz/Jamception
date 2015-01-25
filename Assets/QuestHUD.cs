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


public sealed class QuestHUD : MonoBehaviour
{
    #region Static vars
    private static readonly List<IQuestSet> questsSets;
    #endregion

    #region Vars
    private GUIStyle textStyle;
    private Camera playerCamera;
    public QuestLog questLog;

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

        var q = set.GetQuests();

        for (int i = 0; i < q.Count; i++)
        {
            questLog.AddQuest(q[i]);
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
        int index = 0;

        foreach (QuestTracker q in questLog)
        {
            float height = (new GUIStyle()).CalcSize(new GUIContent(" ")).y;
            
            Color c;
            
            switch (q.State)
            {
                case QuestState.Completed:
                    c = Color.green;
                    break;
                case QuestState.Failed:
                    c = Color.red;
                    break;
                case QuestState.InProgress:
                default:
                    c = Color.white;
                    break;
            }

            GUIStyle s = new GUIStyle() { fontSize = 18 };
            s.normal.textColor = c;

            GUI.Label(new Rect(0f, height * index, 100f, height), q.Desc, s);

            QuestLine l = q as QuestLine;

            if (l != null)
            {
                index++;

                float offset = 50f;

                foreach (QuestTracker t in l.Quests())
                {
                    switch (t.State)
                    {
                        case QuestState.Completed:
                            c = Color.green;
                            break;
                        case QuestState.Failed:
                            c = Color.red;
                            break;
                        case QuestState.InProgress:
                        default:
                            c = Color.white;
                            break;
                    }

                    s.normal.textColor = c;

                    GUI.Label(new Rect(offset, index * height, 100f, height), l.Desc, s);

                    index++;
                }
            }

            index++;
        }
    }
	
	// Update is called once per frame
	private void Update() 
    {
        questLog.Update();
	}
}
