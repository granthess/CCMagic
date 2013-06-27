/*
    Copyright 2012, Grant Hess

    This file is part of CC Magic.

    S3ToolKit.Utils is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Foobar is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with CC Magic.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CCMagic.ViewModel;
using S3ToolKit.MagicEngine.Database;

namespace CCMagic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel Vm { get { return (MainViewModel)TopGrid.DataContext; } }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CFG_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Vm != null)
            {
                Vm.CCMEngine.ViewContext.SaveChanges();
            }
        }

        private void SETS_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Vm != null)
            {
                Vm.CCMEngine.ViewContext.SaveChanges();
            }
        }

        private void EnabledSetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Vm.CCMEngine.CFGSetsToDisable.Clear();
            foreach (var item in (sender as ListView).SelectedItems)
            {
                Vm.CCMEngine.CFGSetsToDisable.Add(item as SetEntity);
            }
            
        }

        private void DisabledSetList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Vm.CCMEngine.CFGSetsToEnable.Clear();
            foreach (var item in (sender as ListView).SelectedItems)
            {
                Vm.CCMEngine.CFGSetsToEnable.Add(item as SetEntity);
            }
        }

        private void Sets_Text_Changed(object sender, TextChangedEventArgs e)
        {

        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Vm.CCMEngine.CurrentSet = (sender as TreeView).SelectedItem as SetEntity;
        }



    }
}
