using UnityEditor;
using UnityEngine;
namespace DefaultNamespace
{
    [CustomPropertyDrawer(typeof(Range))]
    public class RangeDrawer : PropertyDrawer
    {
            SerializedProperty min, max, value;
            string name;
        
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                name = property.displayName;
                min = property.FindPropertyRelative("Min");
                max = property.FindPropertyRelative("Max");
                value = property.FindPropertyRelative("hiddenValue");
        
                Rect contentPosition = EditorGUI.PrefixLabel(position, new GUIContent(name));
                float half = contentPosition.width / 2;
                GUI.skin.label.padding = new RectOffset(3, 3, 6, 6);
        
                //show the Min and Max from the Range
                EditorGUIUtility.labelWidth = 24f;
                contentPosition.width *= 0.5f;
                contentPosition.height = 16f;
                EditorGUI.indentLevel = 0;
        
                // Draw min value
                EditorGUI.BeginProperty(contentPosition, label, min);
                {
                    min.floatValue  = EditorGUI.FloatField(contentPosition, new GUIContent("min"), min.floatValue);
                }
                EditorGUI.EndProperty();
        
                contentPosition.x += half;
                
                //Draw Max value
                EditorGUI.BeginProperty(contentPosition, label, max);
                {
                    max.floatValue = EditorGUI.FloatField(contentPosition, new GUIContent("max"), max.floatValue);
                }
                EditorGUI.EndProperty();
                
                contentPosition.y += 16f;
                contentPosition.x -= half;
                contentPosition.width *= 2f;
                
                //Draw current value
                EditorGUI.BeginProperty(contentPosition, label, max);
                {
                    value.floatValue = EditorGUI.Slider(contentPosition, value.floatValue, min.floatValue, max.floatValue);
                }
                EditorGUI.EndProperty();
                
            }
        
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return 32f;
            }
    }
}