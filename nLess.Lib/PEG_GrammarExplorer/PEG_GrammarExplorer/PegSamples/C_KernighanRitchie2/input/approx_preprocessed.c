
/***************************************************************
 *
 * Fuzzy string searching subroutines
 *
 * Author:    John Rex
 * Date:      August, 1988
 * References: (1) Computer Algorithms, by Sara Baase
 *                 Addison-Wesley, 1988, pp 242-4.
 *             (2) Hall PAV, Dowling GR: "Approximate string matching",
 *                 ACM Computing Surveys, 12:381-402, 1980.
 *
 * Verified on:
 *    Datalite, DeSmet, Ecosoft, Lattice, MetaWare, MSC, Turbo, Watcom
 *
 * Compile time preprocessor switches:
 *    TEST - if defined, include test driver
 *
 * Usage:
 *
 *    char *pattern, *text;  - search for pattern in text
 *    int degree;      - degree of allowed mismatch
 *    char *start, *end;
 *    int howclose;
 *
 *    void App_init(pattern, text, degree);   - setup routine
 *    void App_next(&start, &end, &howclose); - find next match
 *
 *    - searching is done when App_next() returns start==NULL
 *
 **************************************************************/


typedef unsigned int   size_t;


/*prototypes*/
void * malloc(     size_t _Size);
void   free(       void * _Memory);
size_t strlen(       const char * _Str);

static char *Text, *Pattern; 
static int Textloc;          
static int Plen;             
static int Degree;           
static int *Ldiff, *Rdiff;   
static int *Loff,  *Roff;    

void App_init(char *pattern, char *text, int degree)
{
      int i;

      

      Text = text;
      Pattern = pattern;
      Degree = degree;

      

      Plen = strlen(pattern);
      Ldiff  = (int *) malloc(sizeof(int) * (Plen + 1) * 4);
      Rdiff  = Ldiff + Plen + 1;
      Loff   = Rdiff + Plen + 1;
      Roff   = Loff +  Plen + 1;
      for (i = 0; i <= Plen; i++)
      {
            Rdiff[i] = i;   
            Roff[i]  = 1;
      }

      Textloc = -1; 
}

void App_next(char **start, char **end, int *howclose)
{
      int *temp, a, b, c, i;

      *start = ((void *)0);
      while (*start == ((void *)0))  
      {
            if (Text[++Textloc] == '\0') 
                  break;

            temp = Rdiff;   
            Rdiff = Ldiff;  
            Ldiff = temp;   
            Rdiff[0] = 0;   

            temp = Roff;    
            Roff = Loff;
            Loff = temp;
            Roff[1] = 0;

            for (i = 0; i < Plen; i++)    
            {
                  

                  if (Pattern[i] == Text[Textloc])
                        a = Ldiff[i];
                  else  a = Ldiff[i] + 1;
                  b = Ldiff[i+1] + 1;
                  c = Rdiff[i] + 1;

                  

                  if (b < a)
                        a = b;
                  if (c < a)
                        a = c;

                  

                  Rdiff[i+1] = a;
            }

            
            



            if (Plen > 1) for (i=2; i<=Plen; i++)
            {
                  if (Ldiff[i-1] < Rdiff[i])
                        Roff[i] = Loff[i-1] - 1;
                  else if (Rdiff[i-1] < Rdiff[i])
                        Roff[i] = Roff[i-1];
                  else if (Ldiff[i] < Rdiff[i])
                        Roff[i] = Loff[i] - 1;
                  else 
                        Roff[i] = Loff[i-1] - 1;
            }

            

            if (Rdiff[Plen] <= Degree)    
            {
                  *end = Text + Textloc;
                  *start = *end + Roff[Plen];
                  *howclose = Rdiff[Plen];
            }
      }

      if (start == ((void *)0)) 
            free(Ldiff);
}
