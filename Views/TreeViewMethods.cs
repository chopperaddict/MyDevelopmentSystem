using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Media;

namespace MyDev . Views
{
    public static class TreeViewMethods
    {
        public static List<TreeViewItem> FindTreeViewItems ( this Visual @this )
        {
            if ( @this == null )
                return null;

            var result = new List<TreeViewItem> ( );

            var frameworkElement = @this as FrameworkElement;
            if ( frameworkElement != null )
            {
                frameworkElement . ApplyTemplate ( );
            }

            Visual child = null;
            for ( int i = 0, count = VisualTreeHelper . GetChildrenCount ( @this ) ; i < count ; i++ )
            {
                child = VisualTreeHelper . GetChild ( @this , i ) as Visual;
                if ( child == null )
                    continue;
                var treeViewItem = child as TreeViewItem;
                if ( treeViewItem != null )
                {
                    result . Add ( treeViewItem );
                    if ( !treeViewItem . IsExpanded )
                    {
                        treeViewItem . IsExpanded = true;
                        treeViewItem . UpdateLayout ( );
                    }
                }
                else 
                    continue;
                foreach ( var childTreeViewItem in FindTreeViewItems ( child ) )
                {
                    result . Add ( childTreeViewItem );
                }
            }
            return result;
        }

        public static TreeViewItem [ ] getTreeViewItems ( TreeView treeView )
        {
            List<TreeViewItem> returnItems = new List<TreeViewItem> ( );
            for ( int x = 0 ; x < treeView . Items . Count ; x++ )
            {
                returnItems . AddRange ( getTreeViewItems ( ( TreeViewItem ) treeView . Items [ x ] ) );
            }
            return returnItems . ToArray ( );
        }
        public static TreeViewItem [ ] getTreeViewItems ( TreeViewItem currentTreeViewItem )
        {
            List<TreeViewItem> returnItems = new List<TreeViewItem> ( );
            returnItems . Add ( currentTreeViewItem );
            for ( int x = 0 ; x < currentTreeViewItem . Items . Count ; x++ )
            {
                returnItems . AddRange ( getTreeViewItems ( ( TreeViewItem ) currentTreeViewItem . Items [ x ] ) );
            }
            return returnItems . ToArray ( );
        }
    }
}
