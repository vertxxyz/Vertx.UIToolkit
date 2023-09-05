using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Vertx.UIToolkit.Editor
{
	internal static class MenuItems
	{
		[MenuItem("CONTEXT/StyleSheet/Apply To An Open Window")]
		private static void ApplyStyleSheetToOpenWindow(MenuCommand command)
		{
			var window = ScriptableObject.CreateInstance<ApplyStyleSheetToWindow>();
			window.titleContent = new GUIContent("Apply StyleSheet To");
			window.StyleSheet = (StyleSheet)command.context;
			window.ShowUtility();
		}
	}

	internal sealed class ApplyStyleSheetToWindow : EditorWindow
	{
		[field: SerializeField]
		public StyleSheet StyleSheet { get; set; }

		private void CreateGUI()
		{
			const string windowIconName = "WindowIcon";
			Type contextMenuType = Type.GetType("UnityEditor.UIElements.EditorMenuExtensions+ContextMenu,UnityEditor");

			List<EditorWindow> windows = Resources.FindObjectsOfTypeAll<EditorWindow>().Where(w => w != this && w.GetType() != contextMenuType).ToList();
			var columns = new Columns
			{
				new()
				{
					title = "Name",
					makeCell = () =>
					{
						VisualElement parent = new()
						{
							style = { flexDirection = FlexDirection.Row }
						};
						parent.Add(new VisualElement
						{
							style =
							{
								minWidth = 16,
								minHeight = 16,
								marginLeft = 6,
							},
							name = windowIconName
						});
						parent.Add(new Label("Name")
						{
							style =
							{
								marginLeft = 2,
								marginRight = 6
							}
						});
						return parent;
					},
					bindCell = (element, i) =>
					{
						GUIContent content = windows[i].titleContent;
						element.Q(windowIconName).style.backgroundImage = content.image as Texture2D;
						element.Q<Label>().text = content.text;
					},
					minWidth = 150
				},
				new()
				{
					title = "Style Sheet",
					stretchable = true,
					makeCell = () => new Button(),
					bindCell = (element, i) =>
					{
						VisualElement root = windows[i].rootVisualElement;
						var button = (Button)element;
						button.text = root.styleSheets.Contains(StyleSheet) ? "Remove" : "Add";
						button.clickable = new Clickable(() =>
						{
							if (root.styleSheets.Contains(StyleSheet))
								root.styleSheets.Remove(StyleSheet);
							else
								root.styleSheets.Add(StyleSheet);
						});
					}
				},
			};
			MultiColumnListView view = new MultiColumnListView(columns)
			{
				itemsSource = windows
			};
			rootVisualElement.Add(view);
			view.StretchToParentSize();
		}
	}
}