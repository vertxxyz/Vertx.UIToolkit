using UnityEngine;
using UnityEngine.UIElements;

namespace Vertx.UIToolkit
{
	/// <summary>
	/// Extensions to affect multi-part properties which are irritating to set via the defaults.
	/// </summary>
	public static class StyleExtensions
	{
		public static void SetBackground(this IStyle style, Sprite sprite) => style.backgroundImage = new StyleBackground(sprite);
		
		public static void SetBackground(this IStyle style, Texture2D texture) => style.backgroundImage = new StyleBackground(texture);
		
		public static void SetBackground(this IStyle style, VectorImage image) => style.backgroundImage = new StyleBackground(image);
		
		public static void SetBackground(this IStyle style, Color color) => style.backgroundColor = color;

		public static void SetSize(this IStyle style, StyleLength width, StyleLength height)
		{
			style.width = width;
			style.height = height;
		}
		
		public static void SetSize(this IStyle style, StyleLength value)
		{
			style.width = value;
			style.height = value;
		}
		
		public static void SetSize(this IStyle style, Vector2 value)
		{
			style.width = value.x;
			style.height = value.y;
		}
		
		public static void SetBorderWidth(this IStyle style, StyleFloat top, StyleFloat right, StyleFloat bottom, StyleFloat left)
		{
			style.borderTopWidth = top;
			style.borderRightWidth = right;
			style.borderBottomWidth = bottom;
			style.borderLeftWidth = left;
		}
		
		public static void SetBorderWidth(this IStyle style, StyleFloat topAndBottom, StyleFloat leftAndRight)
		{
			style.borderTopWidth = topAndBottom;
			style.borderBottomWidth = topAndBottom;
			style.borderLeftWidth = leftAndRight;
			style.borderRightWidth = leftAndRight;
		}
		
		public static void SetBorderWidth(this IStyle style, StyleFloat top, StyleFloat leftAndRight, StyleFloat bottom)
		{
			style.borderTopWidth = top;
			style.borderBottomWidth = bottom;
			style.borderLeftWidth = leftAndRight;
			style.borderRightWidth = leftAndRight;
		}
		
		public static void SetBorderWidth(this IStyle style, StyleFloat value)
		{
			style.borderTopWidth = value;
			style.borderBottomWidth = value;
			style.borderLeftWidth = value;
			style.borderRightWidth = value;
		}
		
		public static void SetBorderColor(this IStyle style, StyleColor top, StyleColor right, StyleColor bottom, StyleColor left)
		{
			style.borderTopColor = top;
			style.borderRightColor = right;
			style.borderBottomColor = bottom;
			style.borderLeftColor = left;
		}
		
		public static void SetBorderColor(this IStyle style, StyleColor topAndBottom, StyleColor leftAndRight)
		{
			style.borderTopColor = topAndBottom;
			style.borderBottomColor = topAndBottom;
			style.borderLeftColor = leftAndRight;
			style.borderRightColor = leftAndRight;
		}
		
		public static void SetBorderColor(this IStyle style, StyleColor top, StyleColor leftAndRight, StyleColor bottom)
		{
			style.borderTopColor = top;
			style.borderBottomColor = bottom;
			style.borderLeftColor = leftAndRight;
			style.borderRightColor = leftAndRight;
		}
		
		public static void SetBorderColor(this IStyle style, StyleColor value)
		{
			style.borderTopColor = value;
			style.borderBottomColor = value;
			style.borderLeftColor = value;
			style.borderRightColor = value;
		}
		
		public static void SetBorderRadius(this IStyle style, StyleLength topLeft, StyleLength topRight, StyleLength bottomRight, StyleLength bottomLeft)
		{
			style.borderTopLeftRadius = topLeft;
			style.borderTopRightRadius = topRight;
			style.borderBottomRightRadius = bottomRight;
			style.borderBottomLeftRadius = bottomLeft;
		}
		
		public static void SetBorderRadius(this IStyle style, StyleLength topLeft, StyleLength topRightAndBottomLeft, StyleLength bottomRight)
		{
			style.borderTopLeftRadius = topLeft;
			style.borderTopRightRadius = topRightAndBottomLeft;
			style.borderBottomLeftRadius = topRightAndBottomLeft;
			style.borderBottomRightRadius = bottomRight;
		}
		
		public static void SetBorderRadius(this IStyle style, StyleLength topLeftAndBottomRight, StyleLength topRightAndBottomLeft)
		{
			style.borderTopLeftRadius = topLeftAndBottomRight;
			style.borderBottomRightRadius = topLeftAndBottomRight;
			style.borderTopRightRadius = topRightAndBottomLeft;
			style.borderBottomLeftRadius = topRightAndBottomLeft;
		}
		
		public static void SetBorderRadius(this IStyle style, StyleLength value)
		{
			style.borderTopLeftRadius = value;
			style.borderTopRightRadius = value;
			style.borderBottomRightRadius = value;
			style.borderBottomLeftRadius = value;
		}

		public static void SetPadding(this IStyle style, StyleLength top, StyleLength right, StyleLength bottom, StyleLength left)
		{
			style.paddingTop = top;
			style.paddingRight = right;
			style.paddingBottom = bottom;
			style.paddingLeft = left;
		}
		
		public static void SetPadding(this IStyle style, StyleLength top, StyleLength leftAndRight, StyleLength bottom)
		{
			style.paddingTop = top;
			style.paddingBottom = bottom;
			style.paddingLeft = leftAndRight;
			style.paddingRight = leftAndRight;
		}
		
		public static void SetPadding(this IStyle style, StyleLength topAndBottom, StyleLength leftAndRight)
		{
			style.paddingTop = topAndBottom;
			style.paddingBottom = topAndBottom;
			style.paddingLeft = leftAndRight;
			style.paddingRight = leftAndRight;
		}
		
		public static void SetPadding(this IStyle style, StyleLength value)
		{
			style.paddingTop = value;
			style.paddingRight = value;
			style.paddingBottom = value;
			style.paddingLeft = value;
		}
		
		public static void SetMargin(this IStyle style, StyleLength top, StyleLength right, StyleLength bottom, StyleLength left)
		{
			style.marginTop = top;
			style.marginRight = right;
			style.marginBottom = bottom;
			style.marginLeft = left;
		}
		
		public static void SetMargin(this IStyle style, StyleLength top, StyleLength leftAndRight, StyleLength bottom)
		{
			style.marginTop = top;
			style.marginBottom = bottom;
			style.marginLeft = leftAndRight;
			style.marginRight = leftAndRight;
		}
		
		public static void SetMargin(this IStyle style, StyleLength topAndBottom, StyleLength leftAndRight)
		{
			style.marginTop = topAndBottom;
			style.marginBottom = topAndBottom;
			style.marginLeft = leftAndRight;
			style.marginRight = leftAndRight;
		}
		
		public static void SetMargin(this IStyle style, StyleLength value)
		{
			style.marginTop = value;
			style.marginRight = value;
			style.marginBottom = value;
			style.marginLeft = value;
		}
		
		public static void SetUnitySlice(this IStyle style, StyleInt top, StyleInt right, StyleInt bottom, StyleInt left)
		{
			style.unitySliceTop = top;
			style.unitySliceRight = right;
			style.unitySliceBottom = bottom;
			style.unitySliceLeft = left;
		}
		
		public static void SetUnitySlice(this IStyle style, StyleInt top, StyleInt leftAndRight, StyleInt bottom)
		{
			style.unitySliceTop = top;
			style.unitySliceBottom = bottom;
			style.unitySliceLeft = leftAndRight;
			style.unitySliceRight = leftAndRight;
		}
		
		public static void SetUnitySlice(this IStyle style, StyleInt topAndBottom, StyleInt leftAndRight)
		{
			style.unitySliceTop = topAndBottom;
			style.unitySliceBottom = topAndBottom;
			style.unitySliceLeft = leftAndRight;
			style.unitySliceRight = leftAndRight;
		}
		
		public static void SetUnitySlice(this IStyle style, StyleInt value)
		{
			style.unitySliceTop = value;
			style.unitySliceRight = value;
			style.unitySliceBottom = value;
			style.unitySliceLeft = value;
		}

		public static void SetLeftBottom(this IStyle style, Vector2 leftBottom)
		{
			style.left = leftBottom.x;
			style.bottom = leftBottom.y;
		}
		
		public static void SetLeftTop(this IStyle style, Vector2 leftTop)
		{
			style.left = leftTop.x;
			style.top = leftTop.y;
		}
		
		public static void SetRightBottom(this IStyle style, Vector2 rightBottom)
		{
			style.right = rightBottom.x;
			style.bottom = rightBottom.y;
		}
		
		public static void SetRightTop(this IStyle style, Vector2 rightTop)
		{
			style.right = rightTop.x;
			style.top = rightTop.y;
		}
	}
}