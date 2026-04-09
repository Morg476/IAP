using mk_5202_i;

namespace starter_code;

// Handles initialisation for the MK5202 assessment checker
public static class Initialiser
{
    // Starts the checker when the application runs
    public static void Start()
    {
        try
        {
            // Executes the MK5202 startup process
            smlco.slo(GetDir());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mk5202 failed: {ex.Message}");
        }
    }

    // Stops the checker and writes results to a log file
    public static void Stop()
    {
        try
        {
            // Reads and decodes the MK5202 output file
            var enc = smlco.rfb(Path.Combine(GetDir(), "mk_5202_d"));
            if (enc.Length > 0)
                WriteLog(smlco.hro(smlco.dro(enc)));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Mk5202 failed: {ex.Message}");
        }
    }

    // Returns the full path to a specified file in the project directory
    public static string GetDir(string fileName = "AyventDb.db")
    {
        return Path.Combine(GetDir(), fileName);
    }

    // Retrieves the root directory of the project
    public static string GetDir()
    {
        var dir = Directory.GetParent(AppContext.BaseDirectory)?
            .Parent?.Parent?.Parent?.FullName;

        dir = dir ?? Directory.GetCurrentDirectory();
        return dir;
    }

    // Writes decoded checker results to a log file
    private static void WriteLog(string text)
    {
        File.AppendAllText(
            Path.Combine(GetDir(), "runlog.txt"),
            text + Environment.NewLine
        );
    }
}