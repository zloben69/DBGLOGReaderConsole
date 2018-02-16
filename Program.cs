using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
namespace DBGLOGReaderConsole
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      var start = DateTime.Now;
      string input_mac = @"/Users/zloben69/Documents/Programming/Py/log/2017 08 20 0000 (Float).DAT";
      string input_win = @"C:\Users\newworker\Documents\py\!sandbox\DBGLOGReader\log\2017 08 20 0000 (Float).DAT";
      string output_win = @"C:\Users\newworker\Documents\py\!sandbox\DBGLOGReader";
      string output_mac = "/Users/zloben69/Documents/Programming/Py";

      DBGLOGData data = new DBGLOGData();
      Console.WriteLine("Start");
      data.Read(input_win);
      data.PrintToFile(output_win);
      data.Print(20);
      Console.WriteLine("Total execution time: " + (DateTime.Now - start).TotalSeconds);
      Console.ReadKey();
    }
  }
  class DBGLOGData
  {
    private List<string> Date_time = new List<string>();
    private List<string> Tagindex = new List<string>();
    private List<double> Value = new List<double>();
    private List<int> Index = new List<int>(1);
    string datalogpath = "";
    void Add (string str)
    {
      if (Regex.IsMatch(str, @"[0-9]*:[0-9]*"))
      {
        Date_time.Add(str);
      }
      else Tagindex.Add(str);
    }
    void Add (double val)
    {
      Value.Add(val);;
    }
    void Add (int val)
    {
      Index.Add(val);
    }
    List<string> Get_names(string fi)
    {
      var start = DateTime.Now;
      List<string> names = new List<string>();
      try
      {
        fi = fi.Remove(fi.LastIndexOf('(')) + "(Tagname).DAT";
        StreamReader fs = new StreamReader(fi);
        while (true)
        {
          foreach (var i in fs.ReadToEnd().Split(null))
          {
            if (Regex.IsMatch(i, @"\w") & i.Length < 40 & i.Length > 3)
            {
              names.Add(i);
            }
          }
          break;
        }
        Console.WriteLine("Get_names execution time: " + (DateTime.Now - start).TotalSeconds);
        return names;
      }
      catch (Exception ex)
      {
        for (int i = 0; i != 1000; i++)
        {
          names.Add(i.ToString());
        }
        Console.WriteLine(ex);
				Console.WriteLine("Get_names time execution: " + (DateTime.Now - start).TotalSeconds);
        return names;
      }
    }
    public void Read(string file)
    {
      datalogpath = file;
      string temp;
      List<string> name = Get_names(file);
      var start = DateTime.Now;
      System.IO.BinaryReader fs = new BinaryReader(File.Open(file, FileMode.Open));
      bool flag = false;
      while (true)
      {
        byte blyat = fs.ReadByte();
        if (blyat == 0x0D)
        {
          flag = true;
        }
        else if (blyat == 0x20 & flag == true)
        {
          temp = System.Text.Encoding.Default.GetString(fs.ReadBytes(4)) + "-";
          temp += System.Text.Encoding.Default.GetString(fs.ReadBytes(2)) + "-";
          temp += System.Text.Encoding.Default.GetString(fs.ReadBytes(2)) + " ";
          temp += System.Text.Encoding.Default.GetString(fs.ReadBytes(8)) + ":";
          temp += System.Text.Encoding.Default.GetString(fs.ReadBytes(3)) + ":";
          Add(temp);
          //this.Add(System.Text.Encoding.Default.GetString(fs.ReadBytes(19)));
          fs.ReadBytes(3);
          Add(name[Convert.ToInt16(System.Text.Encoding.Default.GetString(fs.ReadBytes(2)))]);
          //this.Add(System.Text.Encoding.Default.GetString(fs.ReadBytes(2))); //No tagmanes, only tag number
          Add(BitConverter.ToDouble(fs.ReadBytes(8), 0));
          fs.ReadBytes(2);
          Add(BitConverter.ToInt16(fs.ReadBytes(4), 0));
        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
      Console.WriteLine("Read execution time: " + (DateTime.Now - start).TotalSeconds);
    }
    public void Print(long len)
    {
      Console.WriteLine();
      for (var i = 0; i != len; i++)
      {
        Console.WriteLine("{0}, {1}, {2:0.000}, {3}", Date_time[i], Tagindex[i], Value[i], Index[i]);
      }
      Console.WriteLine();
    }
    public void PrintToFile(string path)
    {
      var start = DateTime.Now;
      try
      {
        if (Environment.OSVersion.ToString().Contains("NT"))
        {
          path += "\\" + datalogpath.Remove(datalogpath.LastIndexOf('(')).Substring(datalogpath.LastIndexOf('\\') + 1) + ".csv";
        }
        else { path += "/" + datalogpath.Remove(datalogpath.LastIndexOf('(')).Substring(datalogpath.LastIndexOf('/') + 1) + ".csv"; }
      }
      catch (Exception)
      {
        path += "/Temp.csv";
      }
      Console.WriteLine(path);
      try
      {
        if (File.Exists(path))
        {
          File.Delete(path);
        }
        using (FileStream fs = File.Create(path))
        {
          for (int i = 0; i != Index.Count; i++)
          {
            Byte[] info = new System.Text.UTF8Encoding(true).GetBytes(
              String.Format("{0}, {1}, {2:0.000}, {3}\n", Date_time[i], Tagindex[i], Value[i], Index[i]));
            fs.Write(info, 0, info.Length);       
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
      Console.WriteLine("PrintToFile execution time: " + (DateTime.Now - start).TotalSeconds);
    }
  }
}