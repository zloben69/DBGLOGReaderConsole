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
          date = System.Text.Encoding.Default.GetString(fs.ReadBytes(16));
					fs.ReadBytes(3);
          tagindex = System.Text.Encoding.Default.GetString(fs.ReadBytes(2));
          value = BitConverter.ToDouble(fs.ReadBytes(8), 0);
          fs.ReadBytes(2);
          index = BitConverter.ToInt32(fs.ReadBytes(6), 0);
          Console.WriteLine("{0}, {1}, {2}, {3}, {4}", 
                            date,
                            tagindex,
                            value, 
                            index);
          break;
        }
        else if (blyat == 0x1A)
        {
          break;
        }
      }
    }
  }
}
