﻿namespace Common.Commons;

public static class IOHelper
{
    public static async Task<byte[]> ToArrrayAsync(this Stream stream)
    {
        using MemoryStream ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        ms.Position = 0;
        return ms.ToArray();
    }
    public static byte[] ToArray(this Stream stream)
    {
        using MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        return ms.ToArray();
    }
    public static void CreateDir(FileInfo file)
    {
        file.Directory.Create();
    }
    
}