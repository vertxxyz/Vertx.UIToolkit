using System;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Vertx.UIToolkit.Editor
{
	public static class InspectorElementQuery
	{
		/// <summary>
		/// Gets all property fields under a root <see cref="VisualElement"/>.
		/// </summary>
		/// <param name="root">The root of the query.</param>
		/// <param name="serializedObject">The serialized object associated with all the property fields.</param>
		/// <param name="allFieldsBound">A callback for when all property fields are bound to.</param>
		/// <returns>A lookup from bindingPath to <see cref="PropertyField"/>.</returns>
		public static Dictionary<string, PropertyField> PropertyFields(
			VisualElement root,
			SerializedObject serializedObject,
			Action<Dictionary<string, PropertyField>> allFieldsBound = null)
		{
			Dictionary<string, PropertyField> results = new();
			UQueryBuilder<PropertyField> builder = root.Query<PropertyField>();
			ListPool<PropertyField>.Get(out List<PropertyField> unboundFields);
			try
			{
				foreach (PropertyField field in builder.Build())
				{
					results.Add(field.bindingPath, field);
					SerializedProperty property = serializedObject.FindProperty(field.bindingPath);
					if (property.hasVisibleChildren && field.childCount == 0)
						unboundFields.Add(field);
				}

				if (unboundFields.Count == 0)
					allFieldsBound?.Invoke(results);
				else
				{
					if (allFieldsBound != null)
					{
						EditorCoroutineUtility.StartCoroutine(Callback(
								serializedObject,
								results,
								unboundFields,
								allFieldsBound
							),
							root
						);
						return results;
					}
				}
			}
			catch (Exception)
			{
				ListPool<PropertyField>.Release(unboundFields);
				throw;
			}

			ListPool<PropertyField>.Release(unboundFields);
			return results;
		}

		private static IEnumerator Callback(
			SerializedObject serializedObject,
			Dictionary<string, PropertyField> results,
			List<PropertyField> unboundFields,
			Action<Dictionary<string, PropertyField>> allFieldsBound)
		{
			do
			{
				yield return null;
				for (int i = unboundFields.Count - 1; i >= 0; i--)
				{
					PropertyField root = unboundFields[i];
					if (root.childCount == 0) continue;
					try
					{
						unboundFields.RemoveAt(i);
						UQueryBuilder<PropertyField> builder = root.Query<PropertyField>();
						foreach (PropertyField field in builder.Build())
						{
							if (!results.TryAdd(field.bindingPath, field))
								continue;
							SerializedProperty property = serializedObject.FindProperty(field.bindingPath);
							if (property.hasVisibleChildren && field.childCount == 0)
								unboundFields.Add(field);
						}
					}
					catch (ArgumentNullException)
					{
						yield break;
					}
				}
			} while (unboundFields.Count > 0);

			ListPool<PropertyField>.Release(unboundFields);
			allFieldsBound(results);
		}
	}
}