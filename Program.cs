using System;

interface IDataSource
{
    void WriteData(string data);
    string ReadData();
}

class FileDataSource : IDataSource
{
    private string filename;

    public FileDataSource(string filename)
    {
        this.filename = filename;
    }

    public void WriteData(string data)
    {
        Console.WriteLine($"Writing data to file '{filename}': {data}");
    }

    public string ReadData()
    {
        Console.WriteLine($"Reading data from file '{filename}'.");
        return "Mock data read from file";
    }
}

abstract class DataSourceDecorator : IDataSource
{
    protected IDataSource wrappee;

    public DataSourceDecorator(IDataSource source)
    {
        wrappee = source;
    }

    public virtual void WriteData(string data)
    {
        wrappee.WriteData(data);
    }

    public virtual string ReadData()
    {
        return wrappee.ReadData();
    }
}

class EncryptionDecorator : DataSourceDecorator
{
    public EncryptionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        Console.WriteLine("Encrypting data.");
        wrappee.WriteData($"Encrypted({data})");
    }

    public override string ReadData()
    {
        var decryptedData = wrappee.ReadData();
        Console.WriteLine("Decrypting data.");
        return decryptedData.Replace("Encrypted(", "").Replace(")", "");
    }
}

class CompressionDecorator : DataSourceDecorator
{
    public CompressionDecorator(IDataSource source) : base(source) { }

    public override void WriteData(string data)
    {
        Console.WriteLine("Compressing data.");
        wrappee.WriteData($"Compressed({data})");
    }

    public override string ReadData()
    {
        var decompressedData = wrappee.ReadData();
        Console.WriteLine("Decompressing data.");
        return decompressedData.Replace("Compressed(", "").Replace(")", "");
    }
}



class Application
{
    public static void Main()
    {
        IDataSource source = new FileDataSource("somefile.dat");
        source.WriteData("Salary records");

        source = new CompressionDecorator(source);
        source.WriteData("Salary records");

        source = new EncryptionDecorator(source);
        source.WriteData("Salary records");

     
    }
}
