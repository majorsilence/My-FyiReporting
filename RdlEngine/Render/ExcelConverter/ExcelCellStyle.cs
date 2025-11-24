using ClosedXML.Excel;
using fyiReporting.RDL;
using System.Drawing;
using BorderStyleEnum = fyiReporting.RDL.BorderStyleEnum;

namespace RdlEngine.Render.ExcelConverter
{
	public class ExcelCellStyle
	{
        public static void ApplyStyle(IXLCell cell, StyleInfo styleInfo)
        {
            if (styleInfo == null) return;

            var xlStyle = cell.Style;

            if (!styleInfo.BackgroundColor.IsEmpty && styleInfo.BackgroundColor != Color.Transparent)
            {
                xlStyle.Fill.BackgroundColor = XLColor.FromColor(styleInfo.BackgroundColor);
            }
            else
            {
                xlStyle.Fill.BackgroundColor = XLColor.White;
            }

            xlStyle.Font.FontName = string.IsNullOrEmpty(styleInfo.FontFamily) ? "Arial" : styleInfo.FontFamily;
            xlStyle.Font.FontSize = styleInfo.FontSize <= 0 ? 10 : styleInfo.FontSize;

            xlStyle.Font.Bold = IsBold(styleInfo.FontWeight);

            if (styleInfo.FontStyle == FontStyleEnum.Italic) xlStyle.Font.Italic = true;
            if (styleInfo.TextDecoration == TextDecorationEnum.LineThrough) xlStyle.Font.Strikethrough = true;
            if (styleInfo.TextDecoration == TextDecorationEnum.Underline) xlStyle.Font.Underline = XLFontUnderlineValues.Single;

            if (!styleInfo.Color.IsEmpty)
            {
                xlStyle.Font.FontColor = XLColor.FromColor(styleInfo.Color);
            }

            xlStyle.Alignment.Horizontal = ConvertHorizontalAlign(styleInfo.TextAlign);
            xlStyle.Alignment.Vertical = ConvertVerticalAlign(styleInfo.VerticalAlign);

            xlStyle.Alignment.WrapText = true;

            switch (styleInfo.WritingMode)
            {
                case WritingModeEnum.tb_rl:
                    xlStyle.Alignment.TextRotation = 90;
                    break;
                case WritingModeEnum.tb_lr:
                    xlStyle.Alignment.TextRotation = 90;
                    break;
            }

            ApplyBorders(xlStyle, styleInfo);
        }

        public static void ApplyBorderToRange(IXLRange range, StyleInfo styleInfo)
        {
            var border = range.Style.Border;

            if (HasBorder(styleInfo.BStyleTop))
            {
                border.TopBorder = ConvertBorderStyle(styleInfo.BStyleTop, styleInfo.BWidthTop);
                border.TopBorderColor = XLColor.FromColor(styleInfo.BColorTop);
            }
            if (HasBorder(styleInfo.BStyleBottom))
            {
                border.BottomBorder = ConvertBorderStyle(styleInfo.BStyleBottom, styleInfo.BWidthBottom);
                border.BottomBorderColor = XLColor.FromColor(styleInfo.BColorBottom);
            }
            if (HasBorder(styleInfo.BStyleLeft))
            {
                border.LeftBorder = ConvertBorderStyle(styleInfo.BStyleLeft, styleInfo.BWidthLeft);
                border.LeftBorderColor = XLColor.FromColor(styleInfo.BColorLeft);
            }
            if (HasBorder(styleInfo.BStyleRight))
            {
                border.RightBorder = ConvertBorderStyle(styleInfo.BStyleRight, styleInfo.BWidthRight);
                border.RightBorderColor = XLColor.FromColor(styleInfo.BColorRight);
            }
        }

        private static void ApplyBorders(IXLStyle style, StyleInfo info)
        {
            if (HasBorder(info.BStyleTop))
            {
                style.Border.TopBorder = ConvertBorderStyle(info.BStyleTop, info.BWidthTop);
                style.Border.TopBorderColor = XLColor.FromColor(info.BColorTop);
            }
            if (HasBorder(info.BStyleBottom))
            {
                style.Border.BottomBorder = ConvertBorderStyle(info.BStyleBottom, info.BWidthBottom);
                style.Border.BottomBorderColor = XLColor.FromColor(info.BColorBottom);
            }
            if (HasBorder(info.BStyleLeft))
            {
                style.Border.LeftBorder = ConvertBorderStyle(info.BStyleLeft, info.BWidthLeft);
                style.Border.LeftBorderColor = XLColor.FromColor(info.BColorLeft);
            }
            if (HasBorder(info.BStyleRight))
            {
                style.Border.RightBorder = ConvertBorderStyle(info.BStyleRight, info.BWidthRight);
                style.Border.RightBorderColor = XLColor.FromColor(info.BColorRight);
            }
        }

        private static bool HasBorder(BorderStyleEnum s) => s != BorderStyleEnum.None;

        private static bool IsBold(FontWeightEnum w)
        {
            return w == FontWeightEnum.Bold || w == FontWeightEnum.Bolder ||
                   w == FontWeightEnum.W700 || w == FontWeightEnum.W800 || w == FontWeightEnum.W900;
        }

        private static XLAlignmentHorizontalValues ConvertHorizontalAlign(TextAlignEnum a)
        {
            switch (a)
            {
                case TextAlignEnum.Center: return XLAlignmentHorizontalValues.Center;
                case TextAlignEnum.Right: return XLAlignmentHorizontalValues.Right;
                case TextAlignEnum.Justified: return XLAlignmentHorizontalValues.Justify;
                default: return XLAlignmentHorizontalValues.Left;
            }
        }

        private static XLAlignmentVerticalValues ConvertVerticalAlign(VerticalAlignEnum a)
        {
            switch (a)
            {
                case VerticalAlignEnum.Middle: return XLAlignmentVerticalValues.Center;
                case VerticalAlignEnum.Bottom: return XLAlignmentVerticalValues.Bottom;
                default: return XLAlignmentVerticalValues.Top;
            }
        }

        private static XLBorderStyleValues ConvertBorderStyle(BorderStyleEnum style, float width)
        {
            switch (style)
            {
                case BorderStyleEnum.Dashed: return XLBorderStyleValues.Dashed;
                case BorderStyleEnum.Dotted: return XLBorderStyleValues.Dotted;
                case BorderStyleEnum.Double: return XLBorderStyleValues.Double;
                case BorderStyleEnum.Solid:
                    if (width <= 1.0f) return XLBorderStyleValues.Thin;
                    if (width < 2.0f) return XLBorderStyleValues.Medium;
                    return XLBorderStyleValues.Thick;
                default: return XLBorderStyleValues.None;
            }
        }
    }
}
