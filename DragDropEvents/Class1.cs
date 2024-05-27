using System;

using Avalonia.Controls;
using Avalonia.Input;

using Avalonia.Media;

namespace DragDropEvents;

#nullable enable

public class DragDropWindowDecorator
{
    public DragDropWindowDecorator(
        Window parent,
        Panel dragDropContentPanel,
        Func<DragEventArgs, bool> allowsDropFunc,
        Action<DragEventArgs> onDragEnterFunc,
        Action<DragEventArgs> onDropFunc)
    {
        mParent = parent;
        mDragDropContainerPanel = new DragDropContainerPanel();
        mDragDropContainerPanel.Child = dragDropContentPanel;
        mDragDropContainerPanel.IsVisible = false;

        Control? parentContent = mParent.Content as Control;
        parent.Content = null;

        Panel panel = new Panel();
        if (parentContent != null) panel.Children.Add(parentContent);
        panel.Children.Add(mDragDropContainerPanel);

        mParent.Content = panel;

        mAllowsDropFunc = allowsDropFunc;
        mOnDragEnterFunc = onDragEnterFunc;
        mOnDropFunc = onDropFunc;

        DragDrop.SetAllowDrop(mParent, true);
        DragDrop.SetAllowDrop(mDragDropContainerPanel, true);

        mParent.AddHandler(
            DragDrop.DragEnterEvent,
            OnDragEnter);
        mDragDropContainerPanel.AddHandler(
            DragDrop.DragLeaveEvent,
            OnDragLeave);

        mDragDropContainerPanel.AddHandler(
            DragDrop.DropEvent,
            OnDrop);
    }

    public void Dispose()
    {
        mParent.RemoveHandler(
            DragDrop.DragEnterEvent,
            OnDragEnter);

        mDragDropContainerPanel.RemoveHandler(
            DragDrop.DragLeaveEvent,
            OnDragLeave);

        mDragDropContainerPanel.RemoveHandler(
            DragDrop.DropEvent,
            OnDrop);
    }

    void OnDragEnter(object? sender, DragEventArgs e)
    {
        if (mParent.Content == null)
            return;

        if (!mAllowsDropFunc(e))
            return;

        if (!mDragDropContainerPanel.IsVisible)
        {
            mDragDropContainerPanel.IsVisible = true;

            mOnDragEnterFunc(e);
        }
    }

    void OnDragLeave(object? sender, DragEventArgs e)
    {
        if (mParent.Content == null)
            return;

        if (mDragDropContainerPanel.IsVisible)
        {
            mDragDropContainerPanel.IsVisible = false;
        }
    }

    void OnDrop(object? sender, DragEventArgs e)
    {
        if (mParent.Content == null)
            return;

        mOnDropFunc(e);

        if (mDragDropContainerPanel.IsVisible)
        {
            mDragDropContainerPanel.IsVisible = false;
        }
    }

    class DragDropContainerPanel : Border
    {
        public DragDropContainerPanel()
        {
            Background = Brushes.White;
            Opacity = 0.9;
        }
    }

    readonly Window mParent;
    readonly DragDropContainerPanel mDragDropContainerPanel;
    readonly Func<DragEventArgs, bool> mAllowsDropFunc;
    readonly Action<DragEventArgs> mOnDragEnterFunc;
    readonly Action<DragEventArgs> mOnDropFunc;
}