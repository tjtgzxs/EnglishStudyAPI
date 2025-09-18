﻿namespace Common.Commons;

public static class StringExtensions
{  
    public static bool EqualsIgnoreCase(this string s1, string s2)
    {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }
    public static string Cut(this string s1, int maxLen)
    {
        if (s1 == null)
        {
            return string.Empty;
        }
        int len = s1.Length <= maxLen ? s1.Length : maxLen;//不能超过字符串的最大大小
        return s1[0..len];
    }
    
}