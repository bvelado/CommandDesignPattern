using UnityEngine;
using System.Collections;
using System;

public class AddColorCommand : ICommand
{
    public string Name { get { return "Add color"; } }

    Color _color;
    public Color Color { get { return _color; } }
    int _index;
    public int Index { get { return _index; } }
    Transform _container;
    GameObject _gameObject;

    public AddColorCommand(Color color, Transform container, int index)
    {
        _color = color;
        _index = index;
        _container = container;
    }

    public void execute()
    {
        _gameObject = GameObject.Instantiate(Resources.Load("Prefabs/ColorView"), _container, false) as GameObject;
        _gameObject.transform.SetSiblingIndex(_index);
        _gameObject.GetComponent<ColorView>().Fill(_color);
    }

    public void undo()
    {
        GameObject.Destroy(_gameObject);
        _gameObject = null;
    }
}
