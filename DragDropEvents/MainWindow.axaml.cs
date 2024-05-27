using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace DragDropEvents;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        TextBlock textBlock = new TextBlock();
        textBlock.FontSize = 18;
        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
        textBlock.VerticalAlignment = VerticalAlignment.Center;
        textBlock.Text = "Drop here files or folders";

        this.Content = textBlock;

        Panel dragDropContentPanel = new Panel();
        dragDropContentPanel.Background = Brushes.Beige;

        DragDropWindowDecorator dragDropWindowDecorator = new DragDropWindowDecorator(
            this,
            dragDropContentPanel,
            AllowsDrop,
            OnDragEnter,
            OnDrop);
    }

    void OnDrop(DragEventArgs obj)
    {
        Console.WriteLine("OnDrop");
    }

    void OnDragEnter(DragEventArgs obj)
    {
        Console.WriteLine("OnDragEnter");
    }

    bool AllowsDrop(DragEventArgs arg)
    {
        return true;
    }
}