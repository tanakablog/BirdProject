using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

/// <summary>
/// クラス情報
/// </summary>
public class ClassInfo : StructuralInfoBase {

    public override string GetKey ()
    {
        return "class";
    }

    public override void SetName (string structural)
    {
        strcturalName = structural.Replace ("class", string.Empty).Replace ("{", string.Empty).Replace ("abstract", string.Empty).Trim ();
    }

    public override string GetDeclarationName ()
    {
        var declaration = string.Empty;

        // 継承があるか
        if (inheritanceNameList.Count > 0) {
            StringBuilder builder = new StringBuilder ();

            foreach (var inheritance in inheritanceNameList) {
                if (builder.Length > 0) {
                    builder.Append (", ");
                }

                builder.Append (inheritance); 
            }

            declaration = string.Format ("public class {0} : {1}", strcturalName, builder.ToString ());
        } else {
            declaration = string.Format ("public class {0}", strcturalName);
        }

        return declaration;
    }

    /// <summary>
    /// メンバリスト取得
    /// </summary>
    public List<MenberInfo> GetMenberList()
    {
        return menberList;
    }
}