using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    public MenuManager.MenuName openMenu;

    public void ShowMenu()
    {
        MenuManager.Instance.Show(openMenu);
    }
}
