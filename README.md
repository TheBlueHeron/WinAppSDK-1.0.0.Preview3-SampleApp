### WinAppSDK1.0.0 App
A WinUI 3 application that helps you convert address data to Latitude and Longitude values and route date for use with the Google Maps javascript API.

## File Structure
```
.
├── WinAppSDKApp/ - WinUI 3 Desktop app
│ ├── Activation/ - app activation handlers
│ ├── Behaviors/ - UI controls behaviors
│ ├── Contracts/ - class interfaces
│ ├── Helpers/ - static helper classes
│ ├── Services/ - services implementations
│ │ ├── ActivationService.cs - app activation and initialization
│ │ ├── NavigationService.cs - navigate between pages
│ │ └──  ...
│ ├── Strings/en-us/Resources.resw - localized string resources
│ ├── Styles/ - custom style definitions
│ ├── ViewModels/ - properties and commands consumed in the views
│ ├── Views/ - UI pages
│ │ ├── ShellPage.xaml - main app page with navigation frame (only for SplitView and MenuBar)
│ │ └── ...
│ └── App.xaml - app definition and lifecycle events
├── WinAppSDKApp.Core/ - core project (.NET Standard)
│ ├── Contracts/ - class interfaces
│ ├── Helpers/ - static helper classes
│ ├── Models/ - business models
│ └── Services/ - services implementations
├── WinAppSDKApp (Package)/ - MSIX packaging project
│ ├── Strings/en-us/Resources.resw - localized string resources
│ └── Package.appxmanifest - app properties and declarations
└── README.md
```

### Design pattern
This app uses MVVM Toolkit, for more information see https://aka.ms/mvvmtoolkit.

### Project type
This app uses Navigation Pane, for more information see [navigation pane docs](https://github.com/microsoft/WindowsTemplateStudio/blob/dev/docs/UWP/projectTypes/navigationpane.md).

## Publish / Distribute

Use the [packaging project](http://aka.ms/msix) to create the app package to distribute your app and future updates. 
Right click on the packaging project and click `Publish -> Create App Packages...` to create an app package.

## Additional Documentation

- [WTS WinUI 3 docs](https://github.com/microsoft/WindowsTemplateStudio/tree/dev/docs/WinUI)
- [WinUI 3 docs](https://docs.microsoft.com/windows/apps/winui/winui3/)
- [WinUI 3 GitHub repo](https://github.com/microsoft/microsoft-ui-xaml)
- [Windows App SDK GitHub repo](https://github.com/microsoft/WindowsAppSDK)
