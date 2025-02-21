# VRChat Matrix Barcode Generator [JP]

注意: このリポジトリは実験・研究目的のリポジトリです。商用利用しないようにお願いします。

VRChatのワールド(SDK3)で動作する2次元バーコードジェネレーター

* 任意の文字列(UTF-8エンコード時に最大134byteまで)を2次元バーコードに変換できます

## 最新版

[Download](https://github.com/cleantted/VRChatMatrixBarcodeGenerator/releases/latest)

## 前提アセット

* [VRCSDK3-WORLD](https://vrchat.com/home/download): 2020.05.12.10.33 以降のバージョン
* [UdonSharp](https://github.com/Merlin-san/UdonSharp): v0.15.9 以降のバージョン

## 導入方法

1. `VRCSDK3-WORLD` をインポート
1. `UdonSharp` をインポート
1. `VRChatMatrixBarcodeGenerator` をインポート

## 既知の不具合

* `:` を含む文字列で不正な2次元バーコードが生成されることがある 
* `version >= 4` の2次元バーコード生成する際、fpsが30以下まで低下する

# VRChat Matrix Barcode Generator [EN]

Attention: This repository is for experimental and research purposes. Do not use it for commercial purposes.

This is U# scriptes of Matrix barcode generator for VRChat maps(SDK3)

* Any string (up to 134 bytes when encoded in UTF-8) can be converted to Matrix barcode.

## Latest Release

[Download](https://github.com/cleantted/VRChatMatrixBarcodeGenerator/releases/latest)

## Requirment

* [VRCSDK3-WORLD](https://vrchat.com/home/download): 2020.05.12.10.33 or later
* [UdonSharp](https://github.com/Merlin-san/UdonSharp): v0.15.9 or later

## Instllation

1. Import `VRCSDK3-WORLD`
1. Import `UdonSharp`
1. Import `VRChatMatrixBarcodeGenerator`

## Known Issues.

* Bad Matrix barcodes are sometimes generated by strings containing `:`. 
* The fps drops to less than 30 when generating a Matrix barcode with `version >= 4`.
