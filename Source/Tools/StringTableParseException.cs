using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mah_boi.Tools
{
    public class StringTableParseException : Exception
    {
        public enum MessageType : int
        {
            Info    = 0, // для подсказок пользователю. фича на будушее
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

        public void AddMessage(string msgText, MessageType msgType)
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
        }

        public void DeleteMessage(string msgText, MessageType msgType)
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
        }

        public void ThrowExceptions()
            => throw new StringTableParseException(ToString());

        public string GetExceptions()
            => ToString();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            if (InfoList.Count > 0)
                foreach (var str in InfoList)
                {
                    i++;
                    sb.AppendLine($"Информация №{i}: " + Environment.NewLine + "\t" + str);
                    sb.AppendLine();
                }

            i = 0;
            if (WarningList.Count > 0)
                foreach (var str in WarningList)
                {
                    i++;
                    sb.AppendLine($"Предупреждение №{i}: " + Environment.NewLine + "\t" + str);
                    sb.AppendLine();
                }

            i = 0;
            if (ErrorList.Count > 0)
                foreach (var str in ErrorList)
                {
                    i++;
                    sb.AppendLine($"Ошибка №{i}: " + Environment.NewLine + "\t" + str);
                    sb.AppendLine();
                }

            return sb.ToString();
        }
    }
}
