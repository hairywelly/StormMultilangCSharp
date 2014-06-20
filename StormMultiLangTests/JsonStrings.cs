using System;
using System.Linq;
using System.Text;

namespace StormMultiLangTests
{
    public static class JsonStrings
    {
        public static string TrimStuffForCompare(this string source)
        {
            //remove spaces and line breaks for compare
            return source.Replace(" ", string.Empty).Replace("\r\n", string.Empty);
        }
        
        public static string[] WithoutEnd(this string[] source)
        {
            return source.Take(source.Length - 1).ToArray();
        }

        public static string ToSingleString(this string[] source)
        {
            var singleString = new StringBuilder();
            foreach (var line in source)
            {
                singleString.AppendLine(line);
            }
            return singleString.ToString(0, singleString.Length-2);
        }
        
        private static string[] AppendEndAndArrayIt(StringBuilder sb)
        {
            sb.AppendLine("end");
            return sb.ToString().Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] HandshakeMessageIn()
        {
            var sb = new StringBuilder(341);
            sb.AppendLine(@"{");
            sb.AppendLine(@"    ""conf"": ");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"        ""topology.message.timeout.secs"": 3,");
            sb.AppendLine(@"        ""topology.test.line"": 33");
            sb.AppendLine(@"");
            sb.AppendLine(@"    },");
            sb.AppendLine(@"    ""context"": ");
            sb.AppendLine(@"    {");
            sb.AppendLine(@"        ""task->component"": ");
            sb.AppendLine(@"        {");
            sb.AppendLine(@"            ""1"": ""example-spout"",");
            sb.AppendLine(@"            ""2"": ""__acker"",");
            sb.AppendLine(@"            ""3"": ""example-bolt""");
            sb.AppendLine(@"        },");
            sb.AppendLine(@"        ""taskid"": 3");
            sb.AppendLine(@"    },");
            sb.AppendLine(@"    ""pidDir"": ""C:\\temp"""); //escape "\" for json
            sb.AppendLine(@"}");
            return AppendEndAndArrayIt(sb);
        }

        public static string[] CommandNextIn()
        {
            var sb = new StringBuilder(19);
            sb.AppendLine(@"{""command"": ""next""}");
            return AppendEndAndArrayIt(sb);
        }

        public static string[] CommandAckIn()
        {
            var sb = new StringBuilder(35);
            sb.AppendLine(@"{""command"": ""ack"", ""id"": ""1231231""}");
            return AppendEndAndArrayIt(sb);
        }

        public static string[] CommandFailIn()
        {
            var sb = new StringBuilder(36);
            sb.AppendLine(@"{""command"": ""fail"", ""id"": ""1231231""}");
            return AppendEndAndArrayIt(sb);
        }

        public static string[] TupleIn()
        {
            var sb = new StringBuilder(156);
            sb.AppendLine(@"{");
            sb.AppendLine(@"    ""id"": ""-6955786537413359385"",");
            sb.AppendLine(@"    ""comp"": ""1"",");
            sb.AppendLine(@"    ""stream"": ""2"",");
            sb.AppendLine(@"    ""task"": 9,");
            sb.AppendLine(@"    ""tuple"": [""snow white and the seven dwarfs"", ""field2"", 3]");
            sb.AppendLine(@"}");
            return AppendEndAndArrayIt(sb);
        }

        public static string[] TaskIdsIn()
        {
            var sb = new StringBuilder(156);
            sb.AppendLine(@"[");
            sb.AppendLine(@"    100,");
            sb.AppendLine(@"    2000,");
            sb.AppendLine(@"    30000");
            sb.AppendLine(@"]");
            return AppendEndAndArrayIt(sb);
        }

        public static string CommandNextOut()
        {
            return @"{""command"": ""next""}";
        }

        public static string CommandAckOut()
        {
            return @"{""command"": ""ack"", ""id"": ""1231231""}";
        }

        public static string CommandFailOut()
        {
            return @"{""command"": ""fail"", ""id"": ""1231231""}";
        }

        public static string CommandSyncOut()
        {
            return @"{""command"": ""sync""}";
        }

        public static string PidOut()
        {
            return @"{""pid"": 1234}";
        }

        public static string CommandLogOut()
        {
            return @"{
                        ""command"": ""log"",
                        ""msg"": ""hello world!""
                     }";
        }

        public static string CommandErrorOut()
        {
            return @"{
                        ""command"": ""error"",
                        ""msg"": ""ERROR!!!!!!""
                     }";
        }

        public static string CommandEmitWithAnchorAll()
        {
            return @"{
                        ""command"": ""emit"",
                        ""anchors"": [""1231231"", ""-234234234""],
                        ""stream"": ""1"",
                        ""task"": 9,
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitWithAnchorNoStream()
        {
            return @"{
                        ""command"": ""emit"",
                        ""anchors"": [""1231231"", ""-234234234""],
                        ""task"": 9,
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitWithAnchorNoStreamNoTask()
        {
            return @"{
                        ""command"": ""emit"",
                        ""anchors"": [""1231231"", ""-234234234""],
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitAll()
        {
            return @"{
                        ""command"": ""emit"",
                        ""id"": ""1231231"",
                        ""stream"": ""1"",
                        ""task"": 9,
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitNoStream()
        {
            return @"{
                        ""command"": ""emit"",
                        ""id"": ""1231231"",
                        ""task"": 9,
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitNoStreamNoTask()
        {
            return @"{
                        ""command"": ""emit"",
                        ""id"": ""1231231"",
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }

        public static string CommandEmitNoStreamNoTaskNoId()
        {
            return @"{
                        ""command"": ""emit"",
                        ""tuple"": [""field1"", 2, 3]
                    }";
        }
    }
}