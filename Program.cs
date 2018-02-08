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
      Console.WriteLine("Hello World!");
      List<byte[]> temp = Reader("/Users/zloben69/Projects/DBGLOGReaderConsole/DBGLOGReaderConsole/2017 08 20 0000 (Float).DAT");
      foreach (var item in temp)
      {
        string str = "";
        for (var x = 0; x != item.Length; x++)
        {
          char[] ca = new char[38];
        }
        Console.WriteLine(str);
      }
    }

    static List<byte[]> Reader(string file)
    {
      Dictionary<string, string> data = new Dictionary<string, string>();
      List<byte[]> list = new List<byte[]>();
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
          list.Add(fs.ReadBytes(38));
          //Console.WriteLine(fs.ReadBytes(2).ToString());
          break;
        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
      return list;
    }
  }
}
