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
        CommandStream.Instance.CanUndo += (canUndo) => m_button.interactable = canUndo;
    }
}
