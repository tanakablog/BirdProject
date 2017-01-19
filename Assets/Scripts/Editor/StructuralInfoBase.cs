using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 構造情報ベース
/// </summary>
public abstract class StructuralInfoBase {
    /// <summary>
    /// 構造名
    /// </summary>
    protected string strcturalName;

    /// <summary>
    /// メンバーリスト
    /// </summary>
    public List<MenberInfo> menberList;

    /// <summary>
    /// パース
    /// </summary>
    /// <param name="lines">文字列配列.</param>
    /// <param name="index">パース開始インデックス</param>
    public abstract int Parse (string[] lines, int index);

    /// <summary>
    /// 構造体名設定
    /// </summary>
    public abstract void SetName (string structural);

    /// <summary>
    /// 宣言する構造体名取得
    /// </summary>
    public abstract string GetDeclarationName ();

    /// <summary>
    /// 継承メンバー名取得
    /// </summary>
    public virtual string[] GetInheritanceMemberNames() {
        if (menberList == null) {
            return new string[0];
        }

        return menberList.Where (member => member.isAbstract).Select (menber => menber.name).ToArray ();
    }
}

/// <summary>
/// メンバー情報
/// </summary>
public class MenberInfo {
    public string name;
    public bool isAbstract;
}