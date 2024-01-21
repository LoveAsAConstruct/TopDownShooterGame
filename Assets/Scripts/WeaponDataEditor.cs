using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponData))]
public class WeaponDataEditor : Editor
{
    SerializedProperty muzzlePositionProp;

    private void OnEnable()
    {
        muzzlePositionProp = serializedObject.FindProperty("muzzlePosition");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WeaponData weaponData = (WeaponData)target;

        // Show common properties
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponSprite"));

        // Show properties based on weapon type
        if (weaponData.weaponType == WeaponType.Ranged)
        {
            // Show ranged properties
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fireRate"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("range"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("recoilStrength"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("accuracySpread"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsPerShot"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("projectilePrefab"));
        }
        else if (weaponData.weaponType == WeaponType.Melee)
        {
            // Show melee properties
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRange"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("durability"));
            // Add any additional melee-specific properties here
        }

        if (weaponData.weaponSprite != null)
        {
            GUILayout.Label("Weapon Preview:");
            float ppu = weaponData.weaponSprite.pixelsPerUnit;
            float aspect = (float)weaponData.weaponSprite.texture.width / weaponData.weaponSprite.texture.height;
            float height = 10000; // Adjust this height as needed
            float width = height * aspect;
            Rect rect = GUILayoutUtility.GetRect(width / ppu, height / ppu);

            // Handle mouse events
            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;
            if (rect.Contains(mousePos) && e.type == EventType.MouseDown && e.button == 0)
            {
                // Calculate position relative to the center of the image
                Vector2 centeredMousePos = new Vector2((mousePos.x - rect.x) - rect.width / 2, (mousePos.y - rect.y) - rect.height / 2);
                // Normalize the coordinates
                weaponData.muzzlePosition = new Vector2(centeredMousePos.x / (rect.width / 2), centeredMousePos.y / (rect.height / 2));
                e.Use();
                Repaint();
            }

            // Draw the texture
            GUI.DrawTexture(rect, weaponData.weaponSprite.texture, ScaleMode.ScaleToFit, true);

            // Draw a circle at the muzzle position
            if (weaponData.muzzlePosition != null)
            {
                Handles.color = Color.red;
                Vector2 scaledMuzzlePosition = new Vector2(
                    (weaponData.muzzlePosition.x * (rect.width / 2)) + rect.x + (rect.width / 2),
                    (weaponData.muzzlePosition.y * (rect.height / 2)) + rect.y + (rect.height / 2)
                );
                Handles.DrawSolidDisc(new Vector3(scaledMuzzlePosition.x, scaledMuzzlePosition.y, 0), Vector3.forward, 2);
            }

            serializedObject.ApplyModifiedProperties();
        }
        EditorGUILayout.PropertyField(muzzlePositionProp);
    }
}
