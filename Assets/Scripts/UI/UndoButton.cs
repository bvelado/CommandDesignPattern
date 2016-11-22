using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UndoButton : MonoBehaviour {

    Button m_button;

	void Awake() {
        m_button = GetComponent<Button>();
	}

    void Start()
    {
        CommandStream.Instance.CanUndo += SetInteractable;
    }

    void OnDestroy()
    {
        if (CommandStream.Instance)
            CommandStream.Instance.CanUndo -= SetInteractable;
    }

    void SetInteractable(bool interactable)
    {
        m_button.interactable = interactable;
    }
}
