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
        CommandStream.Instance.CanRedo += SetInteractable;
    }

    void OnDestroy()
    {
        if(CommandStream.Instance)
            CommandStream.Instance.CanRedo -= SetInteractable;
    }

    void SetInteractable(bool interactable)
    {
        m_button.interactable = interactable;
    }
}
