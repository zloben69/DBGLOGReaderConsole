using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DBGLOGReaderConsole
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      string file = "/Users/zloben69/Projects/DBGLOGReaderConsole/DBGLOGReaderConsole/2017 08 20 0000 (Float).DAT";
      string tagfile = file.Remove(file.LastIndexOf('(')) + "(Tagname).DAT";
      Console.WriteLine("{0}\n{1}", file, tagfile);
			DBGLOGData data = new DBGLOGData();
      Console.WriteLine("Hello World!");
      Reader(file, data);
      data.print(10);
      GetNames(tagfile);
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
          foreach(var i in fs.ReadToEnd().Split(null))
          {
            if(i!= " " | i!=System.Convert.ToChar(0x20).ToString() | i!="\n")
            {
              names.Add(i);
            }
          }
          foreach(var i in names)
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

  class DBGLOGData
  {
    private List<string> Date_time = new List<string>();
    private List<string> Tagindex = new List<string>();
    private List<double> Value = new List<double>();
    private List<int> Index = new List<int>(1);

    public void add (string str)
    {
      if (str.Length > 4)
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
