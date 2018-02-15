using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace DBGLOGReaderConsole
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      string file = @"C:\Users\newworker\Documents\py\!sandbox\DBGLOGReader\log\2017 08 20 0000 (Float).DAT";
      //string tagfile = file.Remove(file.LastIndexOf('(')) + "(Tagname).DAT";
			DBGLOGData data = new DBGLOGData();
      Console.WriteLine("Hello World!");
      new Reader().read(file, data);
      data.print(10);
      //GetNames(tagfile);
      Console.ReadKey();
    }
    static void Reader(string file, DBGLOGData data)
    {
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
          data.add(System.Text.Encoding.Default.GetString(fs.ReadBytes(19)));
					fs.ReadBytes(3);
          data.add(System.Text.Encoding.Default.GetString(fs.ReadBytes(2)));
          data.add(BitConverter.ToDouble(fs.ReadBytes(8), 0));
          fs.ReadBytes(2);
          data.add(BitConverter.ToInt16(fs.ReadBytes(4), 0));
        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
    }
    static void GetNames (string fi)
    {
      try
      {
        StreamReader fs = new StreamReader(fi);
        List<string> names = new List<string>();
        while (true)
        {
         foreach (var i in fs.ReadToEnd().Split(null))
          {
            if (Regex.IsMatch(i, @"\w") & i.Length < 40 & i.Length > 3)
            {
              names.Add(i);
            }
          }
          foreach (var i in names)
          {
            Console.WriteLine(i);
          }
          break;
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }
  }

  class Reader
  {
    public void read(string file, DBGLOGData data)
    {
      List<string> name = get_names(file);
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
          data.add(System.Text.Encoding.Default.GetString(fs.ReadBytes(19)));
          fs.ReadBytes(3);
          data.add(name[Convert.ToInt16(System.Text.Encoding.Default.GetString(fs.ReadBytes(2)))]);
          //data.add(System.Text.Encoding.Default.GetString(fs.ReadBytes(2)));
          data.add(BitConverter.ToDouble(fs.ReadBytes(8), 0));
          fs.ReadBytes(2);
          data.add(BitConverter.ToInt16(fs.ReadBytes(4), 0));
        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
    }
    public List<string> get_names(string fi)
    {
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
        return names;
      }
      catch (Exception ex)
      {
        for (int i = 0; i != 1000; i++)
        {
          names.Add(i.ToString());
        }
        Console.WriteLine(ex);
        return names;
      }
    }
  }
  class DBGLOGData
  {
    private List<string> Date_time = new List<string>();
    private List<string> Tagindex = new List<string>();
    private List<double> Value = new List<double>();
    private List<int> Index = new List<int>(1);

    public void add (string str)
    {
      if (Regex.IsMatch(str, @"[0-9]*:[0-9]*"))
      {
        Date_time.Add(str);
      }
      else Tagindex.Add(str);
    }
    public void add (double val)
    {
      Value.Add(val);;
    }
    public void add (int val)
    {
      Index.Add(val);
    }
    public void print(long len)
    {
      for(var i = 0; i != len; i++)
      {
				Console.WriteLine("{0}, {1}, {2:.###}, {3}", Date_time[i], Tagindex[i], Value[i], Index[i]);
      }
    }
  }
}
