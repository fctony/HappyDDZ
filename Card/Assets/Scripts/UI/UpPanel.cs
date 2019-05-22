using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol.Dto.Fight;

public class UpPanel : UIBase
{
    void Awake()
    {
        Bind(UIEvent.SET_TABLE_CARDS);
    }

    public override void Execute(int eventCode, object message)
    {
        switch (eventCode)
        {
            case UIEvent.SET_TABLE_CARDS:
                setTableCards(message as List<CardDto>);
                break;
            default:
                break;
        }
    }

    //底牌图片
    private Image[] imgCards = null;

    void Start()
    {
        imgCards = new Image[3];
        imgCards[0] = transform.Find("imgCard 1").GetComponent<Image>();
        imgCards[1] = transform.Find("imgCard 2").GetComponent<Image>();
        imgCards[2] = transform.Find("imgCard 3").GetComponent<Image>();
    }

    /// <summary>
    /// 设置底牌
    ///  卡牌的数据类 还没有定义 用object代替
    /// </summary>
    private void setTableCards(List<CardDto> cards)
    {
        imgCards[0].sprite = Resources.Load<Sprite>("Poker/" + cards[0].Name);
        imgCards[1].sprite = Resources.Load<Sprite>("Poker/" + cards[1].Name);
        imgCards[2].sprite = Resources.Load<Sprite>("Poker/" + cards[2].Name);
    }
}
