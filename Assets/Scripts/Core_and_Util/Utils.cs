using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 複数クラスで使えそうな便利な関数をまとめたクラス
/// </summary>
public static class Utils
{
    /// <summary>
    /// 複数タグのどれかに一致するか調べる関数
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static bool CompareTags(string targetTag, string[] tags)
    {
        foreach (string t in tags)
        {
            if ((targetTag) == t)
                return true;
        }

        return false;
    }

}
