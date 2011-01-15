#!/bin/bash

this=`dirname $0`

target=$1
config=$2

src=`readlink -m $this/../src`
out=`readlink -m $this/bin`

task="$this/tasks/$target.sh"

[ -f $task ] && $task $src $out/ $config || echo "Unknown target: $target"
