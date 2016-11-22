using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RedoButton : MonoBehaviour {
    Button m_button;

    void Awake()
    {
        m_button = GetComponent<Button>();
    }

    void Start()
    {
        CommandStream.Instance.CanRedo += (canRedo) => m_button.interactable = canRedo;
    }
}
