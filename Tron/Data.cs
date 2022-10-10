
//public class ScoreWriter
//{

//    // Stores the result of 1 winner, format: [BLUE, 4:1]
//    private string[] winArr;
//    public string[] WinArr
//    {
//        get => winArr;
//        set => winArr = value;
//    }


//    // Storse the list of all results
//    private List<string> listScore;
//    public List<string> ListScore
//    {
//        get => listScore;
//    }

//    public ScoreWriter()
//    {
//        this.winArr = new string[2];
//        this.listScore = new List<string>();
//    }

//    public void writeWiner(string path)
//    {
//        try
//        {
//            StreamWriter SW = new StreamWriter(path, true, System.Text.Encoding.Default);
//            SW.WriteLine(this.winArr[0] + " " + this.winArr[1]);
//            SW.Close();
//        }
//        catch (Exception ex)
//        {
//            System.Console.WriteLine(ex);
//        }
//    }

//    public List<string> readAll(string path)
//    {
//        try
//        {
//            using (StreamReader SR = File.OpenText(path))
//            {
//                //string buffer;
//                do
//                {
//                    buffer = SR.ReadLine();
//                    this.ListScore.Add(buffer);
//                } while (buffer != null);
//            }
//            return listScore;
//        }
//        catch (OutOfMemoryException e)
//        {
//            Console.WriteLine(e.Message);
//            return listScore;
//        }
//        catch (System.IO.IOException e)
//        {
//            Console.WriteLine(e.Message);
//            return listScore;
//        }
//    }
//}