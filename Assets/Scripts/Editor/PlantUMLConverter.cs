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
    /// アクセス修飾子テーブル
    /// </summary>
    private static readonly Dictionary<string,string> accessModifiersTable = new Dictionary<string, string>() {
        {"+", "public "},  
        {"-", "private " },
        {"#", "protected " }
    };

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

    [MenuItem("Test/Convert")]
    public static void Convert () {
        PlantUMLConverter converter = new PlantUMLConverter ();

        converter.ConvertProcess ("AssetBundleManager--|>AssetLoader");
    }

    /// <summary>
    /// 変換処理
    /// </summary>
    /// <param name="text">Text.</param>
    public void ConvertProcess(string text)
    {
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
            if (lines [i].IndexOf("interface") >= 0 ) {
                i = ParseInterface (lines, i);
                continue;
            }
        }

        string arrow = arrowPattern.Replace ("{dir}", dirPattern);
        Regex regex = new Regex (arrow);
    }

    /// <summary>
    /// クラスパース処理
    /// </summary>
    /// <returns>クラス終了インデックス</returns>
    /// <param name="lines">文字列リスト</param>
    /// <param name="index">アクセスインデックス</param>
    private int ParseClass( string[] lines, int index)
    {
        // 情報クラス初期化
        var info = new ClassInfo ();
        var menber_list = info.GetMenberList ();
        menber_list = new List<MenberInfo> ();
        info.name = lines [index].Replace ("{", string.Empty);

        // 内容までインデックスをずらす
        index++;
        if (lines [index].IndexOf ("{") >= 0) {
            index++;
        }

        // 定義終了までパース処理
        while (lines [index].IndexOf ("}") < 0) {
            
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
    /// インターフェースパース処理
    /// </summary>
    /// <returns>The interface.</returns>
    /// <param name="lines">Lines.</param>
    /// <param name="index">Index.</param>
    private int ParseInterface( string[] lines, int index )
    {
        // 情報クラス初期化
        var info = new InterfaceInfo ();
        info.menberList = new List<MenberInfo> ();
        info.name = lines [index].Replace ("{", string.Empty);

        // 内容までインデックスをずらす
        index++;
        if (lines [index].IndexOf ("{") >= 0) {
            index++;
        }

        // 定義終了までパース処理
        while (lines [index].IndexOf ("}") < 0) {
            var menber = new MenberInfo ();

            menber.name = ReplaceAccessModifiers (lines [index]);
            menber.isAbstract = true;

            info.menberList.Add (menber);

            index++;
        }

        // インターフェース定義情報をリストに追加
        interfaceInfoList.Add (info);

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
}
