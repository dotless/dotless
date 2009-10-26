#!/bin/env python

# Paginate a text file, adding a header and footer 

import sys, time, string

# If no arguments were given, print a helpful message
if len(sys.argv)!=2:
    print 'Usage: pyprint filename'
    sys.exit(0)

class PrinterFormatter:
    def __init__(self, filename, page_len=58):	
	# Save the time of creation for inclusion in the header
	import time
	self.now=time.asctime(time.localtime(time.time()))

	# Save the filename and page length
	self.filename=filename ; self.page_len = page_len
	
	# Zero all the counters
	self.page=0 ; self.count=0 ; self.header_written=0

    def write_header(self):
	# If the header for this page has just been written, don't
	# write another one.
	if self.header_written: return

	# Increment the page count, and reset the line count
	self.header_written=1 ; self.count=1 ; self.page=self.page+1

	# Write the header
	header=self.filename
	p=str(self.page) ; header=string.ljust(header, 38-len(p))+'['+p+']'
	header=string.ljust(header, 79-len(self.now))+self.now
	sys.stdout.write(header+'\n\n')

    def writeline(self, L):
	# If the line is exactly 80 lines long, chop off any trailing
	# newline, since the printhead will wrap around
        length=len(L)
	if (length % 80) == 0 and L and L[-1]=='\n': L=L[:-1]

	# If we've printed a pageful of lines, output a linefeed and
	# output the header.
	if self.count>self.page_len:
	    sys.stdout.write('')
	    self.write_header()

	# Print the actual line of text
	sys.stdout.write(L) 
	self.count=self.count+1 ; self.header_written=0

# Open the input file, and create a PrinterFormatter object, passing
# it the filename to put in the page header.
    
f=open(sys.argv[1], 'r')
o=PrinterFormatter(sys.argv[1])
o.write_header()			# Print the header on the first page

# Iterate over all the lines in the file; the writeline() method will
# output them and automatically add page breaks and headers where
# required.
while (1):
    L=f.readline()
    if L=="": break
    o.writeline(L)

# Write a final page break and close the input file.
sys.stdout.write('')
f.close()



 

