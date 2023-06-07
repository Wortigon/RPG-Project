using System.Collections.Generic;
using System;
using UnityEngine;

public class MenuController : Singleton<MenuController>
{
    private CameraController cameraController;
    private PlayerCharacter playerCharacter;

    public Transform UI;
    
    public KeyCode menuKeyCode = KeyCode.Escape;
    public UIElementFunctions uiElementPrefab;
    [Serializable]
    public struct UIElementKeyBinding
    {
        public string key;
        public KeyCode keyCode;
        public int containerTypeId;
        public UIContent uiContentPrefab;
        public Vector3 defaultPosition;
        public Vector3 defaultScale;
    }

    public Material transparentMaterial;

    public List<UIElementKeyBinding> uiElementBindings;
    public Dictionary<string, UIElementFunctions> uiElements;

    public Dictionary<KeyCode, string> keyBindings;
    
    private bool isMenuOpen = false;

    protected override void Awake()
    {
        base.Awake();

        cameraController = FindObjectOfType<CameraController>();
        playerCharacter = FindObjectOfType<PlayerCharacter>();

        uiElements = new Dictionary<string, UIElementFunctions>();
        keyBindings = new Dictionary<KeyCode, string>();
        foreach (var binding in uiElementBindings)
        {

            UIElementFunctions uiElement = Instantiate(uiElementPrefab);
            uiElement.transform.SetParent(UI.transform);
            uiElement.transform.localPosition = binding.defaultPosition;
            uiElement.transform.localScale = binding.defaultScale;
            uiElement.SetContent(binding.uiContentPrefab);
            uiElement.SetName(binding.key);
            uiElement.gameObject.SetActive(true);
            uiElement.SetMode(binding.containerTypeId);
            uiElement.gameObject.SetActive(!uiElement.gameObject.activeSelf);
            uiElements.Add(binding.key, uiElement);
            keyBindings.Add(binding.keyCode, binding.key);

        }

        DontDestroyOnLoad(transform.GetChild(0).transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(menuKeyCode))
        {
            ToggleMenu();
        }

        foreach (var binding in keyBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                ToggleUIElement(binding.Value);
            }
        }
    }

    private void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;

        if (isMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cameraController.ToggleFollowingMouse();
            playerCharacter.ToggleCanMove();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameraController.ToggleFollowingMouse();
            playerCharacter.ToggleCanMove();
            UIClosed();
        }
    }

    private void UIClosed()
    {
        foreach (UIElementFunctions uiElement in uiElements.Values)
        {
            uiElement.UIClosed();
        }
    }

    private void ToggleUIElement(string key)
    {
        if (uiElements.TryGetValue(key, out var uiElement))
        {
            if (uiElement.gameObject.activeSelf)
            {
                uiElement.UIClosed();
            }
            else
            {
                uiElement.gameObject.SetActive(true);
            }
        }
    }
}
