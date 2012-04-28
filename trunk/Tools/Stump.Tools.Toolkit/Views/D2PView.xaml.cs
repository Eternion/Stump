using System;
using System.Waf.Applications;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Stump.Tools.Toolkit.ModelViews.D2P;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Panel = System.Windows.Controls.Panel;

namespace Stump.Tools.Toolkit.Views
{
    /// <summary>
    /// Interaction logic for D2PView.xaml
    /// </summary>
    public partial class D2PView : IView
    {
        private readonly Lazy<D2PViewModel> m_viewModel;

        public D2PView()
        {
            InitializeComponent();

            m_viewModel = new Lazy<D2PViewModel>(() => this.GetViewModel<D2PViewModel>());
        }

        public D2PViewModel ViewModel
        {
            get { return m_viewModel.Value; }
        }

        public void SelectNode(D2PTreeViewNode node)
        {
            TreeViewItem item = GetTreeViewItem(TreeView, node);

            if (item != null)
                item.IsSelected = true;
        }

        private void OnSearchTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ViewModel.SearchCommand.Execute(null);
                e.Handled = true;
            }
        }

        #region Treeview Search
        /// <summary>
        /// Recursively search for an item in this subtree.
        /// </summary>
        /// <param name="container">
        /// The parent ItemsControl. This can be a TreeView or a TreeViewItem.
        /// </param>
        /// <param name="item">
        /// The item to search for.
        /// </param>
        /// <returns>
        /// The TreeViewItem that contains the specified item.
        /// </returns>
        private TreeViewItem GetTreeViewItem(ItemsControl container, object item)
        {
            if (container != null)
            {
                if (container.DataContext == item)
                {
                    return container as TreeViewItem;
                }

                // Expand the current container
                if (container is TreeViewItem && !((TreeViewItem) container).IsExpanded)
                {
                    container.SetValue(TreeViewItem.IsExpandedProperty, true);
                }

                // Try to generate the ItemsPresenter and the ItemsPanel.
                // by calling ApplyTemplate.  Note that in the 
                // virtualizing case even if the item is marked 
                // expanded we still need to do this step in order to 
                // regenerate the visuals because they may have been virtualized away.

                container.ApplyTemplate();
                var itemsPresenter =
                    (ItemsPresenter) container.Template.FindName("ItemsHost", container);
                if (itemsPresenter != null)
                {
                    itemsPresenter.ApplyTemplate();
                }
                else
                {
                    // The Tree template has not named the ItemsPresenter, 
                    // so walk the descendents and find the child.
                    itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    if (itemsPresenter == null)
                    {
                        container.UpdateLayout();

                        itemsPresenter = FindVisualChild<ItemsPresenter>(container);
                    }
                }

                var itemsHostPanel = (Panel) VisualTreeHelper.GetChild(itemsPresenter, 0);


                // Ensure that the generator for this panel has been created.
                UIElementCollection children = itemsHostPanel.Children;

                var virtualizingPanel =
                    itemsHostPanel as TreeViewVirtualizingStackPanel;

                for (int i = 0, count = container.Items.Count; i < count; i++)
                {
                    TreeViewItem subContainer;
                    if (virtualizingPanel != null)
                    {
                        // Bring the item into view so 
                        // that the container will be generated.
                        virtualizingPanel.BringIntoView(i);

                        subContainer =
                            (TreeViewItem) container.ItemContainerGenerator.
                                               ContainerFromIndex(i);
                    }
                    else
                    {
                        subContainer =
                            (TreeViewItem) container.ItemContainerGenerator.
                                               ContainerFromIndex(i);

                        // Bring the item into view to maintain the 
                        // same behavior as with a virtualizing panel.
                        if (subContainer != null)
                            subContainer.BringIntoView();
                    }

                    if (subContainer != null)
                    {
                        // Search the next level for the object.
                        TreeViewItem resultContainer = GetTreeViewItem(subContainer, item);
                        if (resultContainer != null)
                        {
                            return resultContainer;
                        }
                        else
                        {
                            // The object is not under this TreeViewItem
                            // so collapse it.
                            subContainer.IsExpanded = false;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Search for an element of a certain type in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of element to find.</typeparam>
        /// <param name="visual">The parent element.</param>
        /// <returns></returns>
        private T FindVisualChild<T>(Visual visual) where T : Visual
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
            {
                var child = (Visual) VisualTreeHelper.GetChild(visual, i);
                if (child != null)
                {
                    var correctlyTyped = child as T;
                    if (correctlyTyped != null)
                    {
                        return correctlyTyped;
                    }

                    var descendent = FindVisualChild<T>(child);
                    if (descendent != null)
                    {
                        return descendent;
                    }
                }
            }

            return null;
        }

#endregion
    }

    public class TreeViewVirtualizingStackPanel : VirtualizingStackPanel
    {
        /// <summary>
        /// Publically expose BringIndexIntoView.
        /// </summary>
        public void BringIntoView(int index)
        {
            BringIndexIntoView(index);
        }
    }
}