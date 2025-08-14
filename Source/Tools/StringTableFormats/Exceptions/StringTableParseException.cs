using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools.StringTableFormats.Exceptions;

public class StringTableParseException : Exception
{
    public enum MessageType
    {
        Info    = 0,
        Warning = 1,
        Error   = 2
    }

    private List<string> InfoList;
    private List<string> WarningList;
    private List<string> ErrorList;

    public StringTableParseException()
    {
        InfoList    = new List<string>();
        WarningList = new List<string>();
        ErrorList   = new List<string>();
    }
    public StringTableParseException(string message) : base(message)
    {
    }

    public StringTableParseException AddMessage(string msgText, MessageType msgType)
    {
        switch (msgType)
        {
            case MessageType.Info:
                break;
            case MessageType.Warning:
                WarningList.Add(msgText);
                break;
            case MessageType.Error:
                ErrorList.Add(msgText);
                break;
            default:
                break;
        }

        return this;
    }

    public StringTableParseException DeleteMessage(string msgText, MessageType msgType)
    {
        switch (msgType)
        {
            case MessageType.Info:
                break;
            case MessageType.Warning:
                WarningList.Remove(msgText);
                break;
            case MessageType.Error:
                ErrorList.Remove(msgText);
                break;
            default:
                break;
        }

        return this;
    }

    public void ThrowExceptions() => throw new StringTableParseException(ToString());

    public string GetExceptions() => ToString();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        int i = 0;
        foreach (var str in InfoList)
        {
            i++;
            sb.AppendLine($"Information №{i}: " + Environment.NewLine + "\t" + str);
            sb.AppendLine();
        }

        i = 0;
        foreach (var str in WarningList)
        {
            i++;
            sb.AppendLine($"Warning №{i}: " + Environment.NewLine + "\t" + str);
            sb.AppendLine();
        }

        i = 0;
        foreach (var str in ErrorList)
        {
            i++;
            sb.AppendLine($"Error №{i}: " + Environment.NewLine + "\t" + str);
            sb.AppendLine();
        }

        return sb.ToString();
    }
}
