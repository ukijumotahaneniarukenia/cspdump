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

CMD

```
$ cspdump  System.Text.Rune --method-static
```

OUT

```
アセンブリ名	名前空間名	型名	スタティックメソッド名	スタティックメソッドの戻り値の型名	スタティックメソッドの仮引数の個数	スタティックメソッドの仮引数の位置番号	スタティックメソッドの仮引数の変数名	スタティックメソッドの仮引数の型名
System.Private.CoreLib	System.Text	System.Text.Rune	op_Equality	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_Inequality	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_LessThan	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_LessThanOrEqual	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_GreaterThan	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_GreaterThanOrEqual	System.Boolean	2	0,1	left,right	Rune,Rune
System.Private.CoreLib	System.Text	System.Text.Rune	op_Explicit	System.Text.Rune	1	0	ch	Char
System.Private.CoreLib	System.Text	System.Text.Rune	op_Explicit	System.Text.Rune	1	0	value	UInt32
System.Private.CoreLib	System.Text	System.Text.Rune	op_Explicit	System.Text.Rune	1	0	value	Int32
System.Private.CoreLib	System.Text	System.Text.Rune	get_ReplacementChar	System.Text.Rune	0	ないよーん	ないよーん	ないよーん
System.Private.CoreLib	System.Text	System.Text.Rune	DecodeFromUtf16	System.Buffers.OperationStatus	3	0,1,2	source,result,charsConsumed	ReadOnlySpan`1,Rune&,Int32&
System.Private.CoreLib	System.Text	System.Text.Rune	DecodeFromUtf8	System.Buffers.OperationStatus	3	0,1,2	source,result,bytesConsumed	ReadOnlySpan`1,Rune&,Int32&
System.Private.CoreLib	System.Text	System.Text.Rune	DecodeLastFromUtf16	System.Buffers.OperationStatus	3	0,1,2	source,result,charsConsumed	ReadOnlySpan`1,Rune&,Int32&
System.Private.CoreLib	System.Text	System.Text.Rune	DecodeLastFromUtf8	System.Buffers.OperationStatus	3	0,1,2	source,value,bytesConsumed	ReadOnlySpan`1,Rune&,Int32&
System.Private.CoreLib	System.Text	System.Text.Rune	GetRuneAt	System.Text.Rune	2	0,1	input,index	String,Int32
System.Private.CoreLib	System.Text	System.Text.Rune	IsValid	System.Boolean	1	0	value	Int32
System.Private.CoreLib	System.Text	System.Text.Rune	IsValid	System.Boolean	1	0	value	UInt32
System.Private.CoreLib	System.Text	System.Text.Rune	TryCreate	System.Boolean	2	0,1	ch,result	Char,Rune&
System.Private.CoreLib	System.Text	System.Text.Rune	TryCreate	System.Boolean	3	0,1,2	highSurrogate,lowSurrogate,result	Char,Char,Rune&
System.Private.CoreLib	System.Text	System.Text.Rune	TryCreate	System.Boolean	2	0,1	value,result	Int32,Rune&
System.Private.CoreLib	System.Text	System.Text.Rune	TryCreate	System.Boolean	2	0,1	value,result	UInt32,Rune&
System.Private.CoreLib	System.Text	System.Text.Rune	TryGetRuneAt	System.Boolean	3	0,1,2	input,index,value	String,Int32,Rune&
System.Private.CoreLib	System.Text	System.Text.Rune	GetNumericValue	System.Double	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	GetUnicodeCategory	System.Globalization.UnicodeCategory	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsControl	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsDigit	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsLetter	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsLetterOrDigit	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsLower	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsNumber	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsPunctuation	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsSeparator	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsSymbol	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsUpper	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	IsWhiteSpace	System.Boolean	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	ToLower	System.Text.Rune	2	0,1	value,culture	Rune,CultureInfo
System.Private.CoreLib	System.Text	System.Text.Rune	ToLowerInvariant	System.Text.Rune	1	0	value	Rune
System.Private.CoreLib	System.Text	System.Text.Rune	ToUpper	System.Text.Rune	2	0,1	value,culture	Rune,CultureInfo
System.Private.CoreLib	System.Text	System.Text.Rune	ToUpperInvariant	System.Text.Rune	1	0	value	Rune
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
