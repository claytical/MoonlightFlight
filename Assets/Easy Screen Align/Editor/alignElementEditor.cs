using UnityEngine;
using System.Collections;
using UnityEditor;
using eageramoeba.DetectResize;

namespace eageramoeba.DetectResize {
	
	[CustomEditor(typeof(alignElement))]
	public class alignElementEditor : Editor {
		private string[] selStrings = new string[] { '\u2196'.ToString(), '\u2191'.ToString(), '\u2197'.ToString(), '\u2190'.ToString(), "X", '\u2192'.ToString(), '\u2199'.ToString(), '\u2193'.ToString(), '\u2198'.ToString() };
		private string[] selStringLook = new string[] {"None", "Camera", "Screen"};
		private string[] selStringBound = new string[] {"Renderer", "Mesh", "Sprite", "Collider", "Override"};

		SerializedObject m_object;
		SerializedProperty alignProp;
		SerializedProperty alignOProp;
		SerializedProperty sideProp;
		SerializedProperty verProp;
		SerializedProperty invProp;
		SerializedProperty invLProp;

		SerializedProperty lockXProp;
		SerializedProperty lockYProp;
		SerializedProperty lockZProp;
		SerializedProperty lockXFloatProp;
		SerializedProperty lockYFloatProp;
		SerializedProperty lockZFloatProp;
		SerializedProperty lockXRProp;
		SerializedProperty lockYRProp;
		SerializedProperty lockZRProp;
		SerializedProperty lockXRFloatProp;
		SerializedProperty lockYRFloatProp;
		SerializedProperty lockZRFloatProp;

		SerializedProperty lerpTransformProp;
		SerializedProperty lerpLookAtProp;
		SerializedProperty lerpStepTransProp;
		SerializedProperty lerpLookAtStepProp;
		SerializedProperty lerpToleranceProp;
		SerializedProperty smoothToleranceProp;
		SerializedProperty lerpToleranceStep;
		SerializedProperty smoothToleranceStep;
		SerializedProperty lerpTimeProp;

		SerializedProperty onResizeProp;
		SerializedProperty onMoveProp;
		
		SerializedProperty GUICameraProp;
		
		SerializedProperty lookAtProp;
		SerializedProperty forwardVector;
		SerializedProperty offsetRProp;

		SerializedProperty percentProp;

		SerializedProperty boundsBProp;
		SerializedProperty boundsOProp;
		SerializedProperty boundsObjProp;

		SerializedProperty camDirProp;

		SerializedProperty lookModeProp;
		SerializedProperty boundsModeProp;

		void OnEnable() {
			m_object = new SerializedObject(target);
			alignProp = m_object.FindProperty("alignE");
			alignOProp = m_object.FindProperty("alignO");
			sideProp = m_object.FindProperty("sideF");
			verProp = m_object.FindProperty("verF");
			invProp = m_object.FindProperty("invert");
			invLProp = m_object.FindProperty("invertL");
			lockXProp = m_object.FindProperty("lockX");
			lockYProp = m_object.FindProperty("lockY");
			lockZProp = m_object.FindProperty("lockZ");
			lockXFloatProp = m_object.FindProperty("lockXFloat");
			lockYFloatProp = m_object.FindProperty("lockYFloat");
			lockZFloatProp = m_object.FindProperty("lockZFloat");
			lockXRProp = m_object.FindProperty("lockXR");
			lockYRProp = m_object.FindProperty("lockYR");
			lockZRProp = m_object.FindProperty("lockZR");
			lockXRFloatProp = m_object.FindProperty("lockXFloatR");
			lockYRFloatProp = m_object.FindProperty("lockYFloatR");
			lockZRFloatProp = m_object.FindProperty("lockZFloatR");
			onResizeProp = m_object.FindProperty("onResize");
			GUICameraProp = m_object.FindProperty("GUICamera");
			forwardVector = m_object.FindProperty("forwardRot");
			offsetRProp = m_object.FindProperty("offsetR");
			onMoveProp = m_object.FindProperty("onMove");
			lerpTransformProp = m_object.FindProperty("lerpTransform");
			lerpLookAtProp = m_object.FindProperty("lerpLookat");
			lerpStepTransProp = m_object.FindProperty("lerpStepTransform");
			lerpLookAtStepProp = m_object.FindProperty("lerpStepLookat");
			lerpToleranceProp = m_object.FindProperty("lerpTolerance");
			smoothToleranceProp = m_object.FindProperty("smoothTolerance");
			lerpToleranceStep = m_object.FindProperty("lerpToleranceF");
			smoothToleranceStep = m_object.FindProperty("smoothToleranceF");
			lerpTimeProp = m_object.FindProperty("lerpTime");
			percentProp = m_object.FindProperty("usePercentage");
			boundsBProp = m_object.FindProperty("overrideBounds");
			boundsOProp = m_object.FindProperty("boundsSet");
			boundsObjProp = m_object.FindProperty("boundsObj");
			lookModeProp = m_object.FindProperty("lookAtMode");
			boundsModeProp = m_object.FindProperty("boundsMode");
		}
	
		public override void OnInspectorGUI() {
			DrawDefaultInspector();
			m_object.Update();
	
			alignElement script = (alignElement)target;

			GUICameraProp.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("GUI Camera", "The camera used to render this element, if none are assigned the Main Camera will be used."), GUICameraProp.objectReferenceValue, typeof(Camera));
			onResizeProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Trigger On Resize", "Only align the element when the DetectResize script returns true"), onResizeProp.boolValue);
			onMoveProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Trigger On Camera Move/Rotation", "Only align the element when the camera is moved or rotated"), onMoveProp.boolValue);
			percentProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Use percentage", "Use percentage of screen for all spacial measurements, calculated by using shortest axis of screen resolution."), percentProp.boolValue);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Object Anchor", "Choose the elements anchor position to be aligned to the screens anchor position"), EditorStyles.boldLabel);
			alignOProp.intValue = GUILayout.SelectionGrid(alignOProp.intValue, selStrings, 3, GUILayout.Width(80));
			EditorGUILayout.LabelField(new GUIContent("Screen Anchor", "Choose the elements anchor position on the screen"), EditorStyles.boldLabel);
			alignProp.intValue = GUILayout.SelectionGrid(alignProp.intValue, selStrings, 3, GUILayout.Width(80));
			EditorGUILayout.Space();
			sideProp.floatValue = EditorGUILayout.FloatField(new GUIContent("X Spacing", "Space the element on the X axis away from the anchor position"), sideProp.floatValue);
			verProp.floatValue = EditorGUILayout.FloatField(new GUIContent("Y Spacing", "Space the element on the Y axis away from the anchor position"), verProp.floatValue);
			EditorGUILayout.Space();
			invProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Invert Align", "Invert the align vector on the x/y axis"), invProp.boolValue);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Lookat", "Make the object point it's z-axis (forward) towards the camera/screen at all times"), EditorStyles.boldLabel);
			lookModeProp.intValue = EditorGUILayout.Popup("Mode:", lookModeProp.intValue, selStringLook, EditorStyles.popup);
			forwardVector.vector3Value = EditorGUILayout.Vector3Field("Lookat World Up", forwardVector.vector3Value);
			//offsetRProp.vector3Value = EditorGUILayout.Vector3Field("Lookat Rotation Offset", offsetRProp.vector3Value);
			EditorGUILayout.Space();
			invLProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Invert Lookat", "Invert the direction when using Lookat"), invLProp.boolValue);
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Lock movement to axis", "Lock an axis to a value, z effects distance to camera"), EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal ();
			lockXProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock X", "Lock the X axis"), lockXProp.boolValue);
			lockXFloatProp.floatValue = EditorGUILayout.FloatField("", lockXFloatProp.floatValue);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			lockYProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock Y", "Lock the Y axis"), lockYProp.boolValue);
			lockYFloatProp.floatValue = EditorGUILayout.FloatField("", lockYFloatProp.floatValue);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.BeginHorizontal ();
			lockZProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock Z-Distance", "Lock the Z axis to a number, effects only the distance from the camera"), lockZProp.boolValue);
			lockZFloatProp.floatValue = EditorGUILayout.FloatField("", lockZFloatProp.floatValue);
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Lock lookat to axis", "Lock a rotation value to an axis"), EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			lockXRProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock X", "Lock the X axis"), lockXRProp.boolValue);
			lockXRFloatProp.floatValue = EditorGUILayout.FloatField("", lockXRFloatProp.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			lockYRProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock Y", "Lock the Y axis"), lockYRProp.boolValue);
			lockYRFloatProp.floatValue = EditorGUILayout.FloatField("", lockYRFloatProp.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			lockZRProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lock Z", "Lock the Z axis, does not use percentage of screen ever"), lockZRProp.boolValue);
			lockZRFloatProp.floatValue = EditorGUILayout.FloatField("", lockZRFloatProp.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Lerp/Smoothing", "Smooth movement of element"), EditorStyles.boldLabel);
			lerpTimeProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lerp by time scale?", "Smoothing/lerp values effected by time scale"), lerpTimeProp.boolValue);
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			lerpTransformProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lerp Transform", "Smooth/Lerp the movement properties"), lerpTransformProp.boolValue);
			lerpStepTransProp.floatValue = EditorGUILayout.FloatField("", lerpStepTransProp.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			lerpToleranceProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lerp Transform Tolerance", "Set a sphere that the lerp cannot move away from on the x,y,z plane. % of smallest side of screen"), lerpToleranceProp.boolValue);
			lerpToleranceStep.floatValue = EditorGUILayout.FloatField("", lerpToleranceStep.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			smoothToleranceProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Smooth Tolerance", "Smooth the lerp tolerance (lerp the lerp)"), smoothToleranceProp.boolValue);
			smoothToleranceStep.floatValue = EditorGUILayout.FloatField("", smoothToleranceStep.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();
			lerpLookAtProp.boolValue = EditorGUILayout.Toggle(new GUIContent("Lerp Lookat", "Smooth/Lerp lookat"), lerpLookAtProp.boolValue);
			lerpLookAtStepProp.floatValue = EditorGUILayout.FloatField("", lerpLookAtStepProp.floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(new GUIContent("Renderer bounds", "Set or override the renderer bounds used with Object Anchor"), EditorStyles.boldLabel);
			boundsModeProp.intValue = EditorGUILayout.Popup("Mode:", boundsModeProp.intValue, selStringBound, EditorStyles.popup);
			boundsOProp.vector2Value = EditorGUILayout.Vector2Field("Bounds Override", boundsOProp.vector2Value);
			boundsObjProp.objectReferenceValue = EditorGUILayout.ObjectField(new GUIContent("Bounds Object", "The object to use for bounds data, useful when wrapping a mesh within an empty."), boundsObjProp.objectReferenceValue, typeof(GameObject));

			m_object.ApplyModifiedProperties();
		}
	}
		
}