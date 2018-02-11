using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DBGLOGReaderConsole
{
  class MainClass
  {
		DBGLOGData data = new DBGLOGData();
    public static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      Reader("/Users/zloben69/Projects/DBGLOGReaderConsole/DBGLOGReaderConsole/2017 08 20 0000 (Float).DAT");
    }
    static void Reader(string file)
    {
      System.IO.BinaryReader fs = new BinaryReader(File.Open(file, FileMode.Open));
      bool flag = false;
      while (true)
      {
        string date;
        string tagindex;
        double value;
        long index;

        byte blyat = fs.ReadByte();
        if (blyat == 0x0D)
        {
          flag = true;
        }
        else if (blyat == 0x20 & flag == true)
        {
          date = System.Text.Encoding.Default.GetString(fs.ReadBytes(19));
					fs.ReadBytes(3);
          tagindex = System.Text.Encoding.Default.GetString(fs.ReadBytes(2));
          value = BitConverter.ToDouble(fs.ReadBytes(8), 0);
          fs.ReadBytes(2);
          index = BitConverter.ToInt16(fs.ReadBytes(4), 0);

        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
    }
  }
  class DBGLOGData
  {
    private List<string> Date_time = new List<string>();
    private List<string> Tagindex = new List<string>();
    private List<double> Value = new List<double>();
    private List<int> Index = new List<int>();

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
  }
}
