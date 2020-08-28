# cspdump
csharpのメタ定義をTSV形式で標準出力に出力するコマンド


# インストール

```
curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/1-0-0/cspdump-linux-x64 -o $HOME/.local/bin/cspdump

curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/1-0-0/cspdump-osx.10.14-x64 -o cspdump

curl -fsSL https://github.com/ukijumotahaneniarukenia/cspdump/releases/download/1-0-0/cspdump-win10-x64 -o cspdump.exe
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


Default: --internal-lib mode

Option: --property-instance --property-static --method-instance --method-static --internal-lib --external-lib

Usage:

CMD: cspdump --external-lib Newtonsoft.Json --method-static

or

CMD: cspdump --external-lib Newtonsoft.Json --method-instance

or

CMD: cspdump --external-lib Newtonsoft.Json --property-static

or

CMD: cspdump --external-lib Newtonsoft.Json --property-instance

or

CMD: cspdump --internal-lib System.DateTime System.Text.NormalizationForm System.Text.Rune --method-static

or

CMD: cspdump System.DateTime System.Text.NormalizationForm System.Text.Rune --method-static


```

標準ライブラリ


```

$ dotnet run --show-assembly-list

$ dotnet run --as-assembly-list System.Collections System.Linq

$ dotnet run --as-assembly-list System.Collections

$ dotnet run --as-assembly-list System.Collections --method-static

$ dotnet run --as-assembly-list System.Collections --method-instance

$ dotnet run --as-assembly-list System.Collections --property-static

$ dotnet run --as-assembly-list System.Collections --property-instance

$ dotnet run --as-assembly-list System.Console System.Collections --property-instance



$ dotnet run --show-namespace-list

$ dotnet run --as-namespace-list System.Collections.Generic

$ dotnet run --as-namespace-list System.Collections.Generic --method-static

$ dotnet run --as-namespace-list System.Collections.Generic --method-instance

$ dotnet run --as-namespace-list System.Collections.Generic --property-static

$ dotnet run --as-namespace-list System.Collections.Generic --property-instance

$ dotnet run --as-namespace-list System.Text.RegularExpressions System.Text.Unicode System.Globalization --property-instance



$ dotnet run --show-type-list

$ dotnet run --as-type-list System.Text.Rune

$ dotnet run --as-type-list System.Text.Rune --method-static

$ dotnet run --as-type-list System.Text.Rune --method-instance

$ dotnet run --as-type-list System.Text.Rune --property-static

$ dotnet run --as-type-list System.Text.Rune --property-instance

$ dotnet run --as-type-list System.Text.Rune System.TimeZone --property-instance

```

外部ライブラリ

分析用のプロジェクトを作成し、分析対象のパッケージをダウンロードしてきてから実行

CMD

```
cspdump --external-lib Newtonsoft.Json --property-static
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


TODO

- [ ] dllファイルをしていしても実行できるようにする
- [ ] 取得列を使えそうなものは随時追加


所感

csharp便利すぎ
