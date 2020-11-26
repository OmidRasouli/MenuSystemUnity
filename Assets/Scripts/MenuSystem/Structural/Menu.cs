using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationMenu))]
public class Menu : MonoBehaviour
{
    [SerializeField] private MenuManager.MenuName type;
    internal MenuManager.MenuName MenuType => type;
    [SerializeField] private AnimationMenu animationMenu;
    [Tooltip("Destroy after close")] public bool destroyable;
    [Tooltip("will disable the menu that was active before this menu, It will show but did not work")] public bool disableBelow;
    internal float scaleFactor;
    internal float YieldTime => animationMenu.time;

    public virtual void Init(float scaleFactor)
    {
        this.scaleFactor = scaleFactor;
        if (animationMenu.animationType == AnimationMenu.AnimationType.Move)
        {
            animationMenu.Init(scaleFactor);
        }
    }
    public virtual void BeforeOpen() { }
    public virtual void AfterOpen() { }
    public virtual void BeforeClose() { }
    public virtual void AfterClose() { }

    public void BringIt()
    {
        animationMenu.Play();
    }

    public virtual void OnBack()
    {
        MenuManager.Instance.Close();
    }
}