using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void OnClickStart()
    {
        GameManager.Instance.StartButton();
    }
    public void OnClickJeyGuide()
    {
        GameManager.Instance.KeyGuideButton();
    }
    public void MinecraftRuleButton()
    {
        GameManager.Instance.RuleButton(GameManager.Instance.MinecraftRule);
    }
    public void CrazyArcadeRuleButton()
    {
        GameManager.Instance.RuleButton(GameManager.Instance.CrazyArcadeRule);

    }
    public void MapleStoryRuleButton()
    {
        GameManager.Instance.RuleButton(GameManager.Instance.MapleStoryRule);
    }
}
