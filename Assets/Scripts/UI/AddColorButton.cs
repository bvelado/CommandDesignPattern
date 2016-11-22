using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddColorButton : MonoBehaviour {

    private Button m_button;
    [SerializeField]
    private Image m_buttonImage;
    public Color Color;
    public Transform Target;

    void Awake()
    {
        m_button = GetComponent<Button>();
    }

    void Start()
    {
        m_buttonImage.color = Color;
    }

    public void HandleButtonPressed()
    {
        CommandStream.Instance.Push(new AddColorCommand(Color, Target, Target.childCount));
    }
}
