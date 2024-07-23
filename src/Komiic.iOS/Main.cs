using System;
using Komiic.Extensions;
using NLog;
using UIKit;

namespace Komiic.iOS;

public static class Application
{
    static Application()
    {
        $"{nameof(Komiic)}.{nameof(iOS)}".ConfigNLog();
        Logger = LogManager.GetCurrentClassLogger();
    }

    private static readonly Logger Logger;

    // This is the main entry point of the application.
    static void Main(string[] args)
    {
        try
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
        catch (Exception e)
        {
            Logger.Fatal(e);
        }
    }
}