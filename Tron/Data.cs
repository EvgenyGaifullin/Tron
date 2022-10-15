
using System;
using System.Collections.Generic;
using System.IO;

public class ScoreWriter
{
    public const string PATH = "C:\\ScoreFile.TXT";
    public const string ORANGE = "ORANGE";
    public const string GREEN = "GREEN";

    // Stores the result of 1 winner, format: [BLUE, 4:1]
    private string winer;
    public string Winer
    {
        get => winer;
        set => winer = value;
    }


    // Storse the list of all results
    private List<string> listScore;
    public List<string> ListScore
    {
        get => listScore;
    }

    public ScoreWriter()
    {
        this.winer = null;
        this.listScore = new List<string>();
    }

    public void writeWiner(string path, string winer)
    {
        try
        {
            StreamWriter SW = new StreamWriter(path, true, System.Text.Encoding.Default);
            SW.WriteLine(winer);
            SW.Close();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex);
        }
    }

    public List<string> readAll(string path, string winer)
    {
        try
        {
            using (StreamReader SR = File.OpenText(path))
            {
                string buffer;
                do
                {
                    buffer = SR.ReadLine();
                    this.ListScore.Add(buffer);
                } while (buffer != null);
            }
            return listScore;
        }
        catch (OutOfMemoryException e)
        {
            Console.WriteLine(e.Message);
            return listScore;
        }
        catch (System.IO.IOException e)
        {
            Console.WriteLine(e.Message);
            return listScore;
        }
    }
}