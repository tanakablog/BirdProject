using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// クラス情報
/// </summary>
public class ClassInfo : StructuralInfoBase {

    /// <summary>
    /// アクセス修飾子テーブル
    /// </summary>
    private static readonly Dictionary<string,string> kACCESS_MODIFIERS_TABLE = new Dictionary<string, string>() {
        {"+", "public "},  
        {"-", "private " },
        {"#", "protected " }
    };
    
    /// <summary>
    /// 継承名リスト
    /// </summary>
    protected List<string> inheritanceNameList;

    /// <summary>
    /// 抽象クラスフラグ
    /// </summary>
    private bool isAbstract = false;

    public override int Parse (string[] lines, int index)
    {
        // 名前パース
        SetName (lines [index]);

        // 内容までインデックスをずらす
        index++;
        if (lines [index].IndexOf ("{") >= 0) {
            index++;
        }

        // 定義終了まで内容をパース
        menberList = new List<MenberInfo> ();
        while (lines [index].IndexOf ("}") < 0) {
            var menber = new MenberInfo ();

            menber.name = ReplaceAccessModifiers (lines [index]);
            menber.isAbstract = lines [index].IndexOf ("abstract ") >= 0;

            menberList.Add (menber);

            index++;
        }

        return index++;
    }

    public override void SetName (string structural)
    {
        // 抽象クラスチェック
        isAbstract = structural.IndexOf ("abstract") >= 0;

        // 構造名保存
        strcturalName = structural.Replace ("class", string.Empty).Replace ("{", string.Empty).Replace ("abstract", string.Empty).Trim ();
    }

    public override string GetDeclarationName ()
    {
        var declaration = string.Empty;

        // 抽象クラスチェック
        if (isAbstract) {
            declaration = string.Format ("public abstract class {0}", strcturalName);
        } else {
            declaration = string.Format ("public class {0}", strcturalName);
        }

        // 継承チェック
        if (inheritanceNameList != null && inheritanceNameList.Count > 0) {
            StringBuilder builder = new StringBuilder ();

            foreach (var inheritance in inheritanceNameList) {
                if (builder.Length > 0) {
                    builder.Append (", ");
                }

                builder.Append (inheritance); 
            }

            declaration = string.Format ("{0} : {1}", declaration, builder.ToString ());
        } 

        return declaration;
    }

    /// <summary>
    /// アクセス修飾子置き換え
    /// </summary>
    /// <returns>置き換え後文字列</returns>
    /// <param name="line">文字列</param>
    private string ReplaceAccessModifiers( string line )
    {
        foreach (var replace in kACCESS_MODIFIERS_TABLE) {
            if (line.IndexOf (replace.Key) >= 0) {
                return line.Replace (replace.Key, replace.Value);
            }
        }

        return line;
    }
}