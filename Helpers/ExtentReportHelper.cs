using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using System;
using System.IO;

public static class ExtentReportHelper
{
    private static ExtentReports? _extent;
    private static ExtentTest? _currentTest;
    private static string? _reportPath;

    public static void InitReport()
    {
        try
        {
            string reportDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExtentReports");
            _reportPath = Path.Combine(reportDirectory, "TestReport.html");

            if (!Directory.Exists(reportDirectory))
            {
                Console.WriteLine($"📂 Creating Directory: {reportDirectory}");
                Directory.CreateDirectory(reportDirectory);
            }

            Console.WriteLine($"📝 Initializing Extent Report at: {_reportPath}");
            var htmlReporter = new ExtentHtmlReporter(_reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error Initializing Report: {ex.Message}");
        }
    }

    public static void CreateTest(string testName)
    {
        if (_extent == null)
        {
            Console.WriteLine("⚠️ Extent Report is not initialized. Call InitReport() first.");
            return;
        }

        _currentTest = _extent.CreateTest(testName);
        Console.WriteLine($"✅ Test Created: {testName}");
    }

    public static void LogTestSuccess(string testName)
    {
        if (_currentTest == null)
        {
            Console.WriteLine($"⚠️ No active test found. Call CreateTest() first.");
            return;
        }
        _currentTest.Pass($"Test '{testName}' Passed.");
        Console.WriteLine($"✅ Test '{testName}' Passed.");
    }

    public static void LogTestFailure(string testName, string screenshotPath)
    {
        if (_currentTest == null)
        {
            Console.WriteLine($"⚠️ No active test found. Call CreateTest() first.");
            return;
        }

        if (File.Exists(screenshotPath))
        {
            _currentTest.Fail($"Test '{testName}' Failed.")
                        .AddScreenCaptureFromPath(screenshotPath);
            Console.WriteLine($"❌ Test '{testName}' Failed. Screenshot: {screenshotPath}");
        }
        else
        {
            _currentTest.Fail($"Test '{testName}' Failed. ❗ Screenshot not found.");
            Console.WriteLine($"❌ Test '{testName}' Failed. ❗ Screenshot not found at {screenshotPath}");
        }
    }

    public static void LogTestError(string errorMessage)
    {
        if (_currentTest == null)
        {
            Console.WriteLine($"⚠️ No active test found. Call CreateTest() first.");
            return;
        }

        _currentTest.Log(Status.Error, errorMessage);
        Console.WriteLine($"⚠️ Error: {errorMessage}");
    }

    public static void LogStep(string message)
    {
        if (_currentTest == null)
        {
            Console.WriteLine($"⚠️ No active test found. Call CreateTest() first.");
            return;
        }

        _currentTest.Info(message);
        Console.WriteLine($"🔹 Step: {message}");
    }

    public static void FlushReport()
    {
        if (_extent == null)
        {
            Console.WriteLine("⚠️ Extent Report is not initialized. Call InitReport() first.");
            return;
        }

        _extent.Flush();
        Console.WriteLine("📄 Extent Report Flushed. ✅");
    }

    public static void DisposeReport()
    {
        if (_extent != null)
        {
            _extent.Flush();  // Just flush the report, Dispose() is not needed
            _extent = null;   // Release the reference
            Console.WriteLine("🗑️ Extent Report Disposed.");
        }
    }
}
