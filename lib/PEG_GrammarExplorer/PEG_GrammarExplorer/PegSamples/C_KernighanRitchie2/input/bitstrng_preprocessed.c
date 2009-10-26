/*
** bitstring(): print bit pattern of bytes formatted to string.
**
** By J. Blauth, Sept. 1992. Hereby placed into the public domain.
**
** byze:    value to transform to bitstring.
** biz:     count of bits to be shown (counted from lowest bit, can be any
**          even or odd number).
** strwid:  total width the string shall have. Since between every 4 bits a
**          blank (0x20) is inserted (not added after lowest bit), width of
**          bitformat only is (biz+(biz/4-1)). Bits are printed right aligned,
**          positions from highest bit to start of string filled with blanks.
**          If value of strwid smaller than space needed to print all bits,
**          strwid is ignored (e.g.:
**                bitstr(s,b,16,5) results in 19 chars +'\0').
**
**   EXAMPLE:
**   for (j = 1; j <= 16; j++) { bitstring(s, j, j, 16); puts(s); }
**       1:                1
**       2:               10
**       3:              011
**       d: 0 0000 0000 1101
**       e: 00 0000 0000 1110
**       f: 000 0000 0000 1111
*/

#include "bitops.h"

int printf(const char * _Format, ...);

void bitstring(char *str, long byze, int biz, int strwid);

void bitstring(char *str, long byze, int biz, int strwid)
{
      int i, j;

      j = strwid - (biz + (biz >> 2)- (biz % 4 ? 0 : 1));
      for (i = 0; i < j; i++)
            *str++ = ' ';
      while (--biz >= 0)
      {
            *str++ = ((byze >> biz) & 1) + '0';
            if (!(biz % 4) && biz)
                  *str++ = ' ';
      }
      *str = '\0';
}


int main(void)
{
      char s[80]; long j;
      for (j = 1L; j <= 16L; j++)
      {
            bitstring(s, (long)j, (int)j, 16);
            printf("%2ld: %s\n", j, s);
      }
      return 0;
}

#endif /* TEST */
