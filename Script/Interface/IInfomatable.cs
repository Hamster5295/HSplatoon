using System.Collections.Generic;

//用于获取信息（主要是装备或玩家的属性，方便做图鉴）
public interface IInfomatable
{
    Dictionary<string,string> GetInfo();
}