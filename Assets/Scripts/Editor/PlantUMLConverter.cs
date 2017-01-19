using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Text;

public class PlantUMLConverter {

    private const string createFolderPath = "Assets";

    /// <summary>
    /// 継承矢印パターン
    /// </summary>
    private const string arrowPattern = @"(?:-{1,}{dir}\|>|<\|{dir}-{1,})";

    /// <summary>
    /// 方向パターン
    /// </summary>
    private const string dirPattern = @"(?:|right|left|up|down|r|l|u|d)";

    /// <summary>
    /// クラス情報リスト
    /// </summary>
    private List<ClassInfo> classInfoList;

    /// <summary>
    /// インターフェース情報リスト
    /// </summary>
    private List<InterfaceInfo> interfaceInfoList;

    private List<StructuralInfoBase> structuralInfoList = new List<StructuralInfoBase>();

    [MenuItem("Test/Convert")]
    public static void Convert () {
        PlantUMLConverter converter = new PlantUMLConverter ();

        converter.ConvertProcess (AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Class.txt").text);
    }

    /// <summary>
    /// 変換処理
    /// </summary>
    /// <param name="text">Text.</param>
    public void ConvertProcess(string text)
    {
        // １行毎に分割
        var lines = text.Replace ("\r\n", "\n").Split ('\n');

        for (int i = 0; i < lines.Length; ++i) {
            // クラスパース処理
            if (lines [i].IndexOf("class") >= 0 ) {
                var info = new ClassInfo ();
                i = info.Parse (lines, i);
                structuralInfoList.Add (info);
                continue;
            }

            // インターフェースパース処理
            if (lines [i].IndexOf("interface") >= 0 ) {
                var info = new InterfaceInfo ();
                i = info.Parse (lines, i);
                structuralInfoList.Add (info);

                Debug.LogWarning (info.GetDeclarationName ());
                continue;
            }
        }

        string arrow = arrowPattern.Replace ("{dir}", dirPattern);
        Regex regex = new Regex (arrow);
    }
}
