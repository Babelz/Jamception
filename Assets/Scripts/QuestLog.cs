using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum QuestState
{
    InProgress,
    Completed,
    Failed
}

public abstract class QuestTracker
{
    #region Vars
    private readonly string desc;

    private QuestState state;
    #endregion

    #region Properties
    public string Desc
    {
        get
        {
            return desc;
        }
    }
    public QuestState State
    {
        get
        {
            return state;
        }
    }
    #endregion

    public QuestTracker(string desc)
    {
        this.desc = desc;
    }

    protected abstract void UpdateState(ref QuestState state);

    public void UpdateState()
    {
        UpdateState(ref state);
    }
}

public class ManualQuestTracker : QuestTracker
{
    #region Vars
    private QuestState mark;
    #endregion

    public ManualQuestTracker(string desc)
        : base(desc)
    {
    }

    protected override void UpdateState(ref QuestState state)
    {
        state = mark;
    }

    public void MarkCompleted()
    {
        mark = QuestState.Completed;
    }
    public void MarkFailed()
    {
        mark = QuestState.Failed;
    }
    public void MarkInProgress()
    {
        mark = QuestState.InProgress;
    }
}

public class ConditionalQuestTracker : QuestTracker
{
    #region Vars
    private readonly Func<bool> condition;  
    #endregion

    public ConditionalQuestTracker(string desc, Func<bool> condition)
        : base(desc)
    {
        if (condition == null)
        {
            throw new ArgumentNullException("condition");
        }

        this.condition = condition;
    }

    protected override void UpdateState(ref QuestState state)
    {
    }
}

public class QuestLine : QuestTracker
{
    #region Vars
    private readonly QuestTracker[] quests;

    private QuestTracker currentQuest;
    #endregion

    public QuestLine(string desc, QuestTracker[] quests)
        : base(desc)
    {
        if (quests == null)
        {
            throw new ArgumentNullException("quests");
        }

        this.quests = quests;
    }

    protected override void UpdateState(ref QuestState state)
    {
        currentQuest.UpdateState();

        int questIndex = Array.IndexOf(quests, currentQuest);

        if (currentQuest.State == QuestState.Completed && questIndex < quests.Length - 1)
        {
            currentQuest = quests[questIndex];
        }

        state = Array.TrueForAll(quests, q => q.State == QuestState.Completed) ? QuestState.Completed : QuestState.InProgress;
    }
}


public class QuestLog
{
    #region Vars
    private readonly List<QuestTracker> quests;
    #endregion

    public QuestLog(List<QuestTracker> quests)
    {
        this.quests = quests;
    }
    public QuestLog()
        : this(new List<QuestTracker>())
    {

    }

    public void AddQuest(QuestTracker quest)
    {
        quests.Add(quest);
    }
    public void RemoveQuest(QuestTracker quest)
    {
        quests.Remove(quest);
    }
}
