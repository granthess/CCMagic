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
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using S3ToolKit.Utils.Logging;
using System.Diagnostics;

namespace CCMagic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());
 
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            //detect duplicate instances and exit the new one
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                // Tell the user he can only have on instance running
                MessageBox.Show("Only one instance of this program can be open at any one time", "Duplicate instance detected", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                // Close the application
                Application.Current.Shutdown();
            }

            // Check for command line arguments
            // Only recognized parameter is /LOG:<level> where level is a number between 0 and 5 that corresponds to 
            // the LogManager.LogLevel enum
            // if <level> is 0 or the parameter is not there, turn logging off
            if (e.Args.Length > 0)
            {
                bool found = false;
                try
                {
                    foreach (string entry in e.Args)
                    {
                        if (entry.ToUpper().StartsWith("/LOG:"))
                        {
                            LogManager.Level = (LogManager.LogLevel)int.Parse(entry.Substring(5).Trim());
                            LogManager.Enable();
                            found = true;
                        }
                    }
                }
                catch (FormatException)
                {
                    found = false;
                }
                if (!found)
                {
                    LogManager.Disable();
                }
            }
            else
            {
                LogManager.Disable();
            }

            //  We're going to default to log level 5 if not available            
            if (LogManager.IsEnabled != true)
            {
                LogManager.Enable();
                LogManager.Level = LogManager.LogLevel.Debug;
            }
        }
    }
}
