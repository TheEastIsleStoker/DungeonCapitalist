using UnityEngine;
using UnityEditor;
using TMPro;

public class FontReplacer : EditorWindow
{
    private TMP_FontAsset newFont;

    [MenuItem("Tools/换字体")]
    static void Open() => GetWindow<FontReplacer>("换字体", false).Show();

    void OnGUI()
    {
        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("新字体", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("全换") && newFont != null)
        {
            // 场景
            foreach (var t in Resources.FindObjectsOfTypeAll<TextMeshProUGUI>())
            {
                if (PrefabUtility.IsPartOfPrefabAsset(t)) continue;
                t.font = newFont;
                EditorUtility.SetDirty(t);
            }

            // 预制体
            foreach (var guid in AssetDatabase.FindAssets("t:Prefab"))
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid));
                if (!prefab) continue;
                foreach (var t in prefab.GetComponentsInChildren<TextMeshProUGUI>(true))
                {
                    t.font = newFont;
                    EditorUtility.SetDirty(t);
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log("换完");
        }
    }
}