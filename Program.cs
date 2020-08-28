﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace app {
    class Program {

        private static string EMPTY = "";
        private static string SEPARATOR = " ";
        private static string FS = "\t";
        private static char RS = '\n';
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
            "System.Text.Rune"
        };
        private static List<string> OPTION_MODE_LIST = new List<string> {
            OPTION_INTERNAL_LIB,
            OPTION_EXTERNAL_LIB
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
        //クラスのパブリックなスタティックプロパティを取得
        private static Dictionary<string, Dictionary<string, string>> getPropertyOfStaticSummaryInfoDict (Type type) {
            Dictionary<string, Dictionary<string, string>> propertyOfStaticSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<PropertyInfo> propertyInfoList = type.GetProperties (BindingFlags.Static | BindingFlags.Public).ToList ();

            foreach (PropertyInfo propertyInfo in propertyInfoList) {

                subgrp++;

                Dictionary<string, string> propertyOfStaticDetailInfoDict = new Dictionary<string, string> ();

                propertyOfStaticDetailInfoDict.Add (PROPERTY_OF_STATIC, propertyInfo.Name); //スタティックプロパティ名
                propertyOfStaticDetailInfoDict.Add (PROPERTY_OF_STATIC_RETURN_TYPE_NAME, propertyInfo.ToString ().Split (SEPARATOR) [0]); //スタティックプロパティ名の戻り値の型名

                propertyOfStaticSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, propertyOfStaticDetailInfoDict);
            }

            return propertyOfStaticSummaryInfoDict;
        }

        //クラスのパブリックなインスタンスプロパティを取得
        private static Dictionary<string, Dictionary<string, string>> getPropertyOfInstanceSummaryInfoDict (Type type) {
            Dictionary<string, Dictionary<string, string>> propertyOfInstanceSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<PropertyInfo> propertyInfoList = type.GetProperties (BindingFlags.Instance | BindingFlags.Public).ToList ();

            foreach (PropertyInfo propertyInfo in propertyInfoList) {

                subgrp++;

                Dictionary<string, string> propertyOfInstanceDetailInfoDict = new Dictionary<string, string> ();

                propertyOfInstanceDetailInfoDict.Add (PROPERTY_OF_INSTANCE, propertyInfo.Name); //インスタンスプロパティ名
                propertyOfInstanceDetailInfoDict.Add (PROPERTY_OF_INSTANCE_RETURN_TYPE_NAME, propertyInfo.ToString ().Split (SEPARATOR) [0]); //インスタンスプロパティ名の戻り値の型名

                propertyOfInstanceSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, propertyOfInstanceDetailInfoDict);
            }

            return propertyOfInstanceSummaryInfoDict;

        }

        // クラスのパブリックなスタティックメソッドを取得
        private static Dictionary<string, Dictionary<string, string>> getMethodOfStaticSummaryInfoDict (Type type) {
            Dictionary<string, Dictionary<string, string>> methodOfStaticSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<MethodInfo> methodOfStaticInfoList = type.GetMethods (BindingFlags.Static | BindingFlags.Public).ToList ();

            foreach (MethodInfo methodOfStaticInfo in methodOfStaticInfoList) {

                subgrp++;

                List<ParameterInfo> parameterOfStaticInfoList = methodOfStaticInfo.GetParameters ().ToList ();

                Dictionary<string, string> methodOfStaticDetailInfoDict = new Dictionary<string, string> ();

                methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_NAME, methodOfStaticInfo.Name); //メソッド名
                methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_RETURN_TYPE_NAME, methodOfStaticInfo.ReturnType.FullName); //メソッドの戻り値の型名

                if (parameterOfStaticInfoList.Count == 0) {
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT, DEFAULT_NONE_INT_VALUE.ToString ()); //メソッドの仮引数の個数
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の位置番号
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の変数名
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の型名
                } else {
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_COUNT, string.Join (STRING_JOINER, parameterOfStaticInfoList.Count.ToString ())); //メソッドの仮引数の個数
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_POSITION_NO, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.Position).ToArray ())); //メソッドの仮引数の位置番号
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_VARIABLE_NAME, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.Name).ToArray ())); //メソッドの仮引数の変数名
                    methodOfStaticDetailInfoDict.Add (METHOD_OF_STATIC_PHONY_ARGUMENT_RETURN_TYPE_NAME, string.Join (STRING_JOINER, parameterOfStaticInfoList.Select (parameterInfo => parameterInfo.ParameterType.Name).ToArray ())); //メソッドの仮引数の型名
                }

                methodOfStaticSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, methodOfStaticDetailInfoDict);
            }

            return methodOfStaticSummaryInfoDict;

        }

        // クラスのパブリックなインスタンスメソッドを取得
        private static Dictionary<string, Dictionary<string, string>> getMethodOfInstanceSummaryInfoDict (Type type) {
            Dictionary<string, Dictionary<string, string>> methodOfInstanceSummaryInfoDict = new Dictionary<string, Dictionary<string, string>> ();

            int subgrp = 0;

            List<MethodInfo> methodOfInstanceInfoList = type.GetMethods (BindingFlags.Instance | BindingFlags.Public).ToList ();

            foreach (MethodInfo methodOfInstanceInfo in methodOfInstanceInfoList) {

                subgrp++;

                List<ParameterInfo> parameterOfInstanceInfoList = methodOfInstanceInfo.GetParameters ().ToList ();

                Dictionary<string, string> methodOfInstanceDetailInfoDict = new Dictionary<string, string> ();

                methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_NAME, methodOfInstanceInfo.Name); //メソッド名
                methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_RETURN_TYPE_NAME, methodOfInstanceInfo.ReturnType.FullName); //メソッドの戻り値の型名

                if (parameterOfInstanceInfoList.Count == 0) {
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT, DEFAULT_NONE_INT_VALUE.ToString ()); //メソッドの仮引数の個数
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の位置番号
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の変数名
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME, DEFAULT_NONE_STRING_VALUE); //メソッドの仮引数の型名
                } else {
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_COUNT, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Count.ToString ())); //メソッドの仮引数の個数
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_POSITION_NO, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.Position).ToArray ())); //メソッドの仮引数の位置番号
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_VARIABLE_NAME, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.Name).ToArray ())); //メソッドの仮引数の変数名
                    methodOfInstanceDetailInfoDict.Add (METHOD_OF_INSTANCE_PHONY_ARGUMENT_RETURN_TYPE_NAME, string.Join (STRING_JOINER, parameterOfInstanceInfoList.Select (parameterInfo => parameterInfo.ParameterType.Name).ToArray ())); //メソッドの仮引数の型名
                }

                methodOfInstanceSummaryInfoDict.Add (String.Format (SUB_GROUP_DIGIT, subgrp) + COLUMN_JOINER + type.FullName, methodOfInstanceDetailInfoDict);
            }

            return methodOfInstanceSummaryInfoDict;

        }

        private static HashSet<string> getStdLibAssemblyNameHashSet () {
            //デフォルトの標準ライブラリのみ
            HashSet<string> stdlibAssemblyNameHashSet = AppDomain.CurrentDomain.GetAssemblies ().Select (Assembly => Assembly.GetName ().Name).ToHashSet ();

            return stdlibAssemblyNameHashSet;
        }

        private static HashSet<string> getStdLibNamespaceNameHashSet () {
            //デフォルトの標準ライブラリのみ
            HashSet<HashSet<string>> stdLibNamespaceNameHashSet = new HashSet<HashSet<string>> ();

            HashSet<Assembly> stdlibAssemblyHashSet = AppDomain.CurrentDomain.GetAssemblies ().ToHashSet ();

            foreach (Assembly assembly in stdlibAssemblyHashSet) {

                stdLibNamespaceNameHashSet.Add (assembly.GetTypes ().Select (type => type.Namespace).ToHashSet ());
            }

            return stdLibNamespaceNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }

        private static HashSet<string> getStdLibTypeNameHashSet () {
            //デフォルトの標準ライブラリのみ
            HashSet<HashSet<string>> stdLibTypeNameHashSet = new HashSet<HashSet<string>> ();

            HashSet<Assembly> stdlibAssemblyList = AppDomain.CurrentDomain.GetAssemblies ().ToHashSet ();

            foreach (Assembly assembly in stdlibAssemblyList) {

                stdLibTypeNameHashSet.Add (assembly.GetTypes ().Select (type => type.FullName).ToHashSet ());
            }

            return stdLibTypeNameHashSet.SelectMany (type => type).ToHashSet (); //flatten
        }

        private static Dictionary<string, List<Type>> getStdLibAssemblyTypeList () {
            //デフォルトの標準ライブラリのみ

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

            //TODO 型名でも名前空間名でもアセンブリ名を逆引きしておく必要がある

            //外部ライブラリ指定
            Dictionary<string, List<Type>> assemblyTypeDict = new Dictionary<string, List<Type>> ();

            foreach (string extLibAssembly in extLibAssemblyHashSet) {

                Assembly assembly = Assembly.Load (extLibAssembly);

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
                "CMD: " + appName + SEPARATOR + "["+String.Join (SEPARATOR, OPTION_SHOW_LIST.ToArray ())+"]" +
                RS +
                RS +
                "CMD: " + appName + SEPARATOR + "["+String.Join (SEPARATOR, OPTION_MODE_LIST.ToArray ())+"]" + SEPARATOR + "["+String.Join (SEPARATOR, OPTION_AS_LIST.ToArray ())+"]" + SEPARATOR + " ["+String.Join (SEPARATOR, OPTION_EXAMPLE_TYPE_LIST.ToArray ())+"]" + SEPARATOR + "["+String.Join (SEPARATOR, OPTION_FETCH_INFO_LIST.ToArray ())+"]" +
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
        private static void showInternalLibFilteredByAssemblyNameListInfo (string appName, HashSet<string> targetNameHashSet) {

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
        private static void showInternalLibFilteredByNamespaceNameListInfo (string appName, HashSet<string> targetNameHashSet) {

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
        private static void showInternalLibFilteredByTypeNameListInfo (string appName, HashSet<string> targetNameHashSet) {

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
        private static void showExternalLibFilteredByAssemblyNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibTypeDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            //filter
            List<string> assemblyNameList = extLibTypeDict.Keys.Where (assemblyName => targetNameHashSet.Contains (assemblyName)).ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {
                List<Type> typeList = extLibTypeDict[assemblyName];

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
        private static void showExternalLibFilteredByNamespaceNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibTypeDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = extLibTypeDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = extLibTypeDict[assemblyName].Where (type => targetNameHashSet.Contains (type.Namespace)).ToList ();

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
        private static void showExternalLibFilteredByTypeNameListInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibTypeDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            List<string> assemblyNameList = extLibTypeDict.Keys.ToList ();

            //body
            foreach (string assemblyName in assemblyNameList) {

                //filter
                List<Type> typeList = extLibTypeDict[assemblyName].Where (type => targetNameHashSet.Contains (type.FullName)).ToList ();

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
        private static void showExternalLibInfo (string appName, HashSet<string> targetNameHashSet) {

            Dictionary<string, List<Type>> extLibTypeDict = getExtLibAssemblyTypeList (targetNameHashSet);

            //header
            outputHeader (DEFAULT_OUTPUT_HEADER_LIST);

            //body
            foreach (string assemblyName in extLibTypeDict.Keys) {

                List<Type> typeList = extLibTypeDict[assemblyName].Where (type => targetNameHashSet.Contains (type.FullName)).ToList ();

                foreach (Type type in extLibTypeDict[assemblyName]) {

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

            HashSet<string> showList = null;

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

            switch (DEFAULT_MODE) {
                case OPTION_INTERNAL_LIB:
                    switch (DEFAULT_AS_LIST) {
                        case OPTION_AS_ASSEMBLY_LIST:
                            showInternalLibFilteredByAssemblyNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            showInternalLibFilteredByNamespaceNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_TYPE_LIST:
                            showInternalLibFilteredByTypeNameListInfo (appName, targetNameHashSet);
                            break;
                        default:
                            Usage (appName);
                            break;
                    }
                    break;
                case OPTION_EXTERNAL_LIB:

                    //ここがみそ 外部ライブラリの場合は振る舞いを変える 知っている情報少ないので、大きく拾う
                    // DEFAULT_AS_LIST = OPTION_AS_NAMESPACE_LIST;

                    switch (DEFAULT_AS_LIST) {
                        case OPTION_AS_ASSEMBLY_LIST:
                            showExternalLibFilteredByAssemblyNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_NAMESPACE_LIST:
                            showExternalLibFilteredByNamespaceNameListInfo (appName, targetNameHashSet);
                            break;
                        case OPTION_AS_TYPE_LIST:
                            showExternalLibFilteredByTypeNameListInfo (appName, targetNameHashSet);
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
