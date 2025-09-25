using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField playerNameFieldRef;
    [SerializeField] private TMP_InputField roomNameFieldRef;
    // Start is called before the first frame update
    void Start()
    {
        SetReferences();
    }

    private void SetReferences()
    {
        PUNManager.Instance.playerNameInputField = playerNameFieldRef;
        PUNManager.Instance.roomNameInputField = roomNameFieldRef;
    }

}
