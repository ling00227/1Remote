﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using _1RM.Service;
using _1RM.Utils;
using _1RM.Utils.Tracing;
using Shawn.Utils;
using Shawn.Utils.Wpf;
using Shawn.Utils.Wpf.FileSystem;

namespace _1RM.View.Settings.General
{
    public class GeneralSettingViewModel : NotifyPropertyChangedBase
    {
        private readonly ConfigurationService _configurationService;
        private readonly LanguageService _languageService;

        public GeneralSettingViewModel(ConfigurationService configurationService, LanguageService languageService)
        {
            _configurationService = configurationService;
            _languageService = languageService;
        }


        public Dictionary<string, string> Languages => _languageService.LanguageCode2Name;
        public string Language
        {
            get => _configurationService.General.CurrentLanguageCode;
            set
            {
                Debug.Assert(Languages.ContainsKey(value));
                if (SetAndNotifyIfChanged(ref _configurationService.General.CurrentLanguageCode, value))
                {
                    // reset lang service
                    _languageService.SetLanguage(value);
                    _configurationService.Save();
                }
            }
        }

        private bool _appStartAutomatically = false;
        public bool AppStartAutomatically
        {
            get => _appStartAutomatically;
            set
            {
                ConfigurationService.SetSelfStart(value);
                _appStartAutomatically = value;
                RaisePropertyChanged();
            }
        }

        public int CloseButtonBehavior
        {
            get => _configurationService.General.CloseButtonBehavior;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.CloseButtonBehavior, value))
                {
                    _configurationService.Save();
                }
            }
        }

        public bool ConfirmBeforeClosingSession
        {
            get => _configurationService.General.ConfirmBeforeClosingSession;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.ConfirmBeforeClosingSession, value))
                {
                    _configurationService.Save();
                }
            }
        }


        public bool ShowSessionIconInSessionWindow
        {
            get => _configurationService.General.ShowSessionIconInSessionWindow;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.ShowSessionIconInSessionWindow, value))
                {
                    _configurationService.Save();
                }
            }
        }

        public string LogPath => SimpleLogHelper.GetFileFullName();

        public SimpleLogHelper.EnumLogLevel LogLevel
        {
            get => SimpleLogHelper.WriteLogLevel;
            set
            {
                if (SimpleLogHelper.WriteLogLevel != value)
                {
                    SimpleLogHelper.WriteLogLevel = value;
                    SimpleLogHelper.PrintLogLevel = value;
                    _configurationService.General.LogLevel = (int)value;
                    RaisePropertyChanged();
                    _configurationService.Save();
                }
            }
        }

        //public bool TabAutoFocusContent
        //{
        //    get => _configurationService.General.TabAutoFocusContent;
        //    set
        //    {
        //        if (SetAndNotifyIfChanged(ref _configurationService.General.TabAutoFocusContent, value))
        //        {
        //            _configurationService.Save();
        //        }
        //    }
        //}

        public bool CopyPortWhenCopyAddress
        {
            get => _configurationService.General.CopyPortWhenCopyAddress;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.CopyPortWhenCopyAddress, value))
                {
                    _configurationService.Save();
                }
            }
        }

        public bool TabWindowCloseButtonOnLeft
        {
            get => _configurationService.General.TabWindowCloseButtonOnLeft;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.TabWindowCloseButtonOnLeft, value))
                {
                    _configurationService.Save();
                }
            }
        }

        public bool TabWindowSetFocusToLocalDesktopOnMouseLeaveRdpWindow
        {
            get => _configurationService.General.TabWindowSetFocusToLocalDesktopOnMouseLeaveRdpWindow;
            set
            {
                if (SetAndNotifyIfChanged(ref _configurationService.General.TabWindowSetFocusToLocalDesktopOnMouseLeaveRdpWindow, value))
                {
                    _configurationService.Save();
                }
            }
        }

        private RelayCommand? _cmdExploreTo = null;
        public RelayCommand CmdExploreTo
        {
            get
            {
                return _cmdExploreTo ??= new RelayCommand((o) =>
                {
                    try
                    {
                        SelectFileHelper.OpenInExplorerAndSelect(LogPath);
                    }
                    catch (Exception e)
                    {
                        UnifyTracing.Error(e);
                    }
                });
            }
        }
    }
}
