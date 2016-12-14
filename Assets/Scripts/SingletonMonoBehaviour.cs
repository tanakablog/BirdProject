using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// MonoBehaviour用シングルトンクラス
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

        /// <summary>
        /// インスタンス
        /// </summary>
        private static T _instance;
        public static T Instance {
            get { 
                // インスタンスチェック
                if (_instance == null) {
                    // インスタンス検索
                    _instance = FindObjectOfType<T> ();
                    if (_instance == null) {
                        // インスタンス生成
                        var type = typeof(T);
                        var obj = new GameObject (type.ToString () + "(Singleton)");
                        _instance = obj.AddComponent<T> ();
                    }
                }

                return _instance;
            }
        }
    }
}
