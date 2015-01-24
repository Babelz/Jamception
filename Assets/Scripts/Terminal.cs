using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Stopwatch = System.Diagnostics.Stopwatch;

public class Terminal : MonoBehaviour
{
    #region Vars
    private TextAdventure adventure;
    private GUIStyle textStyle;

    private List<string> lines;

    private string currentLine;

    private Stopwatch inputTimer;

    private int maxLines;
    #endregion

    public Terminal()
    {
    }

    private void Start()
    {
        // Dialog played when adventure is fininshed.
        string finishedDialog = "";

        // Responses sent upon invalid answer.
        string[] wrongAnswerResponses = new string[]
        {
        };

        List<TextNode> nodes = new List<TextNode>()
        {
        };

        adventure = new TextAdventure(nodes, wrongAnswerResponses, finishedDialog);

        lines = new List<string>()
        {
            "TXT XTREME ADVENTURE 0.1"
        };

        textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.green;
        textStyle.fontSize = 18;

        inputTimer = Stopwatch.StartNew();

        maxLines = (int)(Screen.height / textStyle.CalcSize(new GUIContent(" ")).y) - 1;

        currentLine = string.Empty;
    }

    private void OnGUI()
    {
        int index = 0;

        foreach (string str in lines)
        {
            GUIContent line = new GUIContent(str);

            Vector2 textSize = textStyle.CalcSize(line);

            Rect textRect = new Rect(
                left: transform.position.x,
                top: transform.position.y + index * textSize.y,
                width: textSize.x,
                height: textSize.y);

            GUI.Label(textRect, line, textStyle);

            index++;
        }

        string input = Input.inputString;

        float height = textStyle.CalcSize(new GUIContent(" ")).y;

        Rect textAreaRect = new Rect(
            left: transform.position.x,
            top: Screen.height - height,
            width: Screen.width,
            height: height);

        if (!string.IsNullOrEmpty(input) && inputTimer.ElapsedMilliseconds > 25)
        {
            inputTimer = Stopwatch.StartNew();

            currentLine += input;

            GUI.Label(textAreaRect, currentLine, textStyle);
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            GUI.Label(textAreaRect, currentLine);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NewLine();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentLine.Length >= 1)
            {
                currentLine = currentLine.Substring(0, currentLine.Length - 1);
            }
        }
    }

    private void NewLine()
    {
        lines.Add(currentLine);

        if (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }

        currentLine = string.Empty;
    }
    private void ChangeBackgroundColor(Color color)
    {
        camera.backgroundColor = color;
    }
}

