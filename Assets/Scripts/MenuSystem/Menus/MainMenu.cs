using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainMenu : SwipeableMenu
{
    public override void AfterOpen()
    {
    }

    private IEnumerator ShowLoading()
    {
        yield return new WaitForSeconds(5f);
        MenuManager.Instance.Show(MenuManager.MenuName.Loading);
    }

    public override void OnBack()
    {
        Application.Quit();
    }
}
