using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Stopwatch = System.Diagnostics.Stopwatch;

public class Terminal : MonoBehaviour
{
    #region Vars
    private TextAdventure adventure;
    private GUIStyle inputTextStyle; 
    private GUIStyle outputTextStyle;

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
        string finishedDialog = "The END";

        // Responses sent upon invalid answer.
        string[] wrongAnswerDialogs = new string[]
        {
            "Väärä vastaus tollo!"
        };

        // Adventure nodes.
        List<TextNode> nodes = new List<TextNode>()
        {
            new TextNode(
                "A group of four engineer students, Pidgin, Siquel, Bab and Eeneku have arrived at the game jam site in Kajaani.\nA mighty task lies ahead of them, for they must be able to complete a video game in mere 48 hours!\nAfter a while of planning, the group gets to work on their game. Hours pass, and the team is making good progress.\nSoon however, the team runs into a strange code error.\nThe error is not critical, but it could hamper their progress later.\nWhat should the team do?\n\n- solve\n- ignore",
                new string[] { "Ignoring...", "Lets solve deim problemz!" },
                AdventureBuilder.CreateAnswers()
                .AddAnswer("ignore")
                .AddAnswer("solve"))
        };

        adventure = new TextAdventure(nodes, wrongAnswerDialogs, finishedDialog);
     
        // Terminal lines.
        adventure = new TextAdventure(nodes, wrongAnswerDialogs, finishedDialog);

        lines = new List<string>()
        {
            "AT THE GLOBAL GAME JAM 2015, FINLAND, KAJAANI...", 
            adventure.CurrentNode.EnterDialog
        };

        // Input style.
        inputTextStyle = new GUIStyle();
        inputTextStyle.normal.textColor = Color.green;
        inputTextStyle.fontSize = 18;

        // Output style.
        outputTextStyle = new GUIStyle();
        outputTextStyle.normal.textColor = Color.white;
        outputTextStyle.fontSize = 18;

        inputTimer = Stopwatch.StartNew();

        maxLines = (int)(Screen.height / inputTextStyle.CalcSize(new GUIContent(" ")).y) - 2;

        currentLine = string.Empty;
    }

    private void OnGUI()
    {
        int index = 0;

        List<string> parsedLines = new List<string>();

        foreach (string str in lines)
        {
            if (str.Contains("\n"))
            {
                string[] newLines = str.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

                if(newLines.Length > 0) 
                {
                    parsedLines.AddRange(newLines);
                }

                continue;
            }

            parsedLines.Add(str);
        }

        foreach (string str in parsedLines)
        {
            GUIStyle textStyle = str.StartsWith(">") ? outputTextStyle : inputTextStyle;

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

        float height = inputTextStyle.CalcSize(new GUIContent(" ")).y;

        Rect textAreaRect = new Rect(
            left: transform.position.x,
            top: Screen.height - height,
            width: Screen.width,
            height: height);

        if (!string.IsNullOrEmpty(input) && inputTimer.ElapsedMilliseconds > 25)
        {
            inputTimer = Stopwatch.StartNew();

            currentLine += input;

            GUI.Label(textAreaRect, currentLine, inputTextStyle);
        }

        if (!string.IsNullOrEmpty(currentLine))
        {
            GUI.Label(textAreaRect, currentLine);
        }
    }

    private void NewLine()
    {
        // New line.
        lines.Add(">" + currentLine);
        currentLine = string.Empty;
    }
    private void RemoveLines()
    {
        while (lines.Count > maxLines)
        {
            lines.RemoveAt(0);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            List<string> newLines = new List<string>();

            if (adventure.Finished)
            {   
                NewLine();

                RemoveLines();

                return;
            }

            // Get response.
            newLines.AddRange(adventure.Play(currentLine));

            // If we are fininshed, just add the fininshed dialog to the end.
            if (!adventure.Finished)
            {
                // Add enter dialog. Its the current or next nodes dialog.
                newLines.Add(adventure.CurrentNode.EnterDialog);
            }

            NewLine();

            foreach (string newLine in newLines)
            {
                lines.Add(newLine);
            }

            RemoveLines();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (currentLine.Length >= 1)
            {
                currentLine = currentLine.Substring(0, currentLine.Length - 1);
            }
        }
    }

    private void ChangeBackgroundColor(Color color)
    {
        camera.backgroundColor = color;
    }
}

