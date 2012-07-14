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
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand MyCommand { get; private set; }

        
        public string Data { get; set; }
        public bool Enabled {get;private set;}
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        
        public MainViewModel()
        {
            MyCommand = new RelayCommand(() => DoSomething(), () => CheckSomething());
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
            }
        }

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