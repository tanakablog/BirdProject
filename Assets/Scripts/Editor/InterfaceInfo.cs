using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インターフェース情報
/// </summary>
public class InterfaceInfo : StructuralInfoBase {

    public override string GetKey ()
    {
        return "interface";
    }

    public override void SetName (string structural)
    {
        strcturalName = structural.Replace ("interface", string.Empty).Replace ("{", string.Empty).Trim ();
    }

    public override string GetDeclarationName ()
    {
        return string.Format ("public interface {0}", strcturalName);
    }
}