using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;


public enum JumpType
{
    OnRightAnswer,
    OnWrongAnswer,
    Both
}

public sealed class TextNode
{
    #region Vars
    public readonly string dialog;
    public readonly string rightAnswer;
    public readonly string rightAnswerDialog;
    public readonly int jump;
    public JumpType jumpType;
    #endregion

    // -1 = no jump.

    public TextNode(string dialog, string rightAnswer, string rightAnswerDialog, int jump = -1, JumpType jumpType = JumpType.OnRightAnswer)
    {
        this.dialog = dialog;
        this.rightAnswer = rightAnswer;
        this.rightAnswerDialog = rightAnswerDialog;
        this.jump = jump;
        this.jumpType = jumpType;
    }
}


public sealed class TextAdventure
{
    #region Vars
    private readonly string[] wrongAnswerResponses;
    private readonly TextNode[] nodes;
    private readonly Random random;

    private TextNode current;

    private int index;
    #endregion

    #region Properties
    public bool AtEnd
    {
        get
        {
            return index >= nodes.Length;
        }
    }
    public string CurrentDialog
    {
        get
        {
            return current.dialog;
        }
    }
    #endregion

    public TextAdventure(TextNode[] nodes, string[] wrongAnswerResponses)
    {
        this.nodes = nodes;
        this.wrongAnswerResponses = wrongAnswerResponses;

        random = new Random();

        current = nodes[index];
    }

    public List<string> Play(string text)
    {
        List<string> results = new List<string>();

        if (string.Equals(text, current.rightAnswer) || string.IsNullOrEmpty(current.rightAnswer))
        {
            results.Add(current.rightAnswerDialog);

            if (current.jump != -1 && current.jumpType == JumpType.OnRightAnswer || current.jumpType == JumpType.Both)
            {
                index = current.jump;
            }
            else
            {
                index++;
            }

            if (AtEnd)
            {
                return results;
            }

            current = nodes[index];
        }
        else
        {
            if (current.jump != -1 && current.jumpType == JumpType.OnWrongAnswer || current.jumpType == JumpType.Both)
            {
                index = current.jump;

                if (AtEnd)
                {
                    results.Add(wrongAnswerResponses[random.Next(0, wrongAnswerResponses.Length - 1)]);
                }
                else
                {
                    current = nodes[index];
                }
            }

            results.Add(wrongAnswerResponses[random.Next(0, wrongAnswerResponses.Length - 1)]);
        }

        return results;
    }
}