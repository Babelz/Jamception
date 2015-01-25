using UnityEngine;
using System.Collections;

public class StatusHUD : MonoBehaviour
{
    #region Vars
    private Camera playerView;

    private GUIText siquText;
    private GUIText enkoText;
    private GUIText babText;

    private StatusBehaviour siqu;
    private StatusBehaviour enko;
    private StatusBehaviour bab;
    #endregion

    // Use this for initialization
	private void Start()
    {
        siqu = GameObject.Find("siquSitting").GetComponent<StatusBehaviour>();
        enko = GameObject.Find("enkoSitting").GetComponent<StatusBehaviour>();
        bab = GameObject.Find("babSitting").GetComponent<StatusBehaviour>();

        enkoText = GameObject.Find("enkoStatus").guiText;
        siquText = GameObject.Find("siquStatus").guiText;
        babText = GameObject.Find("babStatus").guiText;

        playerView = GameObject.Find("PlayerCamera").camera;
    }

    private void SetText(GUIText guiText, string[] lines)
    {
        guiText.text = string.Empty;

        for (int i = 0; i < lines.Length; i++)
        {
            guiText.text += lines[i] + "\n";
        }
    }

	// Update is called once per frame
	private void Update()
    {
        SetText(babText, bab.ToStringArray());
        SetText(siquText, siqu.ToStringArray());
        SetText(enkoText, enko.ToStringArray());
	}
}
