using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

public class PlantUMLConverter {

    private const string createFolderPath = "Assets";

    /// <summary>
    /// アクセス修飾子テーブル
    /// </summary>
    private static readonly Dictionary<string,string> accessModifiersTable = new Dictionary<string, string>() {
        {"+", "public "},  
        {"-", "private " },
        {"#", "protected " }
    };

    /// <summary>
    /// 矢印パターン
    /// </summary>
    private const string arrowPattern = @"-*->";

    /// <summary>
    /// クラス情報リスト
    /// </summary>
    private List<ClassInfo> classInfoList;

    [MenuItem("Test/Convert")]
    public static void Convert () {
        PlantUMLConverter converter = new PlantUMLConverter ();

        converter.ConvertProcess ("AssetBundleManager-->AssetLoader");
    }

    public void ConvertProcess(string text)
    {
        Regex regex = new Regex (arrowPattern);

        foreach (var split in regex.Split (text)) {
            Debug.LogWarning (split);
        }

        return;

        classInfoList = new List<ClassInfo> ();
        
        // １行毎に分割
        var lines = text.Replace ("\r\n", "\n").Split ('\n');

        for (int i = 0; i < lines.Length; ++i) {
            // クラスパース処理
            if (lines [i].IndexOf("class") >= 0 ) {
                i = ParseClass (lines, i);
                continue;
            }

            // インターフェースパース処理
        }
    }

    /// <summary>
    /// クラスパース処理
    /// </summary>
    /// <returns>クラス終了インデックス</returns>
    /// <param name="lines">文字列リスト</param>
    /// <param name="index">アクセスインデックス</param>
    private int ParseClass( string[] lines, int index)
    {
        var info = new ClassInfo ();
        info.menberList = new List<MenberInfo> ();
        info.name = "public " + lines [index].Replace ("{", string.Empty);

        index++;

        // クラス定義終了までパース処理
        while (lines [index].IndexOf ("}") < 0) {
            if (lines [index].IndexOf ("{") >= 0) {
                index++;
                continue;
            }

            var menber = new MenberInfo ();

            menber.name = ReplaceAccessModifiers (lines [index]);
            menber.isAbstract = lines [index].IndexOf ("abstract ") >= 0;

            info.menberList.Add (menber);

            index++;
        }

        // クラス定義情報をリストに追加
        classInfoList.Add (info);

        return index++;
    }

    /// <summary>
    /// アクセス修飾子置き換え
    /// </summary>
    /// <returns>置き換え後文字列</returns>
    /// <param name="line">文字列</param>
    private string ReplaceAccessModifiers( string line )
    {
        foreach (var replace in PlantUMLConverter.accessModifiersTable) {
            if (line.IndexOf (replace.Key) >= 0) {
                return line.Replace (replace.Key, replace.Value);
            }
        }

        return line;
    }

    public class ClassInfo {
        public string name;
        public List<MenberInfo> menberList;
    }

    public class InterfaceInfo {
        public string name;
        public List<MenberInfo> menberList;
    }

    public class MenberInfo {
        public string name;
        public bool isAbstract;
    }
}
