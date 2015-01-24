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
        // Dialog played when adventure is finished.
        string finishedDialog = "The game is now completed. All the trials of friendship and endurance are now over,\nand the team faces the age-old question.";

        // Responses sent upon invalid answer.
        string[] wrongAnswerDialogs = new string[]
        {
            "Wrong answer!",
            "Try that again, please!"
        };

        // Adventure nodes.
        List<TextNode> nodes = new List<TextNode>()
        {
            new TextNode(
                "A group of four engineer students, Pidgin, Siquel, Bab and Eeneku have arrived at the game jam site in Kajaani.\nA mighty task lies ahead of them, for they must be able to complete a video game in mere 48 hours!\nAfter a while of planning, the group gets to work on their game. Hours pass, and the team is making good progress.\nSoon however, a wild bug appears in the code!\nThe error is not critical, but it could hamper their progress later.\nWhat should the team do?\n\n- solve\n- ignore\n",
                new string[] { "The engineers decide that their time is too important to waste on something so small, and ignore the problem.\n", "The clever engineers realise how this small issue might bite their asses later, and they decide to spend a few hours working on the problem.\n" },
                AdventureBuilder.CreateAnswers()
                .AddAnswer("ignore", 1)
                .AddAnswer("solve", 2)),

            new TextNode(
                "Several hours later everyone on the team is happily coding away, when suddenly all the computers crash simultaneously!\nThe poor engineers are shocked. They wonder if all their hard earned progress is now lost.\nThe situation looks dire, what should the team do?\n\n- give up\n- try to save data\n",
                new string[] { "Their morale has been crushed, and the team decides to give up on the game jam. However, some of the members disagree on the matter.\n", "Even though it seems hopeless, Siquel proves himself as the hero, and restores all of their data from the hard drives.\n" },
                AdventureBuilder.CreateAnswers()
                .AddAnswer("give up", 3)
                .AddAnswer("try to save data", 4)),

            new TextNode(
                "The bug is crushed and development of the game is going smoothly. The team is sure that their game will absolutely be game of the year material.\nIt's late at night, and the classroom is getting quiet. One of the engineers, Siquel, decides to go get a cup of water.\nOn the way back however, he trips on a bag and the water splashes all over a nearby computer!\nThe computer short circuits, and soon smoke is filling the classroom!\nA fire alarm is activated, and everyone is racing to evacuate the building.\nWill the team risk losing the data, or risk themselves trying to save it from destruction?\n\n- try to save data\n- get out safely\n",
                new string[] {"The data is too important to lose, and Pidgin heroically puts out the flames with a fire extinguisher.\n", "The engineers decide that since their life is more important than a simple jam game, it's better to just escape from the potential danger while they can.\n "},
                AdventureBuilder.CreateAnswers()
                .AddAnswer("try to save data", 5)
                .AddAnswer("get out safely", 3)),

            new TextNode(
                "A while later the team is sitting in the lounge, trying to figure out how to continue their project.\nAfter the data was lost, some arguments started to rise between the members. Especially Pidgin and Siquel.\nThis argument soon grew into loud bickering and name-calling, and the others had to step in before things got out of hand. It's obvious that the team needs a new direction, but who will be the one to win the argument?\n\n- Siquel\n- Pidgin\n",
                new string[] {"Siquel wins the argument, and the team gets back to work on their game.\n", "Pidgin wins the argument, and the team gets back to work on their game.\n"},
                AdventureBuilder.CreateAnswers()
                .AddAnswer("Siquel" ,4)
                .AddAnswer("Pidgin", 5)),

            new TextNode(
                "With Siquel's ideas the team finally gets back on track with their game.\nEverything is working great, and the team is happy to get along with each other again.\nSoon, the final lines of code are written, and the team is happy with their achievement.\n\n- finish game",
                new string[] {""},
                AdventureBuilder.CreateAnswers()
                .AddAnswer("finish game", 10)),

            new TextNode(
                "With Pidgin's clever ideas, the team is soon back on track with their jam game.\nThe game looks incredible, with next gen graphics and several hundreds of hours worth of content.\nEverything is working great, but the team is still angry with each other.\nEven after all of their accomplishments, there is still some tension left in the air.\n\n- finish game",
                new string[] {""},
                AdventureBuilder.CreateAnswers()
                .AddAnswer("finish game", 10)),
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

