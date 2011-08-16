#!/bin/bash

this=`dirname $0`
src=$1
out=$2
config=$3

[ -d $out ] && rm -rf $out || mkdir $out

