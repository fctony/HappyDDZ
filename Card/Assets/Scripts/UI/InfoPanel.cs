using Protocol.Dto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : UIBase
{
    private void Awake()
    {
        Bind(UIEvent.REFRESH_INFO_PANEL);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.REFRESH_INFO_PANEL:
                UserDto user = message as UserDto;
                refreshPanel(user.Name, user.Lv, user.Exp, user.Been);
                break;
            default:
                break;
        }
    }

    //private Image imgHead;
    private Text txtName;
    private Text txtLv;
    private Slider sldExp;
    private Text txtExp;
    private Text txtBeen;

    // Use this for initialization
    void Start()
    {
        txtName = transform.Find("txtName").GetComponent<Text>();
        txtLv = transform.Find("txtLv").GetComponent<Text>();
        sldExp = transform.Find("sldExp").GetComponent<Slider>();
        txtExp = transform.Find("txtExp").GetComponent<Text>();
        txtBeen = transform.Find("txtBeen").GetComponent<Text>();
    }

    /// <summary>
    /// 刷新视图
    ///     有参数，包括 名字 等级 经验 豆子
    /// </summary>
    private void refreshPanel(string name, int lv, int exp, int been)
    {
        txtName.text = name;
        txtLv.text = "Lv." + lv;
        //等级和经验之间的公式：maxExp = lv * 100
        txtExp.text = exp + " / " + lv * 100;
        // 99 / 100 = 0.99
        sldExp.value = (float)exp / lv * 100;
        txtBeen.text = "× " + been.ToString();
    }

}
