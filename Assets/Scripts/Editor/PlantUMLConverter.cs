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
    private const string arrowLeftPattern = @"(?:<\|{dir}-{1,})";

    /// <summary>
    /// 継承矢印パターン
    /// </summary>
    private const string arrowRightPattern = @"(?:-{1,}{dir}\|>)";

    private const string arrowPattern = @"(?:<\|{dir}-{1,}|-{1,}{dir}\|>|<-{dir}-{1,}|-{1,}{dir}->)";

    /// <summary>
    /// 方向パターン
    /// </summary>
    private const string dirPattern = @"(?:|right|left|up|down|r|l|u|d)";

    /// <summary>
    /// 構造体情報リスト
    /// </summary>
    private List<StructuralInfoBase> structuralInfoList = new List<StructuralInfoBase>();

    /// <summary>
    /// パーサー配列
    /// </summary>
    private ParserBase[] parsers = new ParserBase[] {
        new ClassParser(),
        new InterfaceParser()
    };

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

        // パース処理
        for (int i = 0; i < lines.Length; ++i) {
            foreach (var parser in parsers) {
                var infos = parser.Parse (lines, ref i);
                if (infos == null) {
                    continue;
                }

                structuralInfoList.AddRange (infos);
            }
        }

        foreach (var structural in structuralInfoList.OrderBy(x=>x.GetName()) ) {
            Debug.LogWarning (structural.GetName ());
        }

        /*
        // 継承矢印パターン
        string arrow = arrowLeftPattern.Replace ("{dir}", dirPattern);
        Regex regex = new Regex (arrow);

        for (int i = 0; i < lines.Length; ++i) {
            if (!regex.IsMatch (lines [i])) {
                continue;
            }

            var structurals = regex.Split (lines [i]);

            var base_structural = structuralInfoList.FirstOrDefault (x => x.GetName () == structurals[0]);

            var target_structural = structuralInfoList.FirstOrDefault (x => x.GetName () == structurals [1]);

            if (target_structural == null) {
                target_structural = new ClassInfo ();
                target_structural.SetName (structurals [1]);
                structuralInfoList.Add (target_structural);
            }

            if (base_structural == null) {
                base_structural = new ClassInfo ();
                base_structural.SetName (structurals [0]);
                structuralInfoList.Add (base_structural);
                continue;
            }

            var members = base_structural.GetInheritanceMemberNames ();
            if (members.Count == 0) {
                continue;
            }

            // 矢印も先に全てパース

        }
        */
    }

    /// <summary>
    /// 矢印パース
    /// </summary>
    private void ParseArrow(string[] lines )
    {
        // 矢印パターン読み込み
        Regex regex = new Regex (arrowPattern.Replace("{dir}",dirPattern));

        foreach (var line in lines) {
            // 矢印チェック
            if (!regex.IsMatch (line)) {
                continue;
            }

            // クラス名取り出し
            var struct_names = regex.Split (line);
            foreach (var struct_name in struct_names) {
                var replace_name = struct_name.Trim ();

                // すでに登録されているか
                if (structuralInfoList.Any (x => x.GetName() == replace_name)) {
                    continue;
                }
                var info = new ClassInfo ();

                Debug.Log (replace_name + ":" + info.GetName ());

                // クラス名登録
                structuralInfoList.Add (info);
            }
        }


    }
}
