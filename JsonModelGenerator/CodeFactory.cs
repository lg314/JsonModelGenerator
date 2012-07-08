using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JsonModelGenerator
{
    class CodeFactory
    {
        public static void Generate(CodeParameter parm, Type type, JsonModelType modelType)
        {
            string name = type.Name;
            var properties = type.GetProperties();
            List<string> relation = new List<string>();
            StreamWriter msrw = File.CreateText(Path.Combine(parm.Folder, name + ".m"));
            msrw.WriteLine("#import \"" + name + ".h" + "\"");
            msrw.WriteLine("");
            msrw.WriteLine("@implementation " + name);
            WritePropertyM(msrw, properties);

            if (modelType == JsonModelType.Client)
                WriteClientCode(type, msrw, parm.ClientFunction);
            else if (modelType == JsonModelType.Server)
                WriteServerCode(type, relation, msrw, parm.ServerFunction);
            else if (modelType == JsonModelType.ServerClient)
            {
                WriteServerCode(type, relation, msrw, parm.ServerFunction);
                WriteClientCode(type, msrw, parm.ClientFunction);
            }
            msrw.WriteLine("");
            msrw.WriteLine("@end");
            msrw.Close();
            PropertyRelation(relation, properties);
            StreamWriter hsrw = File.CreateText(Path.Combine(parm.Folder, name + ".h"));

            if (modelType == JsonModelType.Client)
            {
                hsrw.WriteLine("#import \"" + parm.ClientProtocal + ".h\"");
            }
            else if (modelType == JsonModelType.Server)
            {
                hsrw.WriteLine("#import \"" + parm.ServerProtocal + ".h\"");
            }
            else if (modelType == JsonModelType.Client)
            {
                hsrw.WriteLine("#import \"" + parm.ClientProtocal + ".h\"");
                hsrw.WriteLine("#import \"" + parm.ServerProtocal + ".h\"");
            }

            foreach (string item in relation.Distinct().Where(x => x != type.Name))
            {
                hsrw.WriteLine("#import \"" + item + ".h\"");
            }
            hsrw.WriteLine("");
            string protocal = "";
            if (modelType == JsonModelType.Client)
            {
                protocal = "<" + parm.ClientProtocal + ">";
            }
            else if (modelType == JsonModelType.Server)
            {
                protocal = "<" + parm.ServerProtocal + ">";
            }
            else if (modelType == JsonModelType.Client)
            {
                protocal = "<" + parm.ClientProtocal + "," + parm.ServerProtocal + ">";
            }

            hsrw.WriteLine("@interface " + name + " : NSObject" + protocal);
            WritePropertyH(hsrw, properties);
            hsrw.WriteLine("");
            hsrw.WriteLine("@end");
            hsrw.Close();
        }

        private static void WriteClientCode(Type type, StreamWriter msrw, string functionName)
        {
            msrw.WriteLine("");
            msrw.WriteLine("-(NSData *)" + functionName + "{");
            WriteContent(msrw, type, "self", "content", 1);
            msrw.WriteLine(GetTab(1) + "NSData *data=[NSJSONSerialization dataWithJSONObject:content options:NSJSONWritingPrettyPrinted error:nil];");
            msrw.WriteLine(GetTab(1) + "return data;");
            msrw.WriteLine("}");

        }

        private static void WriteServerCode(Type t, List<string> relation, StreamWriter msrw, string functionName)
        {
            msrw.WriteLine("");
            msrw.WriteLine("+(id)" + functionName + ":(NSData *)data{");
            if (IsArray(t))
            {
                msrw.WriteLine(GetTab(1) + "NSArray *dict=[NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableLeaves error:nil];");
                ReadContent(relation, msrw, t, "dict", "inst", 1);
            }
            else
            {
                msrw.WriteLine(GetTab(1) + "NSDictionary *dict=[NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableLeaves error:nil];");
                ReadContent(relation, msrw, t, "dict", "inst", 1);
            }
            msrw.WriteLine(GetTab(1) + "return inst;");
            msrw.WriteLine("}");
        }

        private static bool IsArray(Type t)
        {
            if (t == typeof(byte[]))
                return false;
            if (t.IsArray)
                return true;
            if (t.IsGenericType)
                if (t.GetGenericTypeDefinition() == typeof(List<>))
                    return true;
            return false;
        }

        private static string GetTab(int tabCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
            {
                sb.Append("    ");
            }
            return sb.ToString();
        }

        private static void PropertyRelation(List<string> relation, System.Reflection.PropertyInfo[] properties)
        {
            foreach (var pro in properties)
            {
                if (pro.PropertyType == typeof(string))
                {
                }
                else if (pro.PropertyType == typeof(decimal))
                {
                }
                else if (pro.PropertyType == typeof(bool))
                {
                }
                else if (pro.PropertyType == typeof(char))
                {
                }
                else if (pro.PropertyType == typeof(byte))
                {
                }
                else if (pro.PropertyType == typeof(sbyte))
                {
                }
                else if (pro.PropertyType == typeof(short))
                {
                }
                else if (pro.PropertyType == typeof(int))
                {
                }
                else if (pro.PropertyType == typeof(long))
                {
                }
                else if (pro.PropertyType == typeof(ushort))
                {
                }
                else if (pro.PropertyType == typeof(uint))
                {
                }
                else if (pro.PropertyType == typeof(ulong))
                {
                }
                else if (pro.PropertyType == typeof(float))
                {
                }
                else if (pro.PropertyType == typeof(double))
                {
                }
                else if (pro.PropertyType == typeof(DateTime))
                {
                }
                else if (pro.PropertyType == typeof(byte[]))
                {
                }
                else if (IsArray(pro.PropertyType))
                {
                }
                else
                {
                    relation.Add(pro.PropertyType.Name);
                }
            }
        }

        private static void WritePropertyH(StreamWriter srwH, System.Reflection.PropertyInfo[] properties)
        {
            foreach (var pro in properties)
            {
                srwH.WriteLine("");
                if (pro.PropertyType == typeof(string))
                {
                    srwH.WriteLine("@property NSString *" + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(decimal))
                {
                    srwH.WriteLine("@property NSDecimail " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(bool))
                {
                    srwH.WriteLine("@property BOOL " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(char))
                {
                    srwH.WriteLine("@property char " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(byte))
                {
                    srwH.WriteLine("@property unsigned char " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(sbyte))
                {
                    srwH.WriteLine("@property char " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(short))
                {
                    srwH.WriteLine("@property short " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(int))
                {
                    srwH.WriteLine("@property int " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(long))
                {
                    srwH.WriteLine("@property long " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(ushort))
                {
                    srwH.WriteLine("@property unsigned short " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(uint))
                {
                    srwH.WriteLine("@property unsigned int " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(ulong))
                {
                    srwH.WriteLine("@property unsigned long " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(float))
                {
                    srwH.WriteLine("@property float " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(double))
                {
                    srwH.WriteLine("@property double " + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(DateTime))
                {
                    srwH.WriteLine("@property NSDate *" + pro.Name + ";");
                }
                else if (pro.PropertyType == typeof(byte[]))
                {
                    srwH.WriteLine("@property NSData *" + pro.Name + ";");
                }
                else if (IsArray(pro.PropertyType))
                {
                    srwH.WriteLine("@property NSArray *" + pro.Name + ";");
                }
                else
                {
                    srwH.WriteLine("@property " + pro.PropertyType.Name + " *" + pro.Name + ";");
                }
            }
        }

        private static void WritePropertyM(StreamWriter srwM, System.Reflection.PropertyInfo[] properties)
        {
            foreach (var pro in properties)
            {
                srwM.WriteLine("");
                srwM.WriteLine("@synthesize " + pro.Name + ";");
            }
        }

        private static void WriteContent(StreamWriter srw, Type t, string content, string instance, int tabCount)
        {
            bool array = IsArray(t);
            if (array)
            {
                srw.WriteLine(GetTab(tabCount) + "NSMutableArray *" + instance + "=[[NSMutableArray alloc] init];");
                string i = "i" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<[" + content + " count];" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                Type elementType = t.GetElementType();
                if (elementType == null)
                {
                    elementType = t.GetGenericArguments()[0];
                }
                string contentArray = "content" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount + 1) + elementType.Name + " *" + contentArray + "=[" + content + " objectAtIndex:" + i + "];");
                WriteArray(srw, elementType, contentArray, instance, tabCount + 1);
                srw.WriteLine(GetTab(tabCount) + "}");
            }
            else
            {
                srw.WriteLine(GetTab(tabCount) + "NSMutableDictionary *" + instance + "=[[NSMutableDictionary alloc] init];");
                var pros = t.GetProperties();
                foreach (var pro in pros)
                {
                    WriteProperty(srw, pro.PropertyType, pro.Name, content, instance, tabCount);
                }
            }
        }

        private static void WriteArray(StreamWriter srw, Type t, string content, string instance, int tabCount)
        {
            if (t == typeof(string))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(decimal))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(bool))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithBool:" + content + "]];");
            }
            else if (t == typeof(char))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithChar:" + content + "]];");
            }
            else if (t == typeof(byte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithUnsignedChar:" + content + "]];");
            }
            else if (t == typeof(sbyte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithChar:" + content + "]];");
            }
            else if (t == typeof(short))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithShort:" + content + "]];");
            }
            else if (t == typeof(int))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithInt:" + content + "]];");
            }
            else if (t == typeof(long))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithLong:" + content + "]];");
            }
            else if (t == typeof(ushort))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithUnsignedShort:" + content + "]];");
            }
            else if (t == typeof(uint))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithUnsignedInt:" + content + "]];");
            }
            else if (t == typeof(ulong))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithUnsignedLong:" + content + "]];");
            }
            else if (t == typeof(float))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithFloat:" + content + "]];");
            }
            else if (t == typeof(double))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:[NSNumber numberWithDouble:" + content + "]];");
            }
            else if (t == typeof(DateTime))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(byte[]))
            {
                string data = "data" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSData *" + data + "=" + content + ";");
                string length = "length" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "int length=[" + data + " count];");
                string dataBytes = "dataBytes" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "unsigned char *" + dataBytes + "=(unsigned char *)[" + data + " bytes];");
                string contentNext = "content" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSMutableArray *" + contentNext + "=[[NSMutableArray alloc] init];");
                string i = "i" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<length;" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                srw.WriteLine(GetTab(tabCount + 1) + "[" + contentNext + " addObject:[NSNumber numberWithUnsignedChar:" + dataBytes + "[" + i + "]]];");
                srw.WriteLine(GetTab(tabCount) + "}");
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + contentNext + "];");
            }
            else
            {
                string instanceNext = "instance" + rnd.Next().ToString();
                WriteContent(srw, t, content, instanceNext, tabCount);
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + instanceNext + "];");
            }
        }

        private static void WriteProperty(StreamWriter srw, Type t, string property, string content, string instance, int tabCount)
        {
            if (t == typeof(string))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:" + content + "." + property + " forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(decimal))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:" + content + "." + property + " forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(bool))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithBool:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(char))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithChar:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(byte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithUnsignedChar:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(sbyte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithChar:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(short))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithShort:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(int))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithInt:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(long))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithLong:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(ushort))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithUnsignedShort:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(uint))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithUnsignedInt:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(ulong))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithUnsignedLong:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(float))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithFloat:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(double))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:[NSNumber numberWithDouble:" + content + "." + property + "] forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(DateTime))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:" + content + "." + property + " forKey:@\"" + property + "\"];");
            }
            else if (t == typeof(byte[]))
            {
                string data = "data" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSData *" + data + "=" + content + "." + property + ";");
                string length = "length" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "int length=[" + data + " count];");
                string dataBytes = "dataBytes" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "unsigned char *" + dataBytes + "=(unsigned char *)[" + data + " bytes];");
                string contentNext = "content" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSMutableArray *" + contentNext + "=[[NSMutableArray alloc] init];");
                string i = "i" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<length;" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                srw.WriteLine(GetTab(tabCount + 1) + "[" + contentNext + " addObject:[NSNumber numberWithUnsignedChar:" + dataBytes + "[" + i + "]]];");
                srw.WriteLine(GetTab(tabCount) + "}");
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:" + contentNext + " forKey:@\"" + property + "\"];");
            }
            else
            {
                string instanceNext = "instance" + rnd.Next().ToString();
                WriteContent(srw, t, property, instanceNext, tabCount);
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " setValue:" + instanceNext + " forKey:@\"" + property + "\"];");
            }
        }

        private static void ReadContent(List<string> relation, StreamWriter srw, Type t, string content, string instance, int tabCount)
        {
            bool array = IsArray(t);
            if (array)
            {
                srw.WriteLine(GetTab(tabCount) + "NSMutableArray *" + instance + "=[[NSMutableArray alloc] init];");
                string i = "i" + rnd.Next();
                string arrayContent = "content" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSArray *" + arrayContent + "=[" + content + " array];");
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<[" + arrayContent + " count];" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                Type elementType = t.GetElementType();
                if (elementType == null)
                {
                    elementType = t.GetGenericArguments()[0];
                }
                string dictContent = "content" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount + 1) + "NSDictionary *" + dictContent + "=[" + arrayContent + " objectAtIndex:" + i + "];");
                ReadArray(relation, srw, elementType, dictContent, instance, tabCount + 1);
                srw.WriteLine(GetTab(tabCount) + "}");
            }
            else
            {
                srw.WriteLine(GetTab(tabCount) + t.Name + " *" + instance + "=[[" + t.Name + " alloc] init];");
                relation.Add(t.Name);
                var pros = t.GetProperties();
                foreach (var pro in pros)
                {
                    string contentNext = "content" + rnd.Next().ToString();
                    srw.WriteLine(GetTab(tabCount) + "id " + contentNext + "=[" + content + " objectForKey:@\"" + pro.Name + "\"];");
                    ReadProperty(relation, srw, pro.PropertyType, contentNext, pro.Name, instance, tabCount);
                }
            }
        }

        private static void ReadArray(List<string> relation, StreamWriter srw, Type t, string content, string instance, int tabCount)
        {
            if (t == typeof(string))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(decimal))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(bool))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(char))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(byte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(sbyte))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(short))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(int))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(long))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(ushort))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(uint))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(ulong))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(float))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(double))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(DateTime))
            {
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + content + "];");
            }
            else if (t == typeof(byte[]))
            {
                string array = "array" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSArray *" + array + "=" + content + ";");
                string length = "length" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "int " + length + "=[" + array + " length];");
                string dataBytes = "dataBytes" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "unsigned char *" + dataBytes + "=(unsigned char *)malloc(" + length + ");");
                string i = "i" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<" + length + ";" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                srw.WriteLine(GetTab(tabCount + 1) + dataBytes + "[" + i + "]=[[" + array + " objectAtIndex:" + i + "] unsignedCharValue];");
                srw.WriteLine(GetTab(tabCount) + "}");
                string data = "data" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSData *" + data + "=[NSData dataWithBytes:" + dataBytes + " length:" + length + "];");
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + data + "];");
            }
            else
            {
                string instanceNext = "instance" + rnd.Next().ToString();
                ReadContent(relation, srw, t, content, instanceNext, tabCount);
                srw.WriteLine(GetTab(tabCount) + "[" + instance + " addObject:" + instanceNext + "];");
            }
        }

        private static void ReadProperty(List<string> relation, StreamWriter srw, Type t, string content, string property, string instance, int tabCount)
        {
            if (t == typeof(string))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " string];");
            }
            else if (t == typeof(decimal))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " decimalValue];");
            }
            else if (t == typeof(bool))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " boolValue];");
            }
            else if (t == typeof(char))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " charValue];");
            }
            else if (t == typeof(byte))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " unsignedCharValue];");
            }
            else if (t == typeof(sbyte))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " charValue];");
            }
            else if (t == typeof(short))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " shortValue];");
            }
            else if (t == typeof(int))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " intValue];");
            }
            else if (t == typeof(long))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " longValue];");
            }
            else if (t == typeof(ushort))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " unsignedShortValue];");
            }
            else if (t == typeof(uint))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " unsignedIntValue];");
            }
            else if (t == typeof(ulong))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " unsignedLongValue];");
            }
            else if (t == typeof(float))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " floatValue];");
            }
            else if (t == typeof(double))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " doubleValue];");
            }
            else if (t == typeof(DateTime))
            {
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=[" + content + " date];");
            }
            else if (t == typeof(byte[]))
            {
                string array = "array" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSArray *" + array + "=" + content + ";");
                string length = "length" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "int " + length + "=[" + array + " length];");
                string dataBytes = "dataBytes" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "unsigned char *" + dataBytes + "=(unsigned char *)malloc(" + length + ");");
                string i = "i" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "for(int " + i + "=0;" + i + "<" + length + ";" + i + "++)");
                srw.WriteLine(GetTab(tabCount) + "{");
                srw.WriteLine(GetTab(tabCount + 1) + dataBytes + "[" + i + "]=[[" + array + " objectAtIndex:" + i + "] unsignedCharValue];");
                srw.WriteLine(GetTab(tabCount) + "}");
                string data = "data" + rnd.Next().ToString();
                srw.WriteLine(GetTab(tabCount) + "NSData *" + data + "=[NSData dataWithBytes:" + dataBytes + " length:" + length + "];");
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=" + data + ";");
            }
            else
            {
                string instanceNext = "instance" + rnd.Next().ToString();
                ReadContent(relation, srw, t, content, instanceNext, tabCount);
                srw.WriteLine(GetTab(tabCount) + instance + "." + property + "=" + instanceNext + ";");
            }
        }

        static Random rnd = new Random();
    }

    public class CodeParameter
    {
        public string ServerProtocal { get; set; }
        public string ClientProtocal { get; set; }
        public string ServerFunction { get; set; }
        public string ClientFunction { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public string Folder { get; set; }
    }

    public enum JsonModelType
    {
        None,
        Client,
        Server,
        ServerClient
    }
}
