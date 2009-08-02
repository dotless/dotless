
/* BIGNUM1.C
**
** Routines to do Big Number Arithmetic. These are multi-precision, unsigned
** natural numbers (0, 1, 2, ...). For the storage method, see the BigNum
** typedef in file BIGNUM.H
**
** Released to the public domain by Clifford Rhodes, June 15, 1995, with
** no guarantees of any sort as to accuracy or fitness of use.
*/
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


BigNum * BigNumAdd(const BigNum * a, const BigNum * b)
{
      




      UShort  carry = 0;
      int     size, la, lb;
      long    tsum;
      BigNum  * sum;

      

      size = (((a->nlen) > (b->nlen)) ? (a->nlen) : (b->nlen)) + 1;
      if ((sum = BigNumAlloc(size)) == ((void *)0))
            return ((void *)0);

      

      la = a->nlen - 1;
      lb = b->nlen - 1;
      size--;

      while (la >= 0 && lb >= 0)    
      {
            tsum = carry + (long) *(a->n + la) + (long) *(b->n + lb);
            *(sum->n + size) = (UShort) (tsum % (long) 10000);
            carry = (tsum / (long) 10000) ? 1 : 0;
            la--;
            lb--;
            size--;
      }
      if (lb < 0)                   
      {
            while (carry && la >= 0)
            {
                  tsum = carry + (long) *(a->n + la);
                  *(sum->n + size) = (UShort) (tsum % (long) 10000);
                  carry = (tsum / (long) 10000) ? 1 : 0;
                  la--;
                  size--;
            }
            while (la >= 0)
            {
                  *(sum->n + size) = *(a->n + la);
                  la--;
                  size--;
            }
      }
      else                          
      {
            while (carry && lb >= 0)
            {
                  tsum = carry + (long) *(b->n + lb);
                  *(sum->n + size) = (UShort) (tsum % (long) 10000);
                  carry = (tsum / (long) 10000) ? 1 : 0;
                  lb--;
                  size--;
            }
            while (lb >= 0)
            {
                  *(sum->n + size) = *(b->n + lb);
                  lb--;
                  size--;
            }
      }
      *(sum->n + size) = carry;

      return sum;
}

BigNum * BigNumSub(const BigNum * a, const BigNum * b)
{
      





      int      size, la, lb, borrow = 0;
      long     tdiff;
      BigNum * diff;

      

      if ((diff = BigNumAlloc(a->nlen)) == ((void *)0))
            return ((void *)0);

      la = a->nlen - 1;
      size = la;
      lb = b->nlen - 1;

      while (lb >= 0)
      {
            tdiff = (long) *(a->n + la) - (long) *(b->n + lb) - borrow;
            if (tdiff < 0)
            {
                  tdiff += (long) (10000 - 1);
                  borrow = 1;
            }
            else  borrow = 0;
            *(diff->n + size) = (UShort) tdiff + borrow;
            la--;
            lb--;
            size--;
      }
      while (la >= 0)          
      {
            tdiff = (long) *(a->n + la) - borrow;
            if (tdiff < 0)
            {
                  tdiff += (long) (10000 - 1);
                  borrow = 1;
            }
            else  borrow = 0;
            *(diff->n + size) = (UShort) tdiff + borrow;
            la--;
            size--;
      }
      if (borrow)   
      {
            BigNumFree(diff);
            return ((void *)0);
      }
      return diff;
}

BigNum * BigNumMul(const BigNum * a, const BigNum * b)
{
      




      int      size, la, lb, apos, bpos, ppos;
      UShort   carry;
      BigNum * product;

      

      size = a->nlen + b->nlen;
      if ((product = BigNumAlloc(size)) == ((void *)0))
            return ((void *)0);

      la = a->nlen - 1;
      lb = b->nlen - 1;
      size--;

      bpos = lb;

      while (bpos >= 0)             
      {
            ppos = size + (bpos - lb);    

            if (*(b->n + bpos) == 0) 
                  ppos = ppos - la - 1;
            else                    
            {
                  apos = la;
                  carry = 0;
                  while (apos >= 0) 
                  {
                        ULong temp;

                        temp = (ULong) *(a->n + apos) *
                              (ULong) *(b->n + bpos) + carry;

                        




                        temp += (ULong) *(product->n + ppos);

                        

                        *(product->n + ppos) =
                              (UShort) (temp % (ULong) 10000);
                        carry = (UShort) (temp / (ULong) 10000);

                        apos--;
                        ppos--;
                  }
                  *(product->n + ppos) = carry;
            }
            bpos--;
      }
      return product;
}
