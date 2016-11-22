using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class DrawCommandView : MonoBehaviour, IFillable {

    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Text m_text;

    public void Fill(ICommand command)
    {
        if(command.GetType() == typeof(AddLineCommand))
        {
            Fill((AddLineCommand)command);
        }
    }

    void Fill(AddLineCommand command)
    {
        m_image.color = command.Color;
        m_text.text = command.Name;
    }

}
