using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextAddedInputCommand : ICommand
{
    private Text _text;
    private int _position;
    private char _character;

    public TextAddedInputCommand(Text text, char character, int position)
    {
        _text = text;
        _position = position;
        _character = character;
    }

    public void execute()
    {
        StringBuilder sb = new StringBuilder(_text.text.Length + 1);
        sb.Append(_text.text);
        Debug.Log("1 " +sb.ToString());
        sb.Insert(_position, _character);
        Debug.Log("2" + sb.ToString());
        _text.text = sb.ToString();
    }

    public void undo()
    {
        StringBuilder sb = new StringBuilder(_text.text.Length);
        sb.Append(_text.text);
        Debug.Log("3 " + sb.ToString());
        sb.Remove(_position, 1);
        Debug.Log("4 " + sb.ToString());
        _text.text = sb.ToString();
    }
}
