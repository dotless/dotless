/* BIGNUM2.C
**
** Routines to do Big Number Arithmetic. These are multi-precision, unsigned
** natural numbers (0, 1, 2, ...). For the storage method, see the BigNum
** typedef in file BIGNUM.H
**
** Released to the public domain by Clifford Rhodes, June 15, 1995, with
** no guarantees of any sort as to accuracy or fitness of use.
*/
typedef unsigned int   size_t;


/*prototypes*/
void *  memcpy(         void * _Dst,          const void * _Src,      size_t _Size);
void * malloc(     size_t _Size);
void * calloc(     size_t _NumOfElements,      size_t _SizeOfElements);
void  free(       void * _Memory);

typedef unsigned short UShort;
typedef unsigned long  ULong;







typedef struct {
      int nlen;      
      UShort *n;     
} BigNum;


BigNum * BigNumAdd(const BigNum *a, const BigNum *b);
BigNum * BigNumSub(const BigNum *a, const BigNum *b);
BigNum * BigNumMul(const BigNum *a, const BigNum *b);
BigNum * BigNumDiv(const BigNum *a, const BigNum *b, BigNum **c);
BigNum * BigNumAlloc(int nlen);
void     BigNumFree(BigNum *b);

BigNum * BigNumAlloc(int nlen)
{
      




      BigNum * big;

      big = (BigNum *) malloc(sizeof(BigNum));
      if (big != ((void *)0))
      {
            big->nlen = nlen;
            if (nlen < 1)
                  big->n = ((void *)0);
            else if ((big->n =
                  (UShort *) calloc(nlen, sizeof(UShort))) == ((void *)0))
            {
                  free(big);
                  return ((void *)0);
            }
      }
      else  return ((void *)0);

      return big;
}

void BigNumFree(BigNum * b)
{
      

      if (b != ((void *)0))
      {
            if (b->n != ((void *)0))
                  free(b->n);
            free(b);
      }
}

BigNum * BigNumDiv(const BigNum * a, const BigNum * b, BigNum ** c)
{
      






      int      i, j, d, bpos;
      UShort   carry, quo;
      long     m1, m2, m3;
      BigNum * quotient, * atemp, * btemp;

      

      for (bpos = 0; bpos < b->nlen && b->n[bpos] == 0; bpos++)
            ;
      if (bpos == b->nlen)  
            return ((void *)0);

      

      if ((btemp = BigNumAlloc(b->nlen - bpos)) == ((void *)0))
            return ((void *)0);
      memcpy(btemp->n, b->n + bpos, btemp->nlen * sizeof(UShort));

      

      carry = (a->n[0] == 0) ? 0 : 1;
      if ((atemp = BigNumAlloc(a->nlen + carry)) == ((void *)0))
      {
            BigNumFree(btemp);
            return ((void *)0);
      }
      memcpy(atemp->n + carry, a->n, a->nlen * sizeof(UShort));

      

      if ((quotient = BigNumAlloc((((1) > (atemp->nlen - btemp->nlen)) ? (1) : (atemp->nlen - btemp->nlen)))) == ((void *)0))
      {
            BigNumFree(atemp);
            BigNumFree(btemp);
            return ((void *)0);
      }
      if ((*c = BigNumAlloc(btemp->nlen)) == ((void *)0))
      {
            BigNumFree(atemp);
            BigNumFree(btemp);
            BigNumFree(quotient);
            return ((void *)0);
      }
      d = 10000 / (btemp->n[0] + 1);
      for (carry = 0, j = atemp->nlen - 1; j >= 0; j--)   
      {
            m1 = ((long) d * (long) *(atemp->n + j)) + (long) carry;
            *(atemp->n + j) = (UShort) (m1 % (long) 10000);
            carry = (UShort) (m1 / (long) 10000);
      }
      for (carry = 0, j = btemp->nlen - 1; j >= 0; j--)
      {
            m1 = ((long) d * (long) *(btemp->n + j)) + (long) carry;
            *(btemp->n + j) = (UShort) (m1 % (long) 10000);
            carry = (UShort) (m1 / (long) 10000);
      }
      for (j = 0; j < (atemp->nlen - btemp->nlen); j++)   
      {
            if (*btemp->n == *(atemp->n + j))
                  quo = 10000 - 1;
            else
            {
                  m1 = (long) *(atemp->n + j) * (long) 10000;
                  m1 = (m1 + (long) *(atemp->n + j + 1)) / (long) *btemp->n;
                  quo = (UShort) m1;
            }
            m3 = (long) *(atemp->n + j) * (long) 10000 +
                  (long) *(atemp->n + j + 1);
            do
            {
                  if (btemp->nlen > 1)
                        m1 = (long) quo * (long) *(btemp->n + 1);
                  else  m1 = 0l;
                  m2 = (long) quo * (long) *btemp->n;
                  m2 = (m3 - m2) * (long) 10000 +
                        (long) *(atemp->n + j + 2);
                  if (m1 > m2)
                        quo--;
            } while (m1 > m2);

            bpos = btemp->nlen - 1;
            i = j + btemp->nlen;
            m2 = 0l;
            while (i >= j)
            {
                  if (bpos >= 0)
                        m1 = (long) quo * (long) *(btemp->n + bpos);
                  else  m1 = 0l;
                  m3 = (long) *(atemp->n + i) - m1 + m2;
                  if (m3 < 0l)
                  {
                        m2 = m3 / (long) 10000;
                        m3 %= (long) 10000;
                        if (m3 < 0l)
                        {
                              m3 += (long) 10000;
                              m2--;
                        }
                  }
                  else  m2 = 0l;
                  *(atemp->n + i) = (UShort) (m3);
                  bpos--;
                  i--;
            }
            if (m2 == 0l)
                  *(quotient->n + j) = quo;
            else
            {
                  *(quotient->n + j) = quo - 1;
                  bpos = btemp->nlen - 1;
                  i = j + btemp->nlen;
                  carry = 0;
                  while (i >= j)
                  {
                        long tmp;

                        if (bpos >= 0)
                              carry += *(btemp->n + bpos);
                        tmp = carry + (long) *(atemp->n + i);
                        if (tmp >= (long) 10000)
                        {
                              tmp -= (long) 10000;
                              carry = 1;
                        }
                        else  carry = 0;
                        *(atemp->n + i) = (UShort) tmp;
                        bpos--;
                        i--;
                  }
            }
      }
      j = atemp->nlen - btemp->nlen;    
      bpos = 0;
      carry = 0;
      while (j < atemp->nlen)            
      {
            m3 = (long) carry * (long) 10000 + (long) *(atemp->n + j);
            *((*c)->n + bpos) = (UShort) (m3 / d);
            carry = (UShort) (m3 % d);
            j++;
            bpos++;
      }
      BigNumFree(atemp);   
      BigNumFree(btemp);

      return quotient;
}