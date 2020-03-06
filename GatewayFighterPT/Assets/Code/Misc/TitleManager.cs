using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public Image PressAnyButton;
    public Image StageSelect;
    public Button FirstSelected;
    public EventSystem es;
    bool SelectStage = false;

    // Start is called before the first frame update
    void Start()
    {
        StageSelect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SelectStage == false)
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKeyDown("joystick 1 button " + i))
                {
                    SelectStage = true;
                    PressAnyButton.gameObject.SetActive(false);
                    StageSelect.gameObject.SetActive(true);
                    es.SetSelectedGameObject(FirstSelected.gameObject);
                }
            }
        }
    }
}
