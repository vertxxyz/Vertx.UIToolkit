using System;
using UnityEngine.UIElements;

namespace Vertx.UIToolkit
{
	[Flags]
	public enum FocusType
	{
		Text = 1,
		Integer = 1 << 1,
		Float = 1 << 2,
		Double = 1 << 3,
		Numerical = Integer | Float | Double,
		Any = ~0
	}
	
	public static class FocusControllerExtensions
	{
		/// <summary>
		/// Checks whether a field that can receive input is focused.
		/// </summary>
		public static bool IsInputFieldFocused(this FocusController focusController, FocusType flags = FocusType.Any)
		{
			if (!(focusController.focusedElement is VisualElement visualElement))
				return false;

			FocusType type = visualElement switch
			{
				DoubleField => FocusType.Double,
				FloatField => FocusType.Float,
				IntegerField => FocusType.Integer,
				TextField => FocusType.Text,
				_ => FocusType.Any
			};

			if ((type & flags) != 0)
				return visualElement.Query<TextElement>(null, "unity-text-element--inner-input-field-component").Focused().First() != null;
			return false;
		}
	}
}