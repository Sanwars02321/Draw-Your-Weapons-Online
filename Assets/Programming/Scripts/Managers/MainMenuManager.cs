using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameFieldRef;
    [SerializeField] private TMP_InputField roomNameFieldRef;
    [SerializeField] private TextMeshProUGUI warningText;

    void Start()
    {
        SetReferences();
    }

    private void SetReferences()
    {
        PUNManager.Instance.playerNameInputField = playerNameFieldRef;
        PUNManager.Instance.roomNameInputField = roomNameFieldRef;
        PUNManager.Instance.WarningText = warningText;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

}
