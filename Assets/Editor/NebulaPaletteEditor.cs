using Code.Scenario.Starfield;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Inspector for <see cref="NebulaPalette"/>: shows the tones as swatches,
/// warns when a tone is bright enough to compete with gameplay sprites,
/// and imports a pasted list of hex colors.
/// </summary>
[CustomEditor(typeof(NebulaPalette))]
public class NebulaPaletteEditor : Editor
{
    private string _hexInput = "";
    private string _importError;

    public override void OnInspectorGUI()
    {
        var palette = (NebulaPalette)target;

        DrawDefaultInspector();

        EditorGUILayout.Space();
        DrawSwatches(palette);
        DrawWarnings(palette);

        EditorGUILayout.Space();
        DrawHexImport(palette);
    }

    private static void DrawSwatches(NebulaPalette palette)
    {
        Rect row = EditorGUILayout.GetControlRect(false, 24f);

        // Empty space first, then each tone, as the shader stacks them.
        int cells = palette.ToneCount + 1;
        float width = row.width / Mathf.Max(1, cells);

        EditorGUI.DrawRect(new Rect(row.x, row.y, width - 2f, row.height), Color.black);

        for (var i = 0; i < palette.ToneCount; i++)
        {
            var cell = new Rect(row.x + width * (i + 1), row.y, width - 2f, row.height);
            EditorGUI.DrawRect(cell, palette.Tones[i]);
        }
    }

    private static void DrawWarnings(NebulaPalette palette)
    {
        if (palette.Tones.Count > NebulaPalette.MaxTones)
        {
            EditorGUILayout.HelpBox(
                $"Only the first {NebulaPalette.MaxTones} tones are used.",
                MessageType.Warning
            );
        }

        float luminance = palette.MaxLuminance;

        if (luminance > palette.LuminanceWarning)
        {
            EditorGUILayout.HelpBox(
                $"The brightest tone has luminance {luminance:0.00}, above " +
                $"{palette.LuminanceWarning:0.00}. It may compete with ships " +
                "and projectiles; the layer's intensity scales it further.",
                MessageType.Warning
            );
        }
    }

    private void DrawHexImport(NebulaPalette palette)
    {
        EditorGUILayout.LabelField("Import hex colors", EditorStyles.boldLabel);

        _hexInput = EditorGUILayout.TextArea(_hexInput, GUILayout.MinHeight(40f));

        if (GUILayout.Button("Replace tones"))
        {
            Undo.RecordObject(palette, "Import nebula palette");

            if (palette.TryImportHex(_hexInput, out _importError))
            {
                EditorUtility.SetDirty(palette);
                _hexInput = "";
            }
        }

        if (!string.IsNullOrEmpty(_importError))
            EditorGUILayout.HelpBox(_importError, MessageType.Error);
    }
}
