using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Drawer class that allow the user to draw with a stylus on the app.
/// </summary>
public class Drawer : MonoBehaviour
{
    public GameObject deleteButton;
    public Image cover;
    public Image background;
    public Sprite newCover;
    public Sprite coverBackground;
    public Sprite mainBackground;
    public Canvas canv;
    public bool debug = true;
    public string screenShotName = "Note1.png";
    public string coverName = "Cover1.png";

    private bool firstDraw = true;
    private RectTransform rectConfig;
    private UILineRenderer rend = null;
    private GameObject currentRenderer;
    private List<Vector2> tempList = new List<Vector2>();
    private Stack<Transform> childs = new Stack<Transform>();

    // Update is called once per frame
    void Update()
    {
        Vector2 curPos = new Vector2();
        int touchIndex = 0;

        if (debug)
        {
            DebugMouse();
        }
        else
        {
            if (Input.touchCount == 1)
            {
                touchIndex = GetStylusIndex();
                if (touchIndex != -1 && CheckPosition(touchIndex))
                {
                    if (Input.GetTouch(touchIndex).phase == TouchPhase.Moved && Input.GetTouch(0).type == TouchType.Stylus)
                    {
                        if (tempList.Count == 0)
                            CreateNewDrawingLine(Input.GetTouch(touchIndex).position, out curPos);
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canv.transform as RectTransform, Input.GetTouch(touchIndex).position, canv.worldCamera, out curPos);
                        if (!tempList.Contains(curPos) && Vector2.Distance(curPos, tempList[tempList.Count - 1]) > 10f)
                        {
                            tempList.Add(curPos);
                            rend.Points = tempList.ToArray();
                        }
                    }
                    if (Input.GetTouch(touchIndex).phase == TouchPhase.Ended && Input.GetTouch(0).type == TouchType.Stylus)
                        tempList.Clear();
                }
            }
        }
    }
    /// <summary>
    /// Debug without stylus
    /// </summary>
    /// <returns></returns>
    bool CheckMouse()
    {
        if (Input.mousePosition.y >= Screen.height * 0.91f)
        {
            tempList.Clear();
            return (false);
        }
        return (true);
    }

    void DebugMouse()
    {
        Vector2 curPos = new Vector2();

        if (CheckMouse())
        {
            if (Input.GetMouseButton(0))
            {
                if (tempList.Count == 0)
                    CreateNewDrawingLine(Input.mousePosition, out curPos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out curPos);
                if (!tempList.Contains(curPos) && Vector2.Distance(curPos, tempList[tempList.Count - 1]) > 10f)
                {
                    tempList.Add(curPos);
                    rend.Points = tempList.ToArray();
                }

            }
            if (Input.GetMouseButtonUp(0))
            {
                CommandStream.Instance.Push(new AddLineCommand(10f, Color.black, transform, tempList.ToArray(), GetComponent<RectTransform>().sizeDelta));
                tempList.Clear();
                Destroy(rend.gameObject);
            }
                
        }
    }

    void CreateNewDrawingLine(Vector2 pos, out Vector2 curPos)
    {
        currentRenderer = new GameObject();
        tempList.Clear();
        rectConfig = currentRenderer.AddComponent<RectTransform>();
        rectConfig.sizeDelta = GetComponent<RectTransform>().sizeDelta;
        rectConfig.pivot = Vector2.zero;
        rend = currentRenderer.AddComponent<UILineRenderer>();
        rend.LineThickness = 10f;
        rend.color = Color.black;
        currentRenderer.transform.SetParent(this.transform, false);
        childs.Push(currentRenderer.transform);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), pos, Camera.main, out curPos);
        tempList.Add(curPos);
        rend.Points = tempList.ToArray();
    }

    int GetStylusIndex()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).type == TouchType.Stylus)
                return (i);
        }
        return (-1);
    }

    bool CheckPosition(int touchIndex)
    {
        if (Input.GetTouch(touchIndex).position.y >= Screen.height * 0.91f)
        {
            tempList.Clear();
            return (false);
        }
        return (true);
    }

    public void ResetDrawer()
    {
        firstDraw = true;
        Erase();
    }

    public void SwitchBackground()
    {
        if (firstDraw)
        {
            background.sprite = mainBackground;
            background.color = new Color32(255, 255, 255, 15);
        }
        else
        {
            Debug.Log(screenShotName);
        }
    }

    public void Toogle()
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);

    }

    public void Erase()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void ErasePrevious()
    {
        if (childs.Count > 0)
            Destroy(childs.Pop().gameObject);
    }
}