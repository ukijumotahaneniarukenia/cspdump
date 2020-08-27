```
$ cd $HOME

$ git clone https://github.com/ukijumotahaneniarukenia/cspdump.git

$ cd cspdump

$ dotnet new console


$ echo '/bin/* /obj/*' | xargs -n1 >.gitignore


$ dotnet add package Newtonsoft.Json --version 12.0.3


$ dotnet run System.DateTime System.Text.NormalizationForm System.Text.Rune --method-static


$ dotnet run --external-lib Newtonsoft.Json --method-static >Newtonsoft-Json-method-static.tsv


$ dotnet run System.DateTime  --method-static >System-DateTime-method-static.tsv

ビルド定義XMLファイル編集

<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <RootNamespace>cspdump</RootNamespace>
    <PublishSingleFile>true</PublishSingleFile>
    <PublishTrimmed>true</PublishTrimmed>
    <PublishReadyToRun>false</PublishReadyToRun>
  </PropertyGroup>

</Project>


リリースビルド
$ time echo linux-x64 osx.10.14-x64 win10-x64 | xargs -n1 | xargs -t -I@ dotnet publish -c Release -r @ --self-contained


アップロード

$ rm -rf $HOME/media/*

$ find -type f | grep -P cspdump | grep Release| grep publish |teip -Gog '(?<!\.[a-zA-Z]+)$|exe$' -- awk '{print $1,1}' | grep 1$| awk '$0=$1' | ruby -F'/' -anle 'p $F[4],$_'|xargs -n2|awk '{print "mkdir -p $HOME/media/"$1"; cp "$2" $HOME/media/"$1"/"}' | bash

$ tree -ugh $HOME/media
/home/aine/media
├── [aine     aine     4.0K]  linux-x64
│   └── [aine     aine      35M]  cspdump
├── [aine     aine     4.0K]  osx.10.14-x64
│   └── [aine     aine      31M]  cspdump
└── [aine     aine     4.0K]  win10-x64
    └── [aine     aine      26M]  cspdump.exe

3 directories, 3 files


ビルドホストで動作確認
$ cd $HOME/media/linux-x64


$ ./cspdump 


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



外部の場合はコマンドをパス通しておいて、分析用のプロジェクトを作成し、そこい外部ライブラリをダウンロードしてきてから実行する

ないとこうなる

$ ./cspdump --external-lib Newtonsoft.Json --method-static
Unhandled exception. System.IO.FileNotFoundException: Could not load file or assembly 'Newtonsoft.Json, Culture=neutral, PublicKeyToken=null'. The system cannot find the file specified.

File name: 'Newtonsoft.Json, Culture=neutral, PublicKeyToken=null'
   at System.Reflection.RuntimeAssembly.nLoad(AssemblyName fileName, String codeBase, RuntimeAssembly assemblyContext, StackCrawlMark& stackMark, Boolean throwOnFileNotFound, AssemblyLoadContext assemblyLoadContext)
   at System.Reflection.RuntimeAssembly.InternalLoadAssemblyName(AssemblyName assemblyRef, StackCrawlMark& stackMark, AssemblyLoadContext assemblyLoadContext)
   at System.Reflection.Assembly.Load(String assemblyString)
   at app.Program.getExtLibTypeList(HashSet`1 extLibAssemblyHashSet)
   at app.Program.showExternalLibInfo(String appName, HashSet`1 targetTypeNameHashSet)
   at app.Program.Main(String[] args)


Aborted (core dumped)

標準ライブラリは行ける

$ ./cspdump System.DateTime System.Text.NormalizationForm
アセンブリ名	名前空間名	型名	インスタンスプロパティ名	インスタンスプロパティ名の戻り値の型名
System.Private.CoreLib	System	System.DateTime	Date	System.DateTime
System.Private.CoreLib	System	System.DateTime	Day	Int32
System.Private.CoreLib	System	System.DateTime	DayOfWeek	System.DayOfWeek
System.Private.CoreLib	System	System.DateTime	DayOfYear	Int32
System.Private.CoreLib	System	System.DateTime	Hour	Int32
System.Private.CoreLib	System	System.DateTime	Kind	System.DateTimeKind
System.Private.CoreLib	System	System.DateTime	Millisecond	Int32
System.Private.CoreLib	System	System.DateTime	Minute	Int32
System.Private.CoreLib	System	System.DateTime	Month	Int32
System.Private.CoreLib	System	System.DateTime	Second	Int32
System.Private.CoreLib	System	System.DateTime	Ticks	Int64
System.Private.CoreLib	System	System.DateTime	TimeOfDay	System.TimeSpan
System.Private.CoreLib	System	System.DateTime	Year	Int32

```
