
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    [SerializeField] private UI_ScenePrefabContainerSO uiScenePrefabContainer;
    [SerializeField] private UI_PopupPrefabContainerSO uiPopupPrefabContainer;
    private Canvas rootCanvas;
    
    private Stack<BasePopupUI> popupStack = new Stack<BasePopupUI>();
    private int sortingOrder = 10;
    public Canvas Root
    {
        get
        {
            if (rootCanvas != null)
            {
                return rootCanvas;
            }
            GameObject root = GameObject.Find("@MainCanvas");
            if (root == null)
            {
                root = new GameObject("@MainCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                rootCanvas = root.GetComponent<Canvas>();
            }
            if(root.TryGetComponent(out Canvas canvas))
            {
                rootCanvas = canvas;
                return rootCanvas;
            }
            rootCanvas = root.AddComponent<Canvas>();
            rootCanvas.AddComponent<CanvasScaler>()
                .AddComponent<GraphicRaycaster>();
            return rootCanvas;
        }
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (uiPopupPrefabContainer == null)
        {
            uiPopupPrefabContainer = Resources.FindObjectsOfTypeAll<UI_PopupPrefabContainerSO>()[0];
        }

        if(uiScenePrefabContainer == null)
        {
            uiScenePrefabContainer = Resources.FindObjectsOfTypeAll<UI_ScenePrefabContainerSO>()[0];
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Single)
        {
            sortingOrder = 10;
            popupStack.Clear();
            rootCanvas = Root;
        }
    }

    public T CreateSceneUI<T>(UI_Scenes uiScenes) where T : BaseSceneUI
    {
        GameObject prefab = uiScenePrefabContainer.Get(uiScenes);
        GameObject go = GameObject.Instantiate(prefab, Root.transform);
        T ui = go.GetOrAddComponent<T>();
        return ui;
    }
    
    public T CreateUI<T>(UI_Poups uiPoups) where T : BasePopupUI
    {
        GameObject prefab = uiPopupPrefabContainer.Get(uiPoups);
        GameObject go = GameObject.Instantiate(prefab, Root.transform);
        T ui = go.GetOrAddComponent<T>();
        popupStack.Push(ui);
        return ui;
    }
    
    public void SetCanvas(GameObject uiGo, bool isPopup = false)
    {
        Canvas canvas = uiGo.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        if (isPopup)
        {
            canvas.sortingOrder = sortingOrder;
            sortingOrder++;
        }
        else
        {
            canvas.sortingOrder = -1;
        }
    }
    
    public void ClosePopupUI(BasePopupUI popupUI)
    {
        if (popupStack.Count == 0)
        {
            return;
        }
        if (popupStack.Peek() != popupUI)
        {
            return;
        }
        popupStack.Pop();
        Destroy(popupUI.gameObject);
        sortingOrder--;
    }
}
