\ gt-io.fs contains some helper words to ease io


\ creates some variables with current terminal dimensions
\ copyed from http://rosettacode.org/wiki/Terminal_control/Dimensions#Forth
: term-width
   form ( height width )
   nip
;
: term-height
   form ( height width )
   drop
;

\ changes the font format using ansi control characters
\ green bold
: color-green ( -- )
   27 emit ." [1;32m"
;

: color-red ( -- )
   27 emit ." [1;31m"
;

\ changes the font format using ansi control characters
\ resets to normal
: font-normal ( -- )
   27 emit ." [0m"
;

\ calculates the character offset to a align a given text to right half of the terminal
: align-right-half ( -- )
   term-width 2 / ( termWidth/2 )
   spaces
;

\ calculates the character offset to a align a given text to right of the terminal
: align-right ( u -- u )
   dup ( u u ) term-width swap - ( u termWidth-u )
   spaces ( u )
;

\ prints a text with offset
: offset-type ( addr u offset -- )
   >r r@ -rot r> ( offset addr u offset ) term-width swap - ( offset addr u maxlen )
   -rot 0 ( offset maxlen addr u 0 ) do ( offset maxlen addr )
      over i swap ( offset maxlen addr i maxlen ) mod 0= if ( offset maxlen addr )
         rot dup spaces ( maxlen addr offset ) -rot \ if line-break print offset
      then ( offset maxlen addr )
      dup c@ emit 1+ ( offset maxlen nextaddr ) \ print character
      over dup i swap ( offset maxlen newaddr maxlen i maxlen ) mod swap 1- ( offset maxlen newaddr rest maxlen-1 ) = if
         cr \ if last character of line, print cr
      then ( offset maxlen newaddr )
   loop 2drop drop
;

\ prints a special formated text as a log message
: log ( addr u -- )
   cr
   color-green
   term-width 2 / offset-type
   font-normal
   cr
;



\ saves the given gotchitama's state to a file.
: gotchitama-save ( o -- )
;

\ Following taken from ProgrammingForth.pdf, pp 83
\ work in progress
0 value pData
0 value /Data
0 value hData

: FILE-CHECK ( n -- )
    ABORT" File Access Error " ;

: rewind-file ( file-id -- ior ) 0 0 rot reposition-file ;

: MEMORY-CHECK ( n -- ) ABORT" Memory Allocation Error " ;


: InitReadFile ( handle -- size )
    dup rewind-file file-check
    file-size file-check drop ;

: OpenMouth ( caddr len -- )
    r/o open-file file-check dup to hData
    InitReadFile to /Data ;

: slurp ( file-id -- addr len )
    dup InitReadFile
    dup allocate memory-check
    dup to pData dup >r swap rot \ --
    read-file file-check
    r> swap ;


: InitReadFile ( handle -- size )
    dup rewind-file file-check
    file-size file-check drop
    ;

: BURP pData free memory-check ;

: Spit ( caddr len name namelen -- )
    r/w create-file throw >r
    r@ write-file throw
    r> close-file throw ;

0 Value fd-in
: open-input ( addr u -- )  r/o open-file throw to fd-in ;

: save ertl 5 cells s" ~/outputbla " spit ; 
: load s" ~/outputbla " r/o Bin open-file drop ;





