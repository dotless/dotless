#!/usr/local/bin/python

# tabview.py -- View a tab-delimited file in a spreadsheet-like display.
# Contributed by A.M. Kuchling <amk@amk.ca>
#
# The tab-delimited file is displayed on screen.  The highlighted
# position is shown in the top-left corner of the screen; below it are
# shown the contents of that cell.
#
#  Movement keys are:
#    Cursor keys: Move the highlighted cell, scrolling if required.
#    Q or q     : Quit
#    TAB        : Page right a screen
#    Home       : Move to the start of this line
#    End        : Move to the end of this line
#    PgUp/PgDn  : Move a page up or down
#    Insert     : Memorize this position
#    Delete     : Return to memorized position (if any)
#
# TODO : 
#    A 'G' for Goto: enter a cell like AA260 and move there
#    A key to re-read the tab-delimited file
#
# Possible projects:
#    Allow editing of cells, and then saving the modified data
#    Add formula evaluation, and you've got a simple spreadsheet
# program.  (Actually, you should allow displaying both via curses and
# via a Tk widget.)  
#

import curses, re, string

def yx2str(y,x):
    "Convert a coordinate pair like 1,26 to AA2"
    if x<26: s=chr(65+x)
    else:
	x=x-26
	s=chr(65+ (x/26) ) + chr(65+ (x%26) )
    s=s+str(y+1)
    return s

coord_pat = re.compile('^(?P<x>[a-zA-Z]{1,2})(?P<y>\d+)$')

def str2yx(s):
    "Convert a string like A1 to a coordinate pair like 0,0"
    match = coord_pat.match(s)
    if not match: return None
    y,x = match.group('y', 'x')
    x = string.upper(x)
    if len(x)==1: x=ord(x)-65
    else:
	x= (ord(x[0])-65)*26 + ord(x[1])-65 + 26
    return string.atoi(y)-1, x

assert yx2str(0,0) == 'A1'
assert yx2str(1,26) == 'AA2'
assert str2yx('AA2') == (1,26)
assert str2yx('B2') == (1,1)
	
class TabFile:
    def __init__(self, scr, filename, column_width=20):
	self.scr=scr ; self.filename = filename
	self.column_width = column_width
	f=open(filename, 'r')
	self.data = []
	while (1):
	    L=f.readline()
	    if L=="": break
	    self.data.append( string.split(L, '\t') )
#	    if len(self.data)>6: break # XXX
	self.x, self.y = 0,0
	self.win_x, self.win_y = 0,0
	self.max_y, self.max_x = self.scr.getmaxyx()
	self.num_columns = int(self.max_x/self.column_width)
	self.scr.clear()	
	self.display()

    def move_to_end(self):
	"""Move the highlighted location to the end of the current line."""

	# This is a method because I didn't want to have the code to 
	# handle the End key be aware of the internals of the TabFile object.
	yp=self.y+self.win_y ; xp=self.x+self.win_x
	if len(self.data)<=yp: end=0
	else: end=len(self.data[yp])-1
	
	# If the end column is on-screen, just change the
	# .x value appropriately.
	if self.win_x <= end < self.win_x + self.num_columns:
	    self.x = end - self.win_x
	else:
	    if end<self.num_columns:
		self.win_x = 0 ; self.x = end
	    else:
		self.x = self.num_columns-1
		self.win_x = end-self.x
	        
	
    def display(self):
	"""Refresh the current display"""
	
	self.scr.addstr(0,0, 
			yx2str(self.y + self.win_y, self.x+self.win_x)+'    ',
			curses.A_REVERSE)

	for y in range(0, self.max_y-3):
	    self.scr.move(y+2,0) ; self.scr.clrtoeol()
	    for x in range(0, int(self.max_x / self.column_width) ):
		self.scr.attrset(curses.A_NORMAL)
		yp=y+self.win_y ; xp=x+self.win_x
		if len(self.data)<=yp: s=""
		elif len(self.data[yp])<=xp: s=""
		else: s=self.data[yp][xp]
		s = string.ljust(s, 15)[0:15]
		if x==self.x and y==self.y: self.scr.attrset(curses.A_STANDOUT)
		self.scr.addstr(y+2, x*self.column_width, s)

	yp=self.y+self.win_y ; xp=self.x+self.win_x
	if len(self.data)<=yp: s=""
	elif len(self.data[yp])<=xp: s=""
	else: s=self.data[yp][xp]

	self.scr.move(1,0) ; self.scr.clrtoeol()
	self.scr.addstr(s[0:self.max_x])
	self.scr.refresh()

def main(stdscr):
    import string, curses, sys

    if len(sys.argv)==1:
	print 'Usage: tabview.py <filename>'
	return
    filename=sys.argv[1]

    # Clear the screen and display the menu of keys
    stdscr.clear()
    file = TabFile(stdscr, filename)
    
    # Main loop:
    while (1):
	stdscr.move(file.y+2, file.x*file.column_width)     # Move the cursor
	c=stdscr.getch()		# Get a keystroke
	if 0<c<256:
	    c=chr(c)
	    # Q or q exits
	    if c in 'Qq': break  
	    # Tab pages one screen to the right
	    elif c=='\t':
		file.win_x = file.win_x + file.num_columns
		file.display()
	    else: pass                  # Ignore incorrect keys

	# Cursor keys
	elif c==curses.key_up:
	    if file.y == 0:
		if file.win_y>0: file.win_y = file.win_y - 1
	    else: file.y=file.y-1
	    file.display()
	elif c==curses.key_down:
	    if file.y < file.max_y-3 -1: file.y=file.y+1
	    else: file.win_y = file.win_y+1
	    file.display()
	elif c==curses.key_left:
	    if file.x == 0:
		if file.win_x>0: file.win_x = file.win_x - 1
	    else: file.x=file.x-1
	    file.display()
	elif c==curses.key_right:
	    if file.x < int(file.max_x/file.column_width)-1: file.x=file.x+1
	    else: file.win_x = file.win_x+1
	    file.display()

	# Home key moves to the start of this line
	elif c==curses.key_home:
	    file.win_x = file.x = 0
	    file.display()
	# End key moves to the end of this line
	elif c==curses.key_end:
	    file.move_to_end()
	    file.display()

	# PageUp moves up a page
	elif c==curses.key_ppage:
	    file.win_y = file.win_y - (file.max_y - 2)
	    if file.win_y<0: file.win_y = 0
	    file.display()
	# PageDn moves down a page
	elif c==curses.key_npage:
	    file.win_y = file.win_y + (file.max_y - 2)
	    if file.win_y<0: file.win_y = 0
	    file.display()
	
	# Insert memorizes the current position
	elif c==curses.key_ic:
	    file.save_y, file.save_x = file.y + file.win_y, file.x + file.win_x
	# Delete restores a saved position
	elif c==curses.key_dc:
	    if hasattr(file, 'save_y'):
		file.x = file.y = 0
		file.win_y, file.win_x = file.save_y, file.save_x
		file.display()
        else: 
	    stdscr.addstr(0,50, curses.keyname(c)+ ' pressed')
	    stdscr.refresh()
	    pass			# Ignore incorrect keys

if __name__=='__main__':
    import curses, traceback
    try:
	# Initialize curses
	stdscr=curses.initscr()
	# Turn off echoing of keys, and enter cbreak mode,
	# where no buffering is performed on keyboard input
	curses.noecho() ; curses.cbreak()

	# In keypad mode, escape sequences for special keys
	# (like the cursor keys) will be interpreted and
	# a special value like curses.key_left will be returned
	stdscr.keypad(1)
	main(stdscr)			# Enter the main loop
	# Set everything back to normal
	stdscr.keypad(0)
	curses.echo() ; curses.nocbreak()
	curses.endwin()			# Terminate curses
    except:
        # In the event of an error, restore the terminal
	# to a sane state.
	stdscr.keypad(0)
	curses.echo() ; curses.nocbreak()
	curses.endwin()
	traceback.print_exc()		# Print the exception



