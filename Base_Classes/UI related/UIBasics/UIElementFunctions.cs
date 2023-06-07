using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElementFunctions : MonoBehaviour
{
    private bool hideWithUI = true;
    public bool HideWithUI { get { return hideWithUI; } }

    private bool canBeMoved = true;
    public bool CanBeMoved { get { return canBeMoved; } }

    private Toggle hideToggle;
    private Toggle moveToggle;
    private Button closeButton;
    private TextMeshProUGUI nameField;
    private Transform contentContainer;
    private UIContent content;

    public UIContent Content { get { return content; } }
    
    public Sprite hideToggleTrue;
    public Sprite hideToggleFalse;
    public Sprite moveToggleTrue;
    public Sprite moveToggleFalse;

    public Material transparentMaterial;

    private int x_border = 15;
    private int y_border = 15;


    public void Awake()
    {
        closeButton = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Button>();
        hideToggle = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Toggle>();
        moveToggle = transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Toggle>();
        nameField = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        contentContainer = transform.GetChild(1);

        hideToggle.onValueChanged.AddListener(OnHideToggleValueChanged);
        moveToggle.onValueChanged.AddListener(OnMoveToggleValueChanged);

        closeButton.onClick.AddListener(CloseUIElement);

        // Set initial toggle states and icons
        hideToggle.isOn = true;
        moveToggle.isOn = true;
        UpdateHideToggleIcon();
        UpdateMoveToggleIcon();
    }

    public void SetName(string _name)
    {
        if(nameField!= null)
        {
            nameField.text = _name.Substring(0, 1).ToUpper() + _name.Substring(1).ToLower();
        }
        else
        {
            Debug.Log("nameField reference is null");
        }
    }

    private void setSize(Vector2 size)
    {
        RectTransform rtf = this.GetComponent<RectTransform>();
        if (rtf != null)
        {
            // Update the size of the content container
            RectTransform contentRtf = contentContainer.GetComponent<RectTransform>();
            contentRtf.sizeDelta = size;

            // Update the size of the UI element
            Vector2 newSize = new Vector2(size.x + (2 * x_border), size.y + contentContainer.GetComponent<RectTransform>().rect.height + transform.GetChild(0).GetComponent<RectTransform>().rect.height + (y_border * 2));
            rtf.sizeDelta = newSize;
        }
    }

    public void SetContent(UIContent ct)
    {
        if(contentContainer!= null)
        {
            if(contentContainer.childCount == 0)
            {
                content = Instantiate(ct);
                content.transform.SetParent(contentContainer.transform);
                content.Initialize();
                setSize(content.GetSize());
            }
        }
    }

    public void SetMode(int modeId)
    {
        switch (modeId)
        {
            case 0:
                //default behaviour of a windowed element;
                break;
            case 1:
                //disable menu of the UI element
                transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 2:
                //disable menu of the UI element, and set it to always active, with transparent background.
                hideToggle.isOn = false;
                transform.GetChild(0).gameObject.SetActive(false);

                Color tmpcolor = GetComponent<Image>().color;
                tmpcolor.a = 0f;

                GetComponent<Image>().color = tmpcolor;
                GetComponent<Image>().material = transparentMaterial;
                contentContainer.GetComponent<Image>().color = tmpcolor;
                contentContainer.GetComponent<Image>().material = transparentMaterial;
                contentContainer.GetChild(0).GetComponent<Image>().color = tmpcolor;
                contentContainer.GetChild(0).GetComponent<Image>().material = transparentMaterial;
                this.gameObject.SetActive(false);
                break;
            default:
                //default behaviour of a windowed element, same as "case 0";
                break;
        }
        return;
    }

    private void OnHideToggleValueChanged(bool newValue)
    {
        ToggleHideWithUI(newValue);
        UpdateHideToggleIcon();
    }

    private void OnMoveToggleValueChanged(bool newValue)
    {
        ToggleCanBeMoved(newValue);
        UpdateMoveToggleIcon();
    }
    private void UpdateHideToggleIcon()
    {
        if (hideToggle.isOn)
        {
            hideToggle.image.sprite = hideToggleTrue;
        }
        else
        {
            hideToggle.image.sprite = hideToggleFalse;
        }
    }

    private void UpdateMoveToggleIcon()
    {
        if (moveToggle.isOn)
        {
            moveToggle.image.sprite = moveToggleTrue;
        }
        else
        {
            moveToggle.image.sprite = moveToggleFalse;
        }
    }

    public void CloseUIElement()
    {
        gameObject.SetActive(false);
    }

    public void ToggleHideWithUI(bool value)
    {
        hideWithUI = value;
    }

    public void ToggleCanBeMoved(bool value)
    {
        canBeMoved = value;
    }

    public void UIClosed()
    {
        if(hideToggle.isOn) { 
            CloseUIElement();
        }
    }
}
