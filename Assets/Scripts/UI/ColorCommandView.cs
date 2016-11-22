using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ColorCommandView : MonoBehaviour, IFillable {

    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Text m_text;

    public void Fill(ICommand command)
    {
        if(command.GetType() == typeof(AddColorCommand))
        {
            Fill((AddColorCommand)command);
        } else if(command.GetType() == typeof(RemoveColorCommand))
        {
            Fill((RemoveColorCommand)command);
        }
    }

    void Fill(AddColorCommand command)
    {
        m_image.color = command.Color;
        m_text.text = command.Name + " at position " + command.Index;
    }

    void Fill(RemoveColorCommand command)
    {
        m_image.color = command.Color;
        m_text.text = command.Name + " at position " + command.Index;
    }

}
