using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPanel : UIBase
{
    private Button btnSet;
    private Image imgBg;
    private Button btnClose;
    private Text txtAudio;
    private Toggle togAudio;
    private Text txtVolume;
    private Slider sldVolume;
    private Button btnQuit;

    void Start()
    {
        btnSet = transform.Find("btnSet").GetComponent<Button>();
        imgBg = transform.Find("imgBg").GetComponent<Image>();
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        txtAudio = transform.Find("txtAudio").GetComponent<Text>();
        togAudio = transform.Find("togAudio").GetComponent<Toggle>();
        txtVolume = transform.Find("txtVolume").GetComponent<Text>();
        sldVolume = transform.Find("sldVolume").GetComponent<Slider>();
        btnQuit = transform.Find("btnQuit").GetComponent<Button>();

        btnSet.onClick.AddListener(setClick);
        btnClose.onClick.AddListener(closeClick);
        btnQuit.onClick.AddListener(quitClick);
        togAudio.onValueChanged.AddListener(audioValueChanged);
        sldVolume.onValueChanged.AddListener(volumeValueChanged);

        //默认状态
        setObjectsActive(false);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        btnSet.onClick.RemoveListener(setClick);
        btnClose.onClick.RemoveListener(closeClick);
        btnQuit.onClick.RemoveListener(quitClick);
        togAudio.onValueChanged.RemoveListener(audioValueChanged);
        sldVolume.onValueChanged.RemoveListener(volumeValueChanged);
    }

    private void setObjectsActive(bool active)
    {
        imgBg.gameObject.SetActive(active);
        btnClose.gameObject.SetActive(active);
        togAudio.gameObject.SetActive(active);
        sldVolume.gameObject.SetActive(active);
        btnQuit.gameObject.SetActive(active);
        txtAudio.gameObject.SetActive(active);
        txtVolume.gameObject.SetActive(active);
    }


    private void setClick()
    {
        setObjectsActive(true);
    }

    private void closeClick()
    {
        setObjectsActive(false);
    }

    private void quitClick()
    {
        Application.Quit();
    }

    /// <summary>
    /// 开关点击的时候调用
    /// </summary>
    /// <param name="result">勾上了是true 勾掉了是false</param>
    private void audioValueChanged(bool value)
    {
        //操作声音
        //TODO
    }

    /// <summary>
    /// 当滑动条滑动的时候会调用
    /// </summary>
    /// <param name="value">滑动条的值</param>
    private void volumeValueChanged(float value)
    {
        //操作声音
        //TODO
    }


}
