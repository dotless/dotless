

typedef unsigned int   size_t;

void * calloc(     size_t _NumOfElements,      size_t _SizeOfElements);


char *alloc_bit_array(size_t bits)
{
      char *set = calloc((bits + 8 - 1) / 8, sizeof(char));

      return set;
}

int getbit(char *set, int number)
{
        set += number / 8;
        return (*set & (1 << (number % 8))) != 0;    
}

void setbit(char *set, int number, int value)
{
        set += number / 8;
        if (value)
                *set |= 1 << (number % 8);           
        else    *set &= ~(1 << (number % 8));        
}

void flipbit(char *set, int number)
{
        set += number / 8;
        *set ^= 1 << (number % 8);                   
}
