using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace app {
    class Program {

        private static string EMPTY = "";
        private static string SEPARATOR = " ";
        private static string FS = "\t";
        private static char RS = '\n';
        private static string ITEM_JOINER = ".";
        private static string STRING_JOINER = ",";
        private static string COLUMN_JOINER = "-";
        private static string SUB_GROUP_DIGIT = "{0:000000}";
        private const string ASSEMBLY_NAME = "アセンブリ名";
        private const string NAMESPACE_NAME = "名前空間名";
        private static string TYPE_NAME = "型名";
        private static List<string> OUTPUT_COMMON_HEADER_LIST = new List<string> {
            ASSEMBLY_NAME,
            NAMESPACE_NAME,
            TYPE_NAME
        };
        private static string PROPERTY_OF_STATIC = "スタティックプロパティ名";
        private static string PROPERTY_OF_STATIC_RETURN_TYPE_NAME = "スタティックプロパティ名の戻り値の型名";
        private static List<string> OUTPUT_STATIC_PROPERTY_HEADER_LIST = new List<string> {
            PROPERTY_OF_STATIC,
            PROPERTY_OF_STATIC_RETURN_TYPE_NAME
        };
        private static string PROPERTY_OF_INSTANCE = "インスタンスプロパティ名";
        private static string PROPERTY_OF_INSTANCE_RETURN_TYPE_NAME = "インスタンスプロパティ名の戻り値の型名";
        private static List<string> OUTPUT_INSTANCE_PROPERTY_HEADER_LIST = new List<string> {
            PROPERTY_OF_INSTANCE,
            PROPERTY_OF_INSTANCE_RETURN_TYPE_NAME
        };
        private static string METHOD_OF_STATIC_NAME = "スタティックメソッド名";
        private static string METHOD_OF_STATIC_RETURN_TYPE_NAME = "スタティックメソッドの戻り値の型名";
        private static string METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT = "スタティックメソッドの仮引数の個数";
        private static string METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO = "スタティックメソッドの仮引数の位置番号";
        private static string METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME = "スタティックメソッドの仮引数の変数名";
        private static string METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME = "スタティックメソッドの仮引数の型名";
        private static List<string> OUTPUT_STATIC_METHOD_HEADER_LIST = new List<string> {
            METHOD_OF_STATIC_NAME,
            METHOD_OF_STATIC_RETURN_TYPE_NAME,
            METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT,
            METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO,
            METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME,
            METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME
        };
        private static string METHOD_OF_INSTANCE_NAME = "インスタンスメソッド名";
        private static string METHOD_OF_INSTANCE_RETURN_TYPE_NAME = "インスタンスメソッドの戻り値の型名";
        private static string METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT = "インスタンスメソッドの仮引数の個数";
        private static string METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO = "インスタンスメソッドの仮引数の位置番号";
        private static string METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME = "インスタンスメソッドの仮引数の変数名";
        private static string METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME = "インスタンスメソッドの仮引数の型名";
        private static List<string> OUTPUT_INSTANCE_METHOD_HEADER_LIST = new List<string> {
            METHOD_OF_INSTANCE_NAME,
            METHOD_OF_INSTANCE_RETURN_TYPE_NAME,
            METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT,
            METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO,
            METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME,
            METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME
        };
        private const string DEFAULT_NONE_STRING_VALUE = "ないよーん";
        private static int DEFAULT_NONE_INT_VALUE = 0;
        private static List<string> DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_INSTANCE_PROPERTY_HEADER_LIST;
        private const string OPTION_USAGE = "--usage";
        private const string OPTION_HELP = "--help";
        private const string OPTION_AS_ASSEMBLY_LIST = "--as-assembly-list";
        private const string OPTION_AS_NAMESPACE_LIST = "--as-namespace-list";
        private const string OPTION_AS_TYPE_LIST = "--as-type-list";
        private static string DEFAULT_AS_LIST = OPTION_AS_TYPE_LIST;
        private const string OPTION_SHOW_ASSEMBLY_LIST = "--show-assembly-list";
        private const string OPTION_SHOW_NAMESPACE_LIST = "--show-namespace-list";
        private const string OPTION_SHOW_TYPE_LIST = "--show-type-list";
        private static string DEFAULT_SHOW_LIST = "";
        private const string OPTION_INTERNAL_LIB = "--internal-lib";
        private const string OPTION_EXTERNAL_LIB = "--external-lib";
        private const string OPTION_DLL_FILE_LIB = "--dll-file-lib";
        private static string DEFAULT_MODE = OPTION_INTERNAL_LIB;
        private const string OPTION_PROPERTY_STATIC = "--property-static";
        private const string OPTION_PROPERTY_INSTANCE = "--property-instance";
        private const string OPTION_METHOD_STATIC = "--method-static";
        private const string OPTION_METHOD_INSTANCE = "--method-instance";
        private static string DEFAULT_FETCH_INFO = OPTION_PROPERTY_INSTANCE;
        private static List<string> OPTION_EXAMPLE_TYPE_LIST = new List<string> {
            "Newtonsoft.Json",
            "System.DateTime",
            "System.Text.NormalizationForm",
            "System.Text.Rune",
            "/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll",
            "Unity.UnityContainer",
            "Unity.Events.NamedEventArgs"
        };
        private static List<string> OPTION_MODE_LIST = new List<string> {
            OPTION_INTERNAL_LIB,
            OPTION_EXTERNAL_LIB,
            OPTION_DLL_FILE_LIB
        };
        private static List<string> OPTION_AS_LIST = new List<string> {
            OPTION_AS_ASSEMBLY_LIST,
            OPTION_AS_NAMESPACE_LIST,
            OPTION_AS_TYPE_LIST
        };
        private static List<string> OPTION_SHOW_LIST = new List<string> {
            OPTION_SHOW_ASSEMBLY_LIST,
            OPTION_SHOW_NAMESPACE_LIST,
            OPTION_SHOW_TYPE_LIST
        };
        private static List<string> OPTION_FETCH_INFO_LIST = new List<string> {
            OPTION_PROPERTY_STATIC,
            OPTION_PROPERTY_INSTANCE,
            OPTION_METHOD_STATIC,
            OPTION_METHOD_INSTANCE
        };
        private static Dictionary<string, Dictionary<string, string>> getPropertyOfStaticSummaryInfoDict (Type type) {
            //クラスのパブリックなスタティックプロパティを取得
            Dictionary<string, Dictionary<string, string>> propertyOfStaticSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<PropertyInfo> propertyInfoList = type.GetProperties (BindingFlags.Static | BindingFlags.Public).ToList ();

            foreach (PropertyInfo propertyInfo in propertyInfoList) {

                subgrp++;

                Dictionary<string, string> propertyOfStaticDetailInfoDict = new Dictionary<string, string> ();

                propertyOfStaticDetailInfoDict.Add (PROPERTY_OF_STATIC, propertyInfo.Name);
                propertyOfStaticDetailInfoDict.Add (PROPERTY_OF_STATIC_RETURN_TYPE_NAME, propertyInfo.ToString ().Split (SEPARATOR) [0]);

                propertyOfStaticSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, propertyOfStaticDetailInfoDict);
            }

            return propertyOfStaticSummaryInfoDict;
        }
        private static Dictionary<string, Dictionary<string, string>> getPropertyOfInstanceSummaryInfoDict (Type type) {
            //クラスのパブリックなインスタンスプロパティを取得
            Dictionary<string, Dictionary<string, string>> propertyOfInstanceSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<PropertyInfo> propertyInfoList = type.GetProperties (BindingFlags.Instance | BindingFlags.Public).ToList ();

            foreach (PropertyInfo propertyInfo in propertyInfoList) {

                subgrp++;

                Dictionary<string, string> propertyOfInstanceDetailInfoDict = new Dictionary<string, string> ();

                propertyOfInstanceDetailInfoDict.Add (PROPERTY_OF_INSTANCE, propertyInfo.Name);
                propertyOfInstanceDetailInfoDict.Add (PROPERTY_OF_INSTANCE_RETURN_TYPE_NAME, propertyInfo.ToString ().Split (SEPARATOR) [0]);

                propertyOfInstanceSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, propertyOfInstanceDetailInfoDict);
            }

            return propertyOfInstanceSummaryInfoDict;

        }
        private static Dictionary<string, Dictionary<string, string>> getMethodOfStaticSummaryInfoDict (Type type) {
            // クラスのパブリックなスタティックメソッドを取得
            Dictionary<string, Dictionary<string, string>> methodOfStaticSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<MethodInfo> methodOfStaticInfoList = type.GetMethods (BindingFlags.Static | BindingFlags.Public).ToList ();

            foreach (MethodInfo methodOfStaticInfo in methodOfStaticInfoList) {

                subgrp++;

                List<ParameterInfo> parameterOfStaticInfoList = methodOfStaticInfo.GetParameters ().ToList ();

                Dictionary<string, string> methodOfStaticDetailInfoDict = new Dictionary<string, string> ();

                methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_NAME, methodOfStaticInfo.Name);
                methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_RETURN_TYPE_NAME, methodOfStaticInfo.ReturnType.FullName);

                if (parameterOfStaticInfoList.Count == 0) {
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT, DEFAULT_NONE_INT_VALUE.ToString ());
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO, DEFAULT_NONE_STRING_VALUE);
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME, DEFAULT_NONE_STRING_VALUE);
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME, DEFAULT_NONE_STRING_VALUE);
                } else {
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT, string.Join (STRING_JOINER, parameterOfStaticInfoList.Count.ToString ()));
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.Position).ToArray ()));
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.Name).ToArray ()));
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.ParameterType.Name).ToArray ()));
                }

                methodOfStaticSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, methodOfStaticDetailInfoDict);
            }

            return methodOfStaticSummaryInfoDict;

        }
        private static Dictionary<string, Dictionary<string, string>> getMethodOfInstanceSummaryInfoDict (Type type) {
            // クラスのパブリックなインスタンスメソッドを取得
            Dictionary<string, Dictionary<string, string>> methodOfInstanceSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<MethodInfo> methodOfInstanceInfoList = type.GetMethods (BindingFlags.Instance | BindingFlags.Public).ToList ();

            foreach (MethodInfo methodOfInstanceInfo in methodOfInstanceInfoList) {

                subgrp++;

                List<ParameterInfo> parameterOfInstanceInfoList = methodOfInstanceInfo.GetParameters ().ToList ();

                Dictionary<string, string> methodOfInstanceDetailInfoDict = new Dictionary<string, string> ();

                methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_NAME, methodOfInstanceInfo.Name);
                methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_RETURN_TYPE_NAME, methodOfInstanceInfo.ReturnType.FullName);

                if (parameterOfInstanceInfoList.Count == 0) {
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT, DEFAULT_NONE_INT_VALUE.ToString ());
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO, DEFAULT_NONE_STRING_VALUE);
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME, DEFAULT_NONE_STRING_VALUE);
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME, DEFAULT_NONE_STRING_VALUE);
                } else {
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Count.ToString ()));
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.Position).ToArray ()));
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.Name).ToArray ()));
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.ParameterType.Name).ToArray ()));
                }

                methodOfInstanceSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, methodOfInstanceDetailInfoDict);
            }

            return methodOfInstanceSummaryInfoDict;

        }
        private static HashSet<string> getStdLibAssemblyNameHashSet () {
            //標準ライブラリ
            HashSet<string> stdlibAssemblyNameHashSet = AppDomain.CurrentDomain.GetAssemblies ().Select (Assembly => Assembly.GetName ().Name).ToHashSet ();

            return stdlibAssemblyNameHashSet;
        }
        private static HashSet<string> getExtLibAssemblyNameHashSet (HashSet<string> extLibAssemblyHashSet) {
            //外部ライブラリ
            Dictionary<string, List<Type>> assemblyTypeDict = getExtLibAssemblyTypeList (extLibAssemblyHashSet);

            HashSet<string> extlibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            return extlibAssemblyNameHashSet;
        }
        private static HashSet<string> getDllLibAssemblyNameHashSet (HashSet<string> dllLibAssemblyHashSet) {
            //DLLライブラリ
            Dictionary<string, List<Type>> assemblyTypeDict = getDllLibAssemblyTypeList (dllLibAssemblyHashSet);

            HashSet<string> dlllibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            return dlllibAssemblyNameHashSet;
        }
        private static HashSet<string> getStdLibNamespaceNameHashSet () {
            //標準ライブラリ
            HashSet<HashSet<string>> stdLibNamespaceNameHashSet = new HashSet<HashSet<string>> ();

            HashSet<Assembly> stdlibAssemblyHashSet = AppDomain.CurrentDomain.GetAssemblies ().ToHashSet ();

            foreach (Assembly assembly in stdlibAssemblyHashSet) {

                stdLibNamespaceNameHashSet.Add (assembly.GetTypes ().Select (type => type.Namespace).ToHashSet ());
            }

            return stdLibNamespaceNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static HashSet<string> getExtLibNamespaceNameHashSet (HashSet<string> extLibAssemblyHashSet) {
            //外部ライブラリ
            HashSet<HashSet<string>> extLibNamespaceNameHashSet = new HashSet<HashSet<string>> ();

            Dictionary<string, List<Type>> assemblyTypeDict = getExtLibAssemblyTypeList (extLibAssemblyHashSet);

            HashSet<string> extlibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            foreach (string assemblyName in extlibAssemblyNameHashSet) {

                extLibNamespaceNameHashSet.Add (assemblyTypeDict[assemblyName].Select (type => type.Namespace).ToHashSet ());

            }

            return extLibNamespaceNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static HashSet<string> getDllLibNamespaceNameHashSet (HashSet<string> dllLibAssemblyHashSet) {
            //DLLライブラリ
            HashSet<HashSet<string>> dllLibNamespaceNameHashSet = new HashSet<HashSet<string>> ();

            Dictionary<string, List<Type>> assemblyTypeDict = getDllLibAssemblyTypeList (dllLibAssemblyHashSet);

            HashSet<string> dlllibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            foreach (string assemblyName in dlllibAssemblyNameHashSet) {

                dllLibNamespaceNameHashSet.Add (assemblyTypeDict[assemblyName].Select (type => type.Namespace).ToHashSet ());

            }

            return dllLibNamespaceNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static HashSet<string> getStdLibTypeNameHashSet () {
            //標準ライブラリ
            HashSet<HashSet<string>> stdLibTypeNameHashSet = new HashSet<HashSet<string>> ();

            HashSet<Assembly> stdlibAssemblyList = AppDomain.CurrentDomain.GetAssemblies ().ToHashSet ();

            foreach (Assembly assembly in stdlibAssemblyList) {

                stdLibTypeNameHashSet.Add (assembly.GetTypes ().Select (type => type.FullName).ToHashSet ());
            }

            return stdLibTypeNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static HashSet<string> getExtLibTypeNameHashSet (HashSet<string> extLibAssemblyHashSet) {
            //外部ライブラリ
            HashSet<HashSet<string>> extLibTypeNameHashSet = new HashSet<HashSet<string>> ();

            Dictionary<string, List<Type>> assemblyTypeDict = getExtLibAssemblyTypeList (extLibAssemblyHashSet);

            HashSet<string> extlibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            foreach (string assemblyName in extlibAssemblyNameHashSet) {

                extLibTypeNameHashSet.Add (assemblyTypeDict[assemblyName].Select (type => type.FullName).ToHashSet ());

            }

            return extLibTypeNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static HashSet<string> getDllLibTypeNameHashSet (HashSet<string> dllLibAssemblyHashSet) {
            //DLLライブラリ
            HashSet<HashSet<string>> dllLibTypeNameHashSet = new HashSet<HashSet<string>> ();

            Dictionary<string, List<Type>> assemblyTypeDict = getDllLibAssemblyTypeList (dllLibAssemblyHashSet);

            HashSet<string> dlllibAssemblyNameHashSet = assemblyTypeDict.Keys.ToHashSet ();

            foreach (string assemblyName in dlllibAssemblyNameHashSet) {

                dllLibTypeNameHashSet.Add (assemblyTypeDict[assemblyName].Select (type => type.FullName).ToHashSet ());

            }

            return dllLibTypeNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }
        private static Dictionary<string, List<Type>> getStdLibAssemblyTypeList () {
            //標準ライブラリ
            Dictionary<string, List<Type>> assemblyTypeDict = new Dictionary<string, List<Type>> ();

            List<Assembly> stdlibAssemblyList = AppDomain.CurrentDomain.GetAssemblies ().ToList ();

            foreach (Assembly assembly in stdlibAssemblyList) {

                string assemblyName = assembly.GetName ().Name;

                List<Type> typeList = assembly.GetTypes ().ToList ();

                assemblyTypeDict.Add (assemblyName, typeList);

            }
            return assemblyTypeDict;
        }
        private static Dictionary<string, List<Type>> getExtLibAssemblyTypeList (HashSet<string> extLibAssemblyHashSet) {
            //外部ライブラリ
            Dictionary<string, List<Type>> assemblyTypeDict = new Dictionary<string, List<Type>> ();

            foreach (string extLibAssembly in extLibAssemblyHashSet) {

                List<string> itemList = extLibAssembly.Split (ITEM_JOINER).ToList ();

                int itemCnt = itemList.Count;

                for (int i = 1; i <= itemCnt; i++) {

                    string tryLoad = String.Join (ITEM_JOINER, itemList.GetRange (0, i).ToArray ());

                    Assembly assembly = null;

                    try {
                        assembly = Assembly.Load (tryLoad);

                        string assemblyName = assembly.GetName ().Name;

                        List<Type> typeList = assembly.GetTypes ().ToList ();

                        assemblyTypeDict.Add (assemblyName, typeList);

                    } catch (System.Exception) {
                        //nothing to do
                    }
                }
            }
            return assemblyTypeDict;
        }
        private static Dictionary<string, List<Type>> getDllLibAssemblyTypeList (HashSet<string> dllLibAssemblyHashSet) {
            //DLLライブラリ
            Dictionary<string, List<Type>> assemblyTypeDict = new Dictionary<string, List<Type>> ();

            foreach (string dllLibAssembly in dllLibAssemblyHashSet) {

                Assembly assembly = Assembly.LoadFrom (dllLibAssembly);

                string assemblyName = assembly.GetName ().Name;

                List<Type> typeList = assembly.GetTypes ().ToList ();

                assemblyTypeDict.Add (assemblyName, typeList);

            }
            return assemblyTypeDict;
        }
        private static void Usage (string appName) {
            Console.WriteLine (EMPTY +
                RS +
                RS +
                "Default: --internal-lib mode AND --property-instance mode AND --as-type-list mode" +
                RS +
                RS +
                "Usage:" +
                RS +
                RS +
                "CMD: " + appName + SEPARATOR + "[" + String.Join (SEPARATOR, OPTION_SHOW_LIST.ToArray ()) + "]" +
                RS +
                RS +
                "CMD: " + appName + SEPARATOR + "[" + String.Join (SEPARATOR, OPTION_MODE_LIST.ToArray ()) + "]" + SEPARATOR + "[" + String.Join (SEPARATOR, OPTION_AS_LIST.ToArray ()) + "]" + SEPARATOR + " [" + String.Join (SEPARATOR, OPTION_EXAMPLE_TYPE_LIST.ToArray ()) + "]" + SEPARATOR + "[" + String.Join (SEPARATOR, OPTION_FETCH_INFO_LIST.ToArray ()) + "]" +
                RS
            );

            Environment.Exit (0);
        }

        private static void outputShowList (HashSet<string> showHashSet) {

            var showSortedHashSet = new SortedSet<string> (showHashSet.Where (item => item != null).ToHashSet ());

            foreach (string item in showSortedHashSet) {
                Console.WriteLine (item);
            }
        }

        private static void outputHeader (List<string> defaultOutputHeaderList) {
            //header
            {
                {
                    Console.Write (String.Join (FS, OUTPUT_COMMON_HEADER_LIST.ToArray ()));
                    Console.Write (FS);
                    Console.Write (String.Join (FS, defaultOutputHeaderList.ToArray ()));
                }
                Console.WriteLine ();
            }
        }

        private static void outputPropertyOfStaticSummaryInfoDict (string assemblyName, Type type, Dictionary<string, Dictionary<string, string>> summaryDict) {
            //body
            foreach (string rowNum in summaryDict.Keys) {

                Dictionary<string, string> detailDict = summaryDict[rowNum];

                {
                    Console.Write (assemblyName);
                    Console.Write (FS);
                    Console.Write (type.Namespace == null ? DEFAULT_NONE_STRING_VALUE : type.Namespace);
                    Console.Write (FS);
                    Console.Write (type.FullName);
                    Console.Write (FS);
                    Console.Write (detailDict[PROPERTY_OF_STATIC]);
                    Console.Write (FS);
                    Console.Write (detailDict[PROPERTY_OF_STATIC_RETURN_TYPE_NAME]);
                }
                Console.WriteLine ();

            }
        }

        private static void outputPropertyOfInstanceSummaryInfoDict (string assemblyName, Type type, Dictionary<string, Dictionary<string, string>> summaryDict) {
            //body
            foreach (string rowNum in summaryDict.Keys) {

                Dictionary<string, string> detailDict = summaryDict[rowNum];

                {
                    Console.Write (assemblyName);
                    Console.Write (FS);
                    Console.Write (type.Namespace == null ? DEFAULT_NONE_STRING_VALUE : type.Namespace);
                    Console.Write (FS);
                    Console.Write (type.FullName);
                    Console.Write (FS);
                    Console.Write (detailDict[PROPERTY_OF_INSTANCE]);
                    Console.Write (FS);
                    Console.Write (detailDict[PROPERTY_OF_INSTANCE_RETURN_TYPE_NAME]);
                }
                Console.WriteLine ();

            }
        }

        private static void outputMethodOfStaticSummaryInfoDict (string assemblyName, Type type, Dictionary<string, Dictionary<string, string>> summaryDict) {
            //body
            foreach (string rowNum in summaryDict.Keys) {

                Dictionary<string, string> detailDict = summaryDict[rowNum];

                {
                    Console.Write (assemblyName);
                    Console.Write (FS);
                    Console.Write (type.Namespace == null ? DEFAULT_NONE_STRING_VALUE : type.Namespace);
                    Console.Write (FS);
                    Console.Write (type.FullName);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_RETURN_TYPE_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME]);
                }
                Console.WriteLine ();

            }
        }
        private static void outputMethodOfInstanceSummaryInfoDict (string assemblyName, Type type, Dictionary<string, Dictionary<string, string>> summaryDict) {
            //body
            foreach (string rowNum in summaryDict.Keys) {

                Dictionary<string, string> detailDict = summaryDict[rowNum];

                {
                    Console.Write (assemblyName);
                    Console.Write (FS);
                    Console.Write (type.Namespace == null ? DEFAULT_NONE_STRING_VALUE : type.Namespace);
                    Console.Write (FS);
                    Console.Write (type.FullName);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_RETURN_TYPE_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME]);
                    Console.Write (FS);
                    Console.Write (detailDict[METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME]);
                }
                Console.WriteLine ();

            }
        }
        private static void showStdLibFilteredByAssemblyNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> stdLibInfoDict = getStdLibAssemblyTypeList ();

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            //filter
            List<string> assemblyNameList = stdLibInfoDict.Keys.Where (assemblyName => targetNameHashSet.Contains (assemblyName)).ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {
                List<Type> typeList = stdLibInfoDict[assemblyName];

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showStdLibFilteredByNamespaceNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> stdLibInfoDict = getStdLibAssemblyTypeList ();

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = stdLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = stdLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.Namespace)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showStdLibFilteredByTypeNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> stdLibInfoDict = getStdLibAssemblyTypeList ();

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = stdLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = stdLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.FullName)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showExtLibFilteredByAssemblyNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibInfoDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            //filter
            List<string> assemblyNameList = extLibInfoDict.Keys.Where (assemblyName => targetNameHashSet.Contains (assemblyName)).ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {
                List<Type> typeList = extLibInfoDict[assemblyName];

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showExtLibFilteredByNamespaceNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibInfoDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = extLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = extLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.Namespace)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showExtLibFilteredByTypeNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibInfoDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = extLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = extLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.FullName)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showDllLibFilteredByAssemblyNameListInfo (string appName, HashSet<string> targetNameHashSet, HashSet<string> dllFileHashSet) {

            Dictionary<string, List<Type>> dllLibInfoDict = getDllLibAssemblyTypeList (dllFileHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            //filter
            List<string> assemblyNameList = dllLibInfoDict.Keys.Where (assemblyName => targetNameHashSet.Contains (assemblyName)).ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {
                List<Type> typeList = dllLibInfoDict[assemblyName];

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showDllLibFilteredByNamespaceNameListInfo (string appName, HashSet<string> targetNameHashSet, HashSet<string> dllFileHashSet) {

            Dictionary<string, List<Type>> dllLibInfoDict = getDllLibAssemblyTypeList (dllFileHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = dllLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = dllLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.Namespace)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        private static void showDllLibFilteredByTypeNameListInfo (string appName, HashSet<string> targetNameHashSet, HashSet<string> dllFileHashSet) {

            Dictionary<string, List<Type>> dllLibInfoDict = getDllLibAssemblyTypeList (dllFileHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = dllLibInfoDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = dllLibInfoDict[assemblyName].Where (type => targetNameHashSet.Contains (type.FullName)).ToList ();

                foreach (Type type in typeList) {

                    Dictionary<string, Dictionary<string, string>> summaryDict = null;

                    switch (DEFAULT_FETCH_INFO) {

                        case OPTION_PROPERTY_STATIC:
                            summaryDict = getPropertyOfStaticSummaryInfoDict (type);
                            outputPropertyOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            summaryDict = getPropertyOfInstanceSummaryInfoDict (type);
                            outputPropertyOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_STATIC:
                            summaryDict = getMethodOfStaticSummaryInfoDict (type);
                            outputMethodOfStaticSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        case OPTION_METHOD_INSTANCE:
                            summaryDict = getMethodOfInstanceSummaryInfoDict (type);
                            outputMethodOfInstanceSummaryInfoDict (assemblyName, type, summaryDict);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                }
            }
        }
        static void Main (string[] args) {

            string appName = Environment.GetCommandLineArgs () [0];
            appName = Regex.Replace (appName, ".*/", EMPTY); //ファイル名のみにする
            appName = Regex.Replace (appName, "\\..*", EMPTY); //拡張子を取り除く

            List<string> cmdLineArgs = args.ToList ();

            HashSet<string> targetNameHashSet = new HashSet<string> ();

            if (cmdLineArgs.Count == 0) {

                //パイプ経由からの入力の場合
                if (!Console.IsInputRedirected) {
                    //パイプ経由からの入力がない場合
                    Usage (appName);

                }

                string line;
                int rowNum = 0;

                Dictionary<int, List<string>> map = new Dictionary<int, List<string>> ();

                while ((line = Console.ReadLine ()) != null) {
                    //標準入力からの文字列を読み込み、空文字列でない限り、繰り返し処理
                    rowNum++;
                    map.Add (rowNum, line.Split (SEPARATOR).ToList ());
                }

                if (map.Count != 1) {
                    //単一行でない場合
                    Usage (appName);
                }

                foreach (string arg in map[1]) {
                    switch (arg) {
                        case OPTION_USAGE:
                            Usage (appName);
                            break;
                        case OPTION_HELP:
                            Usage (appName);
                            break;
                        case OPTION_AS_ASSEMBLY_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_ASSEMBLY_LIST;
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_NAMESPACE_LIST;
                            break;
                        case OPTION_AS_TYPE_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_TYPE_LIST;
                            break;
                        case OPTION_SHOW_ASSEMBLY_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_ASSEMBLY_LIST;
                            break;
                        case OPTION_SHOW_NAMESPACE_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_NAMESPACE_LIST;
                            break;
                        case OPTION_SHOW_TYPE_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_TYPE_LIST;
                            break;
                        case OPTION_INTERNAL_LIB:
                            DEFAULT_MODE = OPTION_INTERNAL_LIB;
                            break;
                        case OPTION_EXTERNAL_LIB:
                            DEFAULT_MODE = OPTION_EXTERNAL_LIB;
                            break;
                        case OPTION_DLL_FILE_LIB:
                            DEFAULT_MODE = OPTION_DLL_FILE_LIB;
                            break;
                        case OPTION_PROPERTY_STATIC:
                            DEFAULT_FETCH_INFO = OPTION_PROPERTY_STATIC;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_STATIC_PROPERTY_HEADER_LIST;
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            DEFAULT_FETCH_INFO = OPTION_PROPERTY_INSTANCE;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_INSTANCE_PROPERTY_HEADER_LIST;
                            break;
                        case OPTION_METHOD_STATIC:
                            DEFAULT_FETCH_INFO = OPTION_METHOD_STATIC;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_STATIC_METHOD_HEADER_LIST;
                            break;
                        case OPTION_METHOD_INSTANCE:
                            DEFAULT_FETCH_INFO = OPTION_METHOD_INSTANCE;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_INSTANCE_METHOD_HEADER_LIST;
                            break;
                        default:
                            //TODO
                            targetNameHashSet.Add (arg);
                            break;
                    }
                }

            } else {
                foreach (string arg in cmdLineArgs) {
                    switch (arg) {
                        case OPTION_USAGE:
                            Usage (appName);
                            break;
                        case OPTION_HELP:
                            Usage (appName);
                            break;
                        case OPTION_AS_ASSEMBLY_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_ASSEMBLY_LIST;
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_NAMESPACE_LIST;
                            break;
                        case OPTION_AS_TYPE_LIST:
                            DEFAULT_AS_LIST = OPTION_AS_TYPE_LIST;
                            break;
                        case OPTION_SHOW_ASSEMBLY_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_ASSEMBLY_LIST;
                            break;
                        case OPTION_SHOW_NAMESPACE_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_NAMESPACE_LIST;
                            break;
                        case OPTION_SHOW_TYPE_LIST:
                            DEFAULT_SHOW_LIST = OPTION_SHOW_TYPE_LIST;
                            break;
                        case OPTION_INTERNAL_LIB:
                            DEFAULT_MODE = OPTION_INTERNAL_LIB;
                            break;
                        case OPTION_EXTERNAL_LIB:
                            DEFAULT_MODE = OPTION_EXTERNAL_LIB;
                            break;
                        case OPTION_DLL_FILE_LIB:
                            DEFAULT_MODE = OPTION_DLL_FILE_LIB;
                            break;
                        case OPTION_PROPERTY_STATIC:
                            DEFAULT_FETCH_INFO = OPTION_PROPERTY_STATIC;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_STATIC_PROPERTY_HEADER_LIST;
                            break;
                        case OPTION_PROPERTY_INSTANCE:
                            DEFAULT_FETCH_INFO = OPTION_PROPERTY_INSTANCE;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_INSTANCE_PROPERTY_HEADER_LIST;
                            break;
                        case OPTION_METHOD_STATIC:
                            DEFAULT_FETCH_INFO = OPTION_METHOD_STATIC;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_STATIC_METHOD_HEADER_LIST;
                            break;
                        case OPTION_METHOD_INSTANCE:
                            DEFAULT_FETCH_INFO = OPTION_METHOD_INSTANCE;
                            DEFAULT_OUTPUT_HEADER_LIST = OUTPUT_INSTANCE_METHOD_HEADER_LIST;
                            break;
                        default:
                            //TODO
                            targetNameHashSet.Add (arg);
                            break;
                    }
                }
            }

            if (targetNameHashSet.Where (e => e.IndexOf ("-") == 0).ToList ().Count != 0) {
                //オプション引数が指定したもの以外にマッチした場合は除外
                Usage (appName);
            }

            HashSet<string> dllFileHashSet = null;
            if (DEFAULT_MODE == OPTION_DLL_FILE_LIB) {
                //ファイルの存在チェック
                dllFileHashSet = targetNameHashSet.Where (filePath => File.Exists (filePath)).ToHashSet ();

                //dllファイルは絞り込み条件から除外
                targetNameHashSet = targetNameHashSet.Where (filePath => !File.Exists (filePath)).ToHashSet ();

                if (dllFileHashSet.Count == 0) {
                    //指定したファイルパスが存在しない場合は除外
                    Usage (appName);
                }
            }

            HashSet<string> showList = null;

            switch (DEFAULT_MODE) {
                case OPTION_INTERNAL_LIB:
                    switch (DEFAULT_SHOW_LIST) {
                        case OPTION_SHOW_ASSEMBLY_LIST:
                            showList = getStdLibAssemblyNameHashSet ();
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_NAMESPACE_LIST:
                            showList = getStdLibNamespaceNameHashSet ();
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_TYPE_LIST:
                            showList = getStdLibTypeNameHashSet ();
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        default:
                            break;
                    }
                    break;
                case OPTION_EXTERNAL_LIB:
                    switch (DEFAULT_SHOW_LIST) {
                        case OPTION_SHOW_ASSEMBLY_LIST:
                            showList = getExtLibAssemblyNameHashSet (targetNameHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_NAMESPACE_LIST:
                            showList = getExtLibNamespaceNameHashSet (targetNameHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_TYPE_LIST:
                            showList = getExtLibTypeNameHashSet (targetNameHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        default:
                            break;
                    }
                    break;
                case OPTION_DLL_FILE_LIB:
                    switch (DEFAULT_SHOW_LIST) {
                        case OPTION_SHOW_ASSEMBLY_LIST:
                            showList = getDllLibAssemblyNameHashSet (dllFileHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_NAMESPACE_LIST:
                            showList = getDllLibNamespaceNameHashSet (dllFileHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        case OPTION_SHOW_TYPE_LIST:
                            showList = getDllLibTypeNameHashSet (dllFileHashSet);
                            outputShowList (showList);
                            Environment.Exit (0);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Usage (appName);
                    break;
            }

            switch (DEFAULT_MODE) {
                case OPTION_INTERNAL_LIB:
                    switch (DEFAULT_AS_LIST) {
                        case OPTION_AS_ASSEMBLY_LIST:
                            showStdLibFilteredByAssemblyNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            showStdLibFilteredByNamespaceNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_TYPE_LIST:
                            showStdLibFilteredByTypeNameListInfo (appName, targetNameHashSet);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                    break;
                case OPTION_EXTERNAL_LIB:
                    switch (DEFAULT_AS_LIST) {
                        case OPTION_AS_ASSEMBLY_LIST:
                            showExtLibFilteredByAssemblyNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            showExtLibFilteredByNamespaceNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_TYPE_LIST:
                            showExtLibFilteredByTypeNameListInfo (appName, targetNameHashSet);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                    break;
                case OPTION_DLL_FILE_LIB:
                    switch (DEFAULT_AS_LIST) {
                        case OPTION_AS_ASSEMBLY_LIST:
                            showDllLibFilteredByAssemblyNameListInfo (appName, targetNameHashSet, dllFileHashSet);
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            showDllLibFilteredByNamespaceNameListInfo (appName, targetNameHashSet, dllFileHashSet);
                            break;
                        case OPTION_AS_TYPE_LIST:
                            showDllLibFilteredByTypeNameListInfo (appName, targetNameHashSet, dllFileHashSet);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                    break;
                default:
                    Usage (appName);
                    break;
            }
        }
    }
}
