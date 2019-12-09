using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScroll : MonoBehaviour
{
    [SerializeField]
    private float m_lerpTime;
    private ScrollRect m_scrollRect;
    private Button[] m_buttons;
    private int m_index { get; set; }
    private float m_verticalPosition;
    private bool m_up;
    private bool m_down;
    private bool getStickDown = false;

    public void Start()
    {
        Time.timeScale = 0f;
        m_scrollRect = GetComponent<ScrollRect>();
        m_buttons = GetComponentsInChildren<Button>();
        m_buttons[m_index].Select();
        m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));

        for(int i = 0; i < m_buttons.Length; i++)
        {
            m_buttons[i].GetComponent<TestSelectListener>().index = i;
        }
    }

    public void Update()
    {
        
        /*m_up = Input.GetKeyDown(KeyCode.UpArrow);
        m_down = Input.GetKeyDown(KeyCode.DownArrow);

        if (m_up ^ m_down)
        {
            if (m_up)
                m_index = Mathf.Clamp(m_index - 1, 0, m_buttons.Length - 1);
            else
                m_index = Mathf.Clamp(m_index + 1, 0, m_buttons.Length - 1);

            m_buttons[m_index].Select();
            m_verticalPosition = 1f - ((float)m_index / (m_buttons.Length - 1));
        }

        m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);*/
    }

    public void SetVerticalPosition(int i)
    {
        m_verticalPosition = 1f - (float)i / (m_buttons.Length - 1);
        m_scrollRect.verticalNormalizedPosition = m_verticalPosition;
    }
}

