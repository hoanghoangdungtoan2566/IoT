﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows
{
    public class MyMenuItemInfo
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string ClassName { get; set; }
        public string IconName { get; set; }
        public bool BeginGroup { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool Disabled { get; set; }
        public bool IsActive { get; set; }
        public List<MyMenuItemInfo> Childs { get; set; }
    }

}
namespace System.Windows.Controls
{

    public class MyMenuItem : MyButtonBase
    {
        MyMenuItemInfo _data;
        public MyMenuItemInfo Data => _data;
        public MyMenuItem(MyMenuItemInfo info)
        {
            _data = info;
        }
    }
    public abstract class MyMenu : PanelElement<StackPanel>
    {
        protected abstract MyMenuItem CreateItem(MyMenuItemInfo info);
        public event Action<MyMenuItem> ItemClicked;

        public MyMenuItem Add(MyMenuItemInfo info)
        {
            var item = CreateItem(info);

            item.Text = info.Text;
            item.Url = info.Url;

            return Add(item);
        }
        public MyMenuItem Add(MyMenuItem item)
        {
            base.Add(item);

            item.Click += (e) => ItemClicked?.Invoke(item);

            MenuItemAdded?.Invoke(item);
            return item;
        }
        public MyMenuItem Add(string text, string url)
        {
            var item = CreateItem(new MyMenuItemInfo { Text = text, Url = url });
            item.Text = text;
            item.Url = url;

            return Add(item);
        }

        public event Action<MyMenuItem> MenuItemAdded;

        System.Collections.IEnumerable _itemsSource;
        public System.Collections.IEnumerable ItemsSource
        {
            get => _itemsSource;
            set
            {
                if (_itemsSource == value) { return; }
                this.Content.Children.Clear();

                if ((_itemsSource = value) != null)
                {
                    foreach (var obj in value)
                    {
                        var info = obj as MyMenuItemInfo;
                        if (info == null)
                        {
                            info = System.Json.Convert<MyMenuItemInfo>(obj);
                        }
                        this.Add(info);
                    }
                }
            }
        }

        public MyMenuItem this[int index]
        {
            get => (MyMenuItem)this.Content.Children[index];
        }
        public MyMenu()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            this.Css = "menu";
        }
    }

    public class MyTopMenu : MyMenu
    {
        class MI : MyMenuItem
        {
            public MI(MyMenuItemInfo info) : base(info)
            {
                Css = "top-menu-item";
                Content.Margin = new Thickness(10, 0, 10, 0);
            }
        }
        public MyTopMenu()
        {
            this.Content.Orientation = Orientation.Horizontal;
        }
        protected override MyMenuItem CreateItem(MyMenuItemInfo info)
        {
            return new MI(info);
        }
    }

    public class MyNavMenu : MyMenu
    {
        class MI : MyMenuItem
        {
            public MI(MyMenuItemInfo info) : base(info)
            {
                BorderThickness = new Thickness(0, 0, 0, 1);
                Content.Margin = new Thickness(20, 10, 0, 10);
                Content.HorizontalAlignment = HorizontalAlignment.Left;
                Css = "nav-menu-item";
            }
        }
        public MyNavMenu() { }
        protected override MyMenuItem CreateItem(MyMenuItemInfo info)
        {
            return new MI(info);
        }
    }
}
