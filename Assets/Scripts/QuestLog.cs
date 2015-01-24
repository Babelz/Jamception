using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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

        state = QuestState.InProgress;
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
    private readonly UpdateQuestStateDelegate updateStateDelegate;
    #endregion

    public ConditionalQuestTracker(string desc, UpdateQuestStateDelegate updateStateDelegate)
        : base(desc)
    {
        if (updateStateDelegate == null)
        {
            throw new ArgumentNullException("condition");
        }

        this.updateStateDelegate = updateStateDelegate;
    }

    protected override void UpdateState(ref QuestState state)
    {
        updateStateDelegate(ref state);
    }

    public delegate void UpdateQuestStateDelegate(ref QuestState state);
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

        currentQuest = quests[0];
    }

    public void Update()
    {
        foreach (QuestTracker quest in quests.Where(q => q.State == QuestState.InProgress))
        {
            quest.UpdateState();
        }
    }

    protected override void UpdateState(ref QuestState state)
    {
        currentQuest.UpdateState();

        int questIndex = Array.IndexOf(quests, currentQuest);

        if (currentQuest.State == QuestState.Completed && questIndex < quests.Length - 1)
        {
            currentQuest = quests[questIndex];
        }

        // If one quest has failed, line has failed.
        if (Array.Find(quests, q => q.State == QuestState.Failed) != null)
        {
            state = QuestState.Failed;
        }

        // If all quests are completed, the line is complete.
        state = Array.TrueForAll(quests, q => q.State == QuestState.Completed) ? QuestState.Completed : QuestState.InProgress;
    }

    public IEnumerable<QuestTracker> Quests()
    {
        return quests;
    }
}


public class QuestLog : IEnumerable<QuestTracker>
{
    #region Vars
    private readonly List<QuestTracker> quests;
    #endregion

    #region Properties
    public int QuestsCount
    {
        get
        {
            return quests.Count;
        }
    }
    public int CompletedQuests
    {
        get
        {
            return quests.Count(q => q.State == QuestState.Completed);
        }
    }
    public int FailedQuests
    {
        get
        {
            return quests.Count(q => q.State == QuestState.Failed);
        }
    }
    public int QuestsInProgress
    {
        get
        {
            return quests.Count(q => q.State == QuestState.InProgress);
        }
    }
    #endregion

    public QuestLog(List<QuestTracker> quests)
    {
        this.quests = quests;
    }
    public QuestLog()
        : this(new List<QuestTracker>())
    {
    }

    public void Update()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            quests[i].UpdateState();
        }
    }

    public void AddQuest(QuestTracker quest)
    {
        if (quests.Contains(quest))
        {
            quests.Add(quest);
        }
    }
    public bool RemoveQuest(QuestTracker quest)
    {
        return quests.Remove(quest);
    }

    public IEnumerator<QuestTracker> GetEnumerator()
    {
        return quests.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
