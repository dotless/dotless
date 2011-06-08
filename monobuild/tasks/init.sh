#!/bin/bash

this=`dirname $0`

src=$1
out=$2
config=$3

$this/clean.sh $src $out $config

version=`git describe --abbrev=0` | sed 's/v//g'
title="DotLess"
description="DotLess"
company="DotLess"
product="DotLess"
commit="Mono"
copyright=

for path in \
  $src/dotless.Core/Properties \
  $src/dotless.Test/Properties \
  $src/dotless.Compiler/Properties 
do
  [ -d $path ] || mkdir $path
  cat $this/AssemblyInfo.cs.template \
  | sed "s/\$version/$version/g" \
  | sed "s/\$title/$title/g" \
  | sed "s/\$description/$description/g" \
  | sed "s/\$company/$company/g" \
  | sed "s/\$product/$product/g" \
  | sed "s/\$commit/$commit/g" \
  | sed "s/\$copyright/$commit/g" \
  > "$path/AssemblyInfo.cs"
done
