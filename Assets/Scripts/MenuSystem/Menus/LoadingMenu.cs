using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMenu : Menu
{
    private bool startLoading;
    [SerializeField] private Image loading;
    [SerializeField] private Text percentage;
    private float countdown = 3f;
    private float timer;

    public override void BeforeOpen()
    {
        base.BeforeOpen();
        percentage.text = "0%";
    }
    public override void AfterOpen()
    {
        base.AfterOpen();

        startLoading = true;
    }

    private void Update()
    {
        if (startLoading)
        {
            loading.fillAmount = Timing.Lerp(timer / countdown, Timing.EasingType.EaseInSin, 0f, 1f);
            percentage.text = Timing.LerpInt(timer / countdown, Timing.EasingType.EaseInSin, 0, 100).ToString() + "%";
            timer += Time.deltaTime;
            if (timer >= countdown)
            {
                startLoading = false;
                MenuManager.Instance.Show(MenuManager.MenuName.MainMenu);
            }
        }
    }
}
