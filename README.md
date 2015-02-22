# Evolution3d
物理演算と進化の再現を利用した実験です。

##概要
物理演算を用いて、疑似的な生態系を再現したものです。

捕食者(黒)と被食者(緑)の二種類が存在します。  
彼らはそれぞれ、体と動きの遺伝子を持っています。  
一定距離まで近づけば、被食者は食べられ、捕食者は増殖します。  
一定時間で捕食者は飢え死に、被食者は増殖します。  

いわゆる、遺伝的アルゴリズムではありません。

##注意
[Microsoft XNA Game Studio 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=23714)を利用しています。実行には[Microsoft XNA Framework Redistributable 4.0](http://www.microsoft.com/en-us/download/details.aspx?id=20914)が必要です。  
XNA Game Studioは既に開発を終了しています。その為Visual Studio 2013以降では動作しません。Visual Studio 2010以前のバージョンで開発してください。なお、物理エンジンの関係上XBox等では動作しません。  
実行中の操作に関しては、起動後Hボタンを押して参照してください。  
設定等はソースコードで直接指定しています。設定を変更したい場合ソースコードを直接参照してください。  
自動保存機能がデフォルトで有効になっています。一定時間ごとに現在の設定が自動で保存されるのでストレージが少ない環境では注意してください。
  
##スクリーンショット
[![【物理演算】火星](http://img.youtube.com/vi/1ZDDZ7CYr50/0.jpg)](http://www.youtube.com/watch?v=1ZDDZ7CYr50)
[![【物理演算】金星](http://img.youtube.com/vi/RfbYT1-Bg_o/0.jpg)](http://www.youtube.com/watch?v=RfbYT1-Bg_o)

##ライセンス
物理エンジンラッパー以外には修正BSDライセンスが適用されます。
詳細は[LICENSE](LICENSE)を参照してください。
