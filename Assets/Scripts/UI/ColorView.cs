using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorView : MonoBehaviour {

    private Image m_image;

    public Color Color { get { return m_image.color; } }

    void Awake()
    {
        m_image = GetComponent<Image>();
    }

	public void Fill(Color color)
    {
        m_image.color = color;
    }
}
