using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public sealed class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public float scaleFactor;

    private Stack<Menu> menus;
    public List<MenuName> instanceMenus;
    public enum MenuName
    {
        MainMenu, Loading, PopUp
    }

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private LoadingMenu loading;
    [SerializeField] private PopUpMenu popUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
        }
    }

    private void Start()
    {
        menus = new Stack<Menu>();
        instanceMenus = new List<MenuName>();
        scaleFactor = GetComponent<Canvas>().scaleFactor;
        Show(MenuName.Loading);
    }


    public void Show(MenuName menu)
    {
        switch (menu)
        {
            case MenuName.MainMenu:
                Show(mainMenu);
                break;
            case MenuName.Loading:
                Show(loading);
                break;
            case MenuName.PopUp:
                Show(popUp);
                break;
            default:
                break;
        }
    }

    private void Show(Menu menu)
    {
        menu = ShowMenu(menu);
        StartCoroutine(OpenMenu(menu));
    }

    private IEnumerator OpenMenu(Menu menu)
    {
        menu.BeforeOpen();
        menu.BringIt();
        yield return new WaitForSeconds(menu.YieldTime);
        menu.AfterOpen();
    }

    private IEnumerator CloseMenu(Menu menu)
    {
        menu.BeforeClose();
        menu.BringIt();
        yield return new WaitForSeconds(menu.YieldTime);
        menu.AfterClose();
        yield return new WaitForSeconds(0.1f);
        if (menu.destroyable)
        {
            instanceMenus.Remove(menu.MenuType);
            Destroy(menu.gameObject);
        }
    }

    private Menu ShowMenu(Menu menu)
    {
        if (menus.Count > 0 && !menu.disableBelow)
        {
            Close(menus.Pop());
        }
        if (!instanceMenus.Any(x => x == menu.MenuType))
        {
            menu = Instantiate(menu.gameObject, transform).GetComponent<Menu>();
            menu.Init(scaleFactor);
            instanceMenus.Add(menu.MenuType);
            menus.Push(menu);
        }
        else
        {
            if (menus.Contains(menu))
            {
                MoveTopStack(menu);
            }
        }
        return menu;
    }

    private void MoveTopStack(Menu menu)
    {
        var tmp = new Stack<Menu>();
        while (menus.Peek() != menu)
        {
            tmp.Push(menus.Pop());
        }
        menus.Pop();
        while (tmp.Count > 0)
        {
            menus.Push(tmp.Pop());
        }
        menus.Push(menu);
    }

    private void Close(Menu menu)
    {
        StartCoroutine(CloseMenu(menu));
    }

    public void Close()
    {
        StartCoroutine(CloseMenu(menus.Pop()));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && menus.Count > 0)
            menus.Peek().OnBack();
    }
}
