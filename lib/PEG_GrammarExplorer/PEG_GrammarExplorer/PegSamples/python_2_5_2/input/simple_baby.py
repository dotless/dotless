
def comp2(n):
    "Constrain n to the range of a 32-bit 2s-complement number"
    if not (-pow(2,31) <=n < pow(2,31)): 
	if n>0: n = -pow(2,31) + (n-pow(2,31))
	else:   n = pow(2,31) + n + pow(2,31) 
    return n

class SSEM:
    def __init__(self):
	# There are 32 32-bit words which make up the memory
	self.store = [0]*32
	# Zero the accumulator and the program counter
	self.accum = self.CI = 0
	self.stopped = 0

    def step(self):
	"""Perform a single instruction, updating the store and registers."""

	if self.stopped: return  # Do nothing when the machine is stopped
	# 2.3: "The CI is always incremented prior to fetching an
	#       instruction for execution..."
	self.CI = comp2( (self.CI + 1) )

	# Fetch the instruction
	inst = self.store[ self.CI & 31]

	# Decode the line number affected by the instruction, and the
	# function number
	lineno, funcno = inst & 31, (inst >> 13) & 7

	assert 0<= funcno <=7
	if funcno == 0:
	    # s,C : JMP : Copy content of Store line to CI
	    self.CI = self.store[ lineno ]
	elif funcno == 1:
	    # c+s,C : JRP : Add content of Store line to CI
	    self.CI = comp2(self.CI + self.store[ lineno ])
	elif funcno == 2:
	    # -s,A : LDN : Copy content of Store line, negated, to accum
	    self.accum = comp2 (- self.store[ lineno ])
	elif funcno == 3:
	    # a,S : STO : Copy content of acc. to Store line
	    self.store[ lineno ] = self.accum
	elif funcno == 4 or funcno==5:
	    # a-s,A : SUB : Subtract content of Store line from accum
	    self.accum = comp2( self.accum - self.store[ lineno ] )
	elif funcno == 6:
	    # Test : CMP : Skip next instruction if content of accum
	    #              is negative
	    if self.accum < 0: self.CI = comp2(self.CI + 1)
	elif funcno == 7:
	    # Stop : STOP : Light "Stop" neon and halt the machine
	    self.stopped = 1
	
	# Assertions to test invariants
	assert -pow(2,31) <= self.accum <pow(2,31)
	assert -pow(2,31) <= self.store[ lineno ] <pow(2,31)
	assert -pow(2,31) <= self.CI <pow(2,31)



import re, string
inst_pat = re.compile("""\s*
      (?P<mnemonic>JMP|JRP|LDN|STOP|STO|SUB|CMP)  
       \s* 
      (?P<arg>\d+)        # numeric argument""", re.VERBOSE | re.IGNORECASE)

# There are two variants of the SUB instruction; SUB has an opcode of
# 4, and SUBF has an opcode of 5.  Both have the exact same effects,
# and differ only in their opcode.

opcode = {'JMP':0, 'JRP':1, 'LDN':2, 'STO':3, 'SUB':4, 'SUBF':5,
	  'CMP':6, 'STOP':7 }
	  

def assemble(inst_list):
    if len(inst_list)>32:
	raise ValueError, "Only room for 32 instructions"
    machine = SSEM() 

    for i in range(len(inst_list)):
	m = inst_pat.match(string.upper(inst_list[i]))
	if not m: raise ValueError, "Error in assembly code: " + inst_list[i]
	mnemonic, arg = m.group('mnemonic'), m.group('arg')	
	arg = string.atoi(arg)
	machine.store[ i ] = arg | (opcode[mnemonic]  << 13)
    return machine

if __name__ == '__main__':
    # Try the addition test
    p=pow(2,31)

    assert (comp2(p-1), comp2(p), comp2(p+1)) == (p-1, -p, -p+1)
    assert (comp2(-p), comp2(-p-1), comp2(-p-2)) == (-p, p-1, p-2)

    m=assemble(['JMP 0', 
		'LDN 28', 
		'SUB 29', 
		'STO 30', 
		'LDN 30', 
		'STO 30', 
		'STOP 0'])
    x = p-1 ; y = -p
    m.store[28] = x ; m.store[29] = y
    while not m.stopped:
	m.step()
    assert m.store[30] == comp2(x+y)
	     

