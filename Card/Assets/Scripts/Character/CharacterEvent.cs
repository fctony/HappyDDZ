﻿using System;
using System.Collections.Generic;



public class CharacterEvent
{
    public const int INIT_MY_CARD = 0;//初始化自身卡牌
    public const int INIT_LEFT_CARD = 1;//初始化左边玩家的卡牌
    public const int INIT_RIGHT_CARD = 2;//初始化右边玩家的卡牌

    public const int ADD_MY_CARD = 3;//自身玩家添加底牌
    public const int ADD_LEFT_CARD = 4;//左边玩家添加底牌
    public const int ADD_RIGHT_CARD = 5;//右边玩家添加底牌

    public const int DEAL_CARD = 6;//出牌

    public const int REMOVE_MY_CARD = 7;//移除自身手牌
    public const int REMOVE_LEFT_CARD = 8;//移除左边手牌
    public const int REMOVE_RIGHT_CARD = 9;//移除右边手牌

    public const int UPDATE_SHOW_DESK = 10;//更新桌面的显示
}
