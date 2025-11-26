using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace XPTable.AvaloniaApp.Controls
{
    // Minimal skeleton to get started. Port your drawing logic from WinForms Paint
    // to Avalonia's OnRender / DrawingContext or use Skia directly.
    public class XPTableControl : Control
    {
        public XPTableControl()
        {
            // enable pointer events
            this.PointerPressed += XPTableControl_PointerPressed;
            this.PointerMoved += XPTableControl_PointerMoved;
            this.PointerReleased += XPTableControl_PointerReleased;
        }

        private void XPTableControl_PointerReleased(object? sender, PointerReleasedEventArgs e)
        {
            // TODO: translate to XPTable selection logic
        }

        private void XPTableControl_PointerMoved(object? sender, PointerEventArgs e)
        {
            // TODO: update hover state
        }

        private void XPTableControl_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // TODO: translate to mouse down
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // Example: simple background and placeholder grid
            var bounds = new Rect(new Point(0, 0), this.Bounds.Size);
            context.FillRectangle(Brushes.White, bounds);

            // Draw a placeholder header row
            var headerHeight = 30;
            var headerRect = new Rect(new Point(0, 0), new Size(Bounds.Width, headerHeight));
            context.FillRectangle(Brushes.LightGray, headerRect);
            var tf = new Typeface("Segoe UI");

            var formatted = new FormattedText
            {
                Text = "XPTable (Avalonia) - placeholder",
                Typeface = tf,
                Constraint = new Size(Bounds.Width, headerHeight),
                TextAlignment = TextAlignment.Left
            };

            context.DrawText(Brushes.Black, new Point(8, 6), formatted);

            // You will replace the above with your actual row/column drawing logic.
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            // Provide a default desired size or compute based on content
            return new Size(availableSize.Width, availableSize.Height);
        }
    }
}