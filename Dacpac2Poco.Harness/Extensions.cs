using System.Diagnostics;

public static class Extensions
{
    public static void OpenInVsCode(this string path)
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var vscode = Path.Combine(userProfile, @"AppData\Local\Programs\Microsoft VS Code\Code.exe");
        vscode = Environment.ExpandEnvironmentVariables(vscode);
        System.Diagnostics.Process.Start(vscode, path);
    }

    public static void OpenInNotepad(this string path)
    {
        Process.Start("notepad.exe", path);
    }
}