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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using S3ToolKit.MagicEngine;
using System.Collections.ObjectModel;
using S3ToolKit.MagicEngine.Core;
using System.Data.Entity;
using S3ToolKit.MagicEngine.Database;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;

namespace CCMagic.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public RelayCommand MyCommand { get; private set; }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        // ActiveSharp property change notification implementation
        // use this layout to set properties:
        //     public int Foo
        //     {
        //       get { return _foo; }
        //       set { SetValue(ref _foo, value); }   // assigns value and does prop change notification, all in one line
        //     }
        protected void SetValue<T>(ref T field, T value)
        {
            field = value;   //Actually assign the new value
            PropertyInfo changedProperty = ActiveSharp.PropertyMapping.PropertyMap.GetProperty(this, ref field);

            OnPropertyChanged(changedProperty.Name);
        }
        #endregion

        public string Data { get; set; }
        public bool Enabled { get; private set; }

        #region Pure Import from Engine
        public Engine CCMEngine { get { return Engine.Instance; } }
        #endregion

        #region VMProperties
        private ObservableCollection<GameVersionDisplayEntry> _GameInfo;
        public ObservableCollection<GameVersionDisplayEntry> GameInfo { get { return GetGameVersionInfo(); } }
        private ObservableCollection<GameVersionDisplayEntry> GetGameVersionInfo()
        {
            if (_GameInfo == null)
            {
                if (!IsInDesignMode)
                {
                    _GameInfo = new ObservableCollection<GameVersionDisplayEntry>();
                    foreach (GameVersionEntry Entry in Engine.Instance.GameInfo)
                    {
                        _GameInfo.Add(new GameVersionDisplayEntry(Entry));
                    }
                }
                else
                {
                }
            }

            return _GameInfo;
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        public MainViewModel()
        {
            MyCommand = new RelayCommand(() => DoSomething(), () => CheckSomething());

            // Commands for Configurations Tab
            CFGAdd = new RelayCommand(() => DoCFGAdd(), () => CheckCFGAdd());
            CFGRemove = new RelayCommand(() => DoCFGRemove(), () => CheckCFGRemove());
            CFGRemoveAllSets = new RelayCommand(() => DoCFGRemoveAllSets(), () => CheckCFGRemoveAllSets());
            CFGRemoveASet = new RelayCommand(() => DoCFGRemoveASet(), () => CheckCFGRemoveASet());
            CFGAddASet = new RelayCommand(() => DoCFGAddASet(), () => CheckCFGAddASet());
            CFGAddAllSets = new RelayCommand(() => DoCFGAddAllSets(), () => CheckCFGAddAllSets());

            // Commands for Sets Tab
            SETAdd = new RelayCommand(() => DoSETAdd(), () => CheckSETAdd());
            SETRemove = new RelayCommand(() => DoSETRemove(), () => CheckSETRemove());


            Enabled = true;

            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                Data = "Visible in design mode";
            }
            else
            {
                // Code runs "for real"
                Data = "Visible in run mode";

                CCMEngine.Initialize();
            }
        }


        #region Commands
        // Configuration Tab Commands
        public RelayCommand CFGAdd { get; private set; }
        public RelayCommand CFGRemove { get; private set; }
        public RelayCommand CFGRemoveAllSets { get; private set; }
        public RelayCommand CFGRemoveASet { get; private set; }
        public RelayCommand CFGAddASet { get; private set; }
        public RelayCommand CFGAddAllSets { get; private set; }

        public RelayCommand SETAdd { get; private set; }
        public RelayCommand SETRemove { get; private set; }


        public bool CFGNameChanged { get; set; }
        public bool CFGDescChanged { get; set; }

        private void DoCFGAdd()
        {
            CCMEngine.AddConfig();
        }

        private bool CheckCFGAdd()
        {
            return true;
        }

        private void DoCFGRemove()
        {
            CCMEngine.RemoveConfig();
        }

        private bool CheckCFGRemove()
        {
            if (IsInDesignMode)
            {
                return false;
            }
            else
            {
                return !CCMEngine.CurrentConfig.IsDefault;
            }
        }

        private void DoCFGRemoveAllSets()
        {
            List<SetEntity> SetsToChange = new List<SetEntity>();
            foreach (SetEntity entry in CCMEngine.EnabledSets)
            {
                SetsToChange.Add(entry);
            }

            CCMEngine.DisableSets(SetsToChange);
        }

        private bool CheckCFGRemoveAllSets()
        {
            return CCMEngine.EnabledSets.Count != 0;
        }

        private void DoCFGRemoveASet()
        {
            CCMEngine.DisableSets(CCMEngine.CFGSetsToDisable);
        }

        private bool CheckCFGRemoveASet()
        {
            return CCMEngine.CFGSetsToDisable.Count > 0;
        }

        private void DoCFGAddASet()
        {
            CCMEngine.EnableSets(CCMEngine.CFGSetsToEnable);
        }

        private bool CheckCFGAddASet()
        {
            return CCMEngine.CFGSetsToEnable.Count > 0;
        }

        private void DoCFGAddAllSets()
        {
            List<SetEntity> SetsToChange = new List<SetEntity>();
            foreach (SetEntity entry in CCMEngine.DisabledSets)
            {
                SetsToChange.Add(entry);
            }

            CCMEngine.EnableSets(SetsToChange);
        }

        private bool CheckCFGAddAllSets()
        {
            return CCMEngine.DisabledSets.Count != 0;
        }

        // Set tab commands
        private void DoSETAdd()
        {
            CCMEngine.AddSet();
        }

        private bool CheckSETAdd()
        {
            return true;
        }


        private void DoSETRemove()
        {
            CCMEngine.RemoveSet();
        }

        private bool CheckSETRemove()
        {
            if (!IsInDesignMode)
            {
                return !CCMEngine.CurrentSet.IsDefault;
            }
            else
            {
                return false;
            }
        }

        #endregion


        private void DoSomething()
        {
            Data = Data + " Clicked";
            Enabled = false;
            RaisePropertyChanged("Data");
        }

        private bool CheckSomething()
        {
            return Enabled;
        }
    }
}