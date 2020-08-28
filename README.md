# cspdump
csharpのメタ定義をTSV形式で標準出力に出力するコマンド


# インストール

```
curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/2-0-0/cspdump-linux-x64 -o $HOME/.local/bin/cspdump

curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/2-0-0/cspdump-osx.10.14-x64 -o cspdump

curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/2-0-0/cspdump-win10-x64 -o cspdump.exe
```

実行権限付与

```
$ ll $HOME/.local/bin/cspdump
-rw-r--r-- 1 aine aine 35M  8月 28 02:09 /home/aine/.local/bin/cspdump

$ chmod 755 $HOME/.local/bin/cspdump

$ ll $HOME/.local/bin/cspdump
-rwxr-xr-x 1 aine aine 35M  8月 28 02:09 /home/aine/.local/bin/cspdump

$ which cspdump
/home/aine/.local/bin/cspdump
```


# 使い方

```
$ cspdump


Default: --internal-lib mode AND --property-instance mode AND --as-type-list mode

Usage:

CMD: cspdump [--show-assembly-list --show-namespace-list --show-type-list]

CMD: cspdump [--internal-lib --external-lib --dll-file-lib] [--as-assembly-list --as-namespace-list --as-type-list]  [Newtonsoft.Json System.DateTime System.Text.NormalizationForm System.Text.Rune /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer Unity.Events.NamedEventArgs] [--property-static --property-instance --method-static --method-instance]

```

標準ライブラリ


```

$ cspdump --show-assembly-list

$ cspdump --as-assembly-list System.Collections System.Linq

$ cspdump --as-assembly-list System.Collections

$ cspdump --as-assembly-list System.Collections --method-static

$ cspdump --as-assembly-list System.Collections --method-instance

$ cspdump --as-assembly-list System.Collections --property-static

$ cspdump --as-assembly-list System.Collections --property-instance

$ cspdump --as-assembly-list System.Console System.Collections --property-instance



$ cspdump --show-namespace-list

$ cspdump --as-namespace-list System.Collections.Generic

$ cspdump --as-namespace-list System.Collections.Generic --method-static

$ cspdump --as-namespace-list System.Collections.Generic --method-instance

$ cspdump --as-namespace-list System.Collections.Generic --property-static

$ cspdump --as-namespace-list System.Collections.Generic --property-instance

$ cspdump --as-namespace-list System.Text.RegularExpressions System.Text.Unicode System.Globalization --property-instance



$ cspdump --show-type-list

$ cspdump --as-type-list System.Text.Rune

$ cspdump --as-type-list System.Text.Rune --method-static

$ cspdump --as-type-list System.Text.Rune --method-instance

$ cspdump --as-type-list System.Text.Rune --property-static

$ cspdump --as-type-list System.Text.Rune --property-instance

$ cspdump --as-type-list System.Text.Rune System.TimeZone --property-instance

```

外部ライブラリ

分析用のプロジェクトを作成し、分析対象のパッケージをダウンロードしてきてから実行

PRE

```
$ mkdir $HOME/wrksp

$ cd $HOME/wrksp

$ dotnet new console

$ dotnet add package RestSharp --version 106.11.5-alpha.0.18
```

CMD

うまく拾えんときは3つ試して、出てきたやつから逆引きし始める

```
$ cspdump --external-lib --as-type-list RestSharp

$ cspdump --external-lib --as-namespace-list RestSharp

$ cspdump --external-lib --as-assembly-list RestSharp

$ cspdump --external-lib --as-assembly-list Google.Apis.Core  --show-assembly-list

$ cspdump --external-lib --as-assembly-list Google.Apis.Core  --show-namespace-list

$ cspdump --external-lib --as-assembly-list Google.Apis.Core  --show-type-list

$ cspdump --external-lib --as-namespace-list RestSharp --method-static

$ cspdump --external-lib --as-namespace-list RestSharp --method-instance

$ cspdump --external-lib --as-namespace-list RestSharp --property-static

$ cspdump --external-lib --as-namespace-list RestSharp --property-instance

$ cspdump --external-lib --as-type-list RestSharp.JsonArray RestSharp.JsonObject RestSharp.RestResponse --property-instance

$ cspdump --external-lib --as-type-list RestSharp.JsonArray RestSharp.JsonObject RestSharp.RestResponse --method-static

$ cspdump --external-lib --as-type-list RestSharp.JsonArray RestSharp.JsonObject RestSharp.RestResponse --property-static

$ cspdump --external-lib --as-type-list RestSharp.JsonArray RestSharp.JsonObject RestSharp.RestResponse --property-instance
```

ERR

ないとこうなる

```
$ cspdump --external-lib Newtonsoft.Json --property-static
Unhandled exception. System.IO.FileNotFoundException: Could not load file or assembly 'Newtonsoft.Json, Culture=neutral, PublicKeyToken=null'. The system cannot find the file specified.

File name: 'Newtonsoft.Json, Culture=neutral, PublicKeyToken=null'
   at System.Reflection.RuntimeAssembly.nLoad(AssemblyName fileName, String codeBase, RuntimeAssembly assemblyContext, StackCrawlMark& stackMark, Boolean throwOnFileNotFound, AssemblyLoadContext assemblyLoadContext)
   at System.Reflection.RuntimeAssembly.InternalLoadAssemblyName(AssemblyName assemblyRef, StackCrawlMark& stackMark, AssemblyLoadContext assemblyLoadContext)
   at System.Reflection.Assembly.Load(String assemblyString)
   at app.Program.getExtLibTypeList(HashSet`1 extLibAssemblyHashSet)
   at app.Program.showExternalLibInfo(String appName, HashSet`1 targetTypeNameHashSet)
   at app.Program.Main(String[] args)


Aborted (core dumped)
```

DLLライブラリ

CMD

たまに出んときがあるので、直指定で

```
$ find $HOME/.nuget/packages -type f | grep -P 'dll$' | grep Unity
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net48/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net48/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net47/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net47/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net40/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net40/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp3.0/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp3.0/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp1.0/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp1.0/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netstandard1.0/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netstandard1.0/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net46/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/net46/Unity.Abstractions.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netstandard2.0/Unity.Container.dll
/home/aine/.nuget/packages/unity/5.11.7/lib/netstandard2.0/Unity.Abstractions.dll


$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll --show-assembly-list
Unity.Container


$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll --show-namespace-list
Unity
Unity.Builder
Unity.Events
Unity.Exceptions
Unity.Extension
Unity.Factories
Unity.Injection
Unity.Lifetime
Unity.ObjectBuilder.BuildPlan.DynamicMethod
Unity.Policy
Unity.Processors
Unity.Registration
Unity.Resolution
Unity.Storage
Unity.Strategies
Unity.Utility

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll --show-type-list


$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll /home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll --as-namespace-list Unity.Utility Unity.Extension

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll /home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll --as-namespace-list Unity.Utility Unity.Extension --method-static

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll /home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll --as-namespace-list Unity.Utility Unity.Extension --method-instance


$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll /home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll --as-namespace-list Unity.Utility Unity.Extension --property-static

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll /home/aine/.nuget/packages/unity/5.11.7/lib/net45/Unity.Abstractions.dll --as-namespace-list Unity.Utility Unity.Extension --property-instance


$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer --as-type-list --method-static

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer --as-type-list --method-instance

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer --as-type-list --property-static

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer --as-type-list --property-instance

$ cspdump --dll-file-lib /home/aine/.nuget/packages/unity/5.11.7/lib/netcoreapp2.0/Unity.Container.dll Unity.UnityContainer Unity.Events.NamedEventArgs --as-type-list --property-instance

```

TODO

- [ ] 取得列を使えそうなものは随時追加


所感

csharp便利すぎ
