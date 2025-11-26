using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;
using System.Collections.Generic;

namespace XPTable.AvaloniaApp.Controls
{
    public class XPTableControl : Control
    {
        private const double HeaderHeight = 36;
        private Typeface _typeface = new Typeface("Segoe UI");
        private List<Column> _columns;
        private int? _sortedColumnIndex = null;
        private bool _sortAscending = true;

        public XPTableControl()
        {
            // Default demo columns - replace or expose property to set real columns
            _columns = new List<Column>
            {
                new Column("Name", 240),
                new Column("Type", 120),
                new Column("Modified", 160),
                new Column("Size", 100)
            };

            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;
        }

        // Simple column model for header rendering
        private class Column
        {
            public string Header { get; }
            public double Width { get; set; }

            public Column(string header, double width)
            {
                Header = header;
                Width = width;
            }
        }

        private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            // placeholder for future mouse-up logic
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            // placeholder for hover state if desired
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            var p = e.GetPosition(this);
            if (p.Y <= HeaderHeight)
            {
                // determine which column was clicked
                double x = 0;
                for (int i = 0; i < _columns.Count; i++)
                {
                    var col = _columns[i];
                    if (p.X >= x && p.X <= x + col.Width)
                    {
                        if (_sortedColumnIndex == i)
                            _sortAscending = !_sortAscending;
                        else
                        {
                            _sortedColumnIndex = i;
                            _sortAscending = true;
                        }

                        // TODO: hook into sorting logic in XPTable.Core
                        InvalidateVisual();
                        e.Handled = true;
                        return;
                    }
                    x += col.Width;
                }
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // background
            var bounds = new Rect(Bounds.Size);
            context.FillRectangle(Brushes.White, bounds);

            // header background
            var headerRect = new Rect(0, 0, Bounds.Width, HeaderHeight);
            context.FillRectangle(Brushes.LightGray, headerRect);

            // draw header columns
            double xPos = 0;
            for (int i = 0; i < _columns.Count; i++)
            {
                var col = _columns[i];
                var colRect = new Rect(xPos, 0, col.Width, HeaderHeight);

                // subtle separator for column (light)
                var separatorPen = new Pen(Brushes.Gray, 1);
                context.DrawLine(separatorPen, new Point(xPos + col.Width, 0), new Point(xPos + col.Width, HeaderHeight));

                // draw header text
                var padding = 8;
                var constraint = new Size(Math.Max(0, col.Width - padding * 2), HeaderHeight);
                var formatted = new FormattedText
                {
                    Text = col.Header,
                    Typeface = _typeface,
                    Constraint = constraint,
                    TextAlignment = TextAlignment.Left,
                    Trimming = TextTrimming.CharacterEllipsis
                };

                // vertically center text
                double textY = (HeaderHeight - formatted.Bounds.Height) / 2.0;
                context.DrawText(Brushes.Black, new Point(xPos + padding, textY), formatted);

                // draw sort glyph if this column is sorted
                if (_sortedColumnIndex == i)
                {
                    var glyph = _sortAscending ? "▲" : "▼";
                    var glyphFormatted = new FormattedText
                    {
                        Text = glyph,
                        Typeface = _typeface,
                        Constraint = new Size(32, HeaderHeight),
                        TextAlignment = TextAlignment.Right
                    };

                    // place glyph near right side of column
                    var glyphX = xPos + col.Width - padding - glyphFormatted.Bounds.Width;
                    var glyphY = (HeaderHeight - glyphFormatted.Bounds.Height) / 2.0;
                    context.DrawText(Brushes.Black, new Point(glyphX, glyphY), glyphFormatted);
                }

                xPos += col.Width;
            }

            // placeholder: draw first row divider (visual cue)
            var rowDividerPen = new Pen(Brushes.LightGray, 1);
            context.DrawLine(rowDividerPen, new Point(0, HeaderHeight), new Point(Bounds.Width, HeaderHeight));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // default desired size: keep header visible and let layout decide width
            double desiredHeight = Math.Max(200, HeaderHeight + 100);
            double desiredWidth = availableSize.Width.IsFinite() ? availableSize.Width : TotalColumnsWidth();
            return new Size(desiredWidth, desiredHeight);
        }

        private double TotalColumnsWidth()
        {
            double sum = 0;
            foreach (var c in _columns) sum += c.Width;
            return sum;
        }
    }

    static class SizeExtensions
    {
        public static bool IsFinite(this double d) => !double.IsInfinity(d) && !double.IsNaN(d);
    }
}
