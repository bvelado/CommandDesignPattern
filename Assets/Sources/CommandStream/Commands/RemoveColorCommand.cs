using System;
using UnityEngine;

public class RemoveColorCommand : ICommand
{
    public string Name { get { return "Remove color"; } }

    Color _color;
    public Color Color { get { return _color; } }
    int _index;
    public int Index { get { return _index; } }
    Transform _container;
    GameObject _gameObject;

    public RemoveColorCommand(GameObject gameObject)
    {
        _gameObject = gameObject;
        _color = gameObject.GetComponent<ColorView>().Color;
        _index = gameObject.transform.GetSiblingIndex();
        _container = gameObject.transform.parent;
    }

    public void execute()
    {
        GameObject.Destroy(_gameObject);
        _gameObject = null;
    }

    public void undo()
    {
        _gameObject = GameObject.Instantiate(Resources.Load("Prefabs/ColorView"), _container, false) as GameObject;
        _gameObject.GetComponent<ColorView>().Fill(_color);
        _gameObject.transform.SetSiblingIndex(_index);
    }
}
