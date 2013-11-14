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

\ changes the font format using ansi control characters
\ resets to normal
: font-normal ( -- )
   27 emit ." [0m"
;

\ prints n spaces
\ : spaces ( n -- )
\   0 ( n 0 ) ?do
\      space
\   loop
\ ;

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
   color-green
   term-width 2 / offset-type
   font-normal
   cr
;



\ saves the given gotchitama's state to a file.
: gotchitama-save ( o -- )


;

