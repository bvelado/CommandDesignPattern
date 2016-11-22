using System;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AddLineCommand : ICompensableCommand
{
    public string Name { get { return "Add line"; } }

    private GameObject _gameObject;
    public GameObject GameObject;

    private float _lineThickness;
    public float LineThickness { get { return _lineThickness; } }

    private Color _color;
    public Color Color { get { return _color; } }

    private Transform _container;
    public Transform Container { get { return _container; } }

    private Vector2[] _linePoints;
    public Vector2[] LinePoints { get { return _linePoints; } }

    private Vector2 _sizeDelta;
    public Vector2 SizeDelta { get { return _sizeDelta; } }

    public AddLineCommand(float lineThickness, Color color, Transform container, Vector2[] linePoints, Vector2 sizeDelta)
    {
        _lineThickness = lineThickness;
        _color = color;
        _container = container;
        _linePoints = linePoints;
        _sizeDelta = sizeDelta;
    }

    public void execute()
    {
        _gameObject = new GameObject();
        RectTransform rectConfig = _gameObject.AddComponent<RectTransform>();
        rectConfig.sizeDelta = _sizeDelta;
        rectConfig.pivot = Vector2.zero;

        UILineRenderer renderer;
        renderer = _gameObject.AddComponent<UILineRenderer>();
        renderer.LineThickness = _lineThickness;
        renderer.color = _color;

        _gameObject.transform.SetParent(_container, false);
        
        renderer.Points = _linePoints;
    }

    public void compensate()
    {
        MonoBehaviour.Destroy(_gameObject);
    }
}
