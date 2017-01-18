using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected List<MenberInfo> menberList;

    /// <summary>
    /// 継承名リスト
    /// </summary>
    protected List<string> inheritanceNameList;

    /// <summary>
    /// キー取得
    /// </summary>
    /// <returns>キー名</returns>
    public abstract string GetKey();

    /// <summary>
    /// 構造体名設定
    /// </summary>
    public abstract void SetName (string structural);

    /// <summary>
    /// 宣言する構造体名取得
    /// </summary>
    public abstract string GetDeclarationName ();
}

/// <summary>
/// メンバー情報
/// </summary>
public class MenberInfo {
    public string name;
    public bool isAbstract;
}