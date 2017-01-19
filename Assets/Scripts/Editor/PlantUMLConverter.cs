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
    /// 構造体情報リスト
    /// </summary>
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

        // 構造体パース
        ParseStructural (lines);

        // 継承矢印パターン
        string arrow = arrowPattern.Replace ("{dir}", dirPattern);
        Regex regex = new Regex (arrow);

        for (int i = 0; i < lines.Length; ++i) {
            if (!regex.IsMatch (lines [i])) {
                continue;
            }

            var structurals = regex.Split (lines [i]);


        }
    }

    /// <summary>
    /// 構造体パース
    /// </summary>
    private void ParseStructural( string[] lines )
    {
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
                continue;
            }
        }
    }
}
