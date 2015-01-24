﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

using Random = System.Random;

public sealed class TextAdventure
{
    #region Vars
    private readonly string[] wrongAnswerDialogs;
    private readonly string finishedDialog;

    private readonly List<TextNode> nodes;

    private readonly Random random;

    private TextNode currentNode;
    private int index;
    #endregion

    #region Properties
    public bool Fininshed
    {
        get
        {
            return index < nodes.Count;
        }
    }
    public TextNode CurrentNode
    {
        get
        {
            return currentNode;
        }
    }
    #endregion

    public TextAdventure(List<TextNode> nodes, string[] wrongAnswerDialogs, string finishedDialog)
    {
        this.nodes = nodes;
        this.wrongAnswerDialogs = wrongAnswerDialogs;
        this.finishedDialog = finishedDialog;

        currentNode = nodes[index];
    }

    public void Reset()
    {
        index = 0;
        currentNode = nodes[index];
    }
    public List<string> Play(string answer)
    {
        List<string> response = new List<string>();

        if (Fininshed)
        {
            throw new InvalidOperationException("Adventure has been fininshed. Reset it to continue.");
        }

        // If right answer, continue.
        if (currentNode.IsRightAnswer(answer))
        {
            // Add right answer dialog to response.
            response.Add(currentNode.GetResponseString(answer));

            int lastIndex = index;
            index = currentNode.GetJumpIndex(answer);

            if (index < 0)
            {
                // No jump. Just get next node.
                index = lastIndex++;
            }

            // If andventure is fininshed, add finished dialog and return response.
            if (Fininshed)
            {
                response.Add(finishedDialog);

                return response;
            }

            currentNode = nodes[index];
        }
        else
        {
            response.Add(wrongAnswerDialogs[random.Next(0, wrongAnswerDialogs.Length - 1)]);
        }

        return response;
    }
}

public static class AdventureBuilder
{
    public static Dictionary<string, int> CreateAnswers()
    {
        return new Dictionary<string, int>();
    }
    /// <summary>
    /// Adds new answer to the dict.
    /// </summary>
    /// <param name="answer">Answer.</param>
    /// <param name="jumpIndex">Jump index. -1 index means there will be no jump and next node will be selected upon right answer.</param>
    public static Dictionary<string, int> AddAnswer(this Dictionary<string, int> dict, string answer, int jumpIndex) 
    {
        dict.Add(answer, jumpIndex);

        return dict;
    }
}

public sealed class TextNode
{
    #region Vars
    // Jump index and answers.
    private readonly Dictionary<string, int> answers;
    private readonly string[] responses;

    private readonly string rightAnswerDialog;
    private readonly string enterDialog;
    #endregion

    #region Properties
    public string EnterDialog
    {
        get
        {
            return enterDialog;
        }
    }
    #endregion

    /// <summary>
    /// Initializes new instance of TextNode.
    /// </summary>
    /// <param name="enterDialog">Dialog wich will be played when this node is entered.</param>
    /// <param name="responses">Dialogs displayed when player gives the right answer.</param>
    /// <param name="answers">Answers and their jump indexes. -1 index means there will be no jump.</param>
    public TextNode(string enterDialog, string[] responses, Dictionary<string, int> answers)
    {
        if (responses.Length != answers.Count)
        {
            throw new InvalidOperationException("There must be same number of responses as there are answers.");
        }

        this.enterDialog = enterDialog;
        this.responses = responses;
        this.answers = answers;
    }

    public bool IsRightAnswer(string answer)
    {
        return answers.ContainsKey(answer);
    }
    public int GetJumpIndex(string answer)
    {
        return answers[answer];
    }
    public string GetResponseString(string answer)
    {
        int index = 0;

        for (int i = 0; i < answers.Count; i++)
        {
            if (answers.ElementAt(i).Key == answer)
            {
                index = i;
            }
        }

        return responses[index];
    }
}