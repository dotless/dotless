/*
**  Bit counter by Ratko Tomic
*/



long   atol(       const char *_Str);
int    printf(     const char * _Format, ...);

int bit_count(long x)
{
        int n = 0;

        if (x) do
              n++;
        while (0 != (x = x&(x-1))) ;
        return(n);
}



main(int argc, char *argv[])
{
      long n;

      while(--argc)
      {
            int i;

            n = atol(*++argv);
            i = bit_count(n);
            printf("%ld contains %d bits set\n",n, i);
      }
      return 0;
}

