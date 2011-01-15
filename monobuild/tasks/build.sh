#!/bin/bash


this=`dirname $0`
src=$1
out=$2
config=$3

$this/init.sh $src $out $config

echo "Compiling to directory $out ..."
xbuild $src/dotless.Compiler/dotless.Compiler.csproj /p:OutDir=$out /p:Configuration=$config

