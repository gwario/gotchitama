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



: split ( str len separator len -- tokens count )
  here >r 2swap
  begin
    2dup 2,             \ save this token ( addr len )
    2over search        \ find next separator
  while
    dup negate  here 2 cells -  +!  \ adjust last token length
    2over nip /string               \ start next search past separator
  repeat
  2drop 2drop
  r>  here over -   ( tokens length )
  dup negate allot           \ reclaim dictionary
  2 cells / ;                \ turn byte length into token count



: .tokens ( tokens count -- )
  1 ?do dup 2@ type ." ." cell+ cell+ loop 2@ type ;
 
s" Hello,How,Are,You,Today" s" ," split .tokens  \ Hello.How.Are.You.Today



\ SERIALIZATION 

7 cells constant gotchisize \ the size in bytes of gotchitama information that gets serialized
Create gotchibuffer gotchisize allot 
: gotchifile s\" ~/output" ;

0 Value fd-in
: open-input ( addr u -- )  r/o open-file throw to fd-in ;
: close-input ( -- )  fd-in close-file throw ;

\ save len bytes from caddr to filename
: save ( caddr len filename filenamelen -- )
    r/w create-file throw >r
    r@ write-file throw
    r> close-file throw ;

: import-gotchi ( o -- ) >r
    gotchibuffer 2 cell+ ( name-addr ) gotchibuffer 4 cell+ @ ( name-addr name-len )
    r@ -rot ( o name-addr name-len )
    ['] constant execute-parsing ( -- )
    gotchibuffer r> cell + gotchisize move ; 

\ Takes a "gotchitama new" as the first argument o
\ This is necessary as we only import the instance variable data and not the other necessary information of a gotchitama instance as the definition/offset of methods.
: load-gotchi ( o c-addr u ) open-input gotchibuffer gotchisize fd-in read-line throw close-input 2drop import-gotchi ;
: save-gotchi ( name namelen caddr -- ) cell + ( name namelen caddr ) gotchisize ( name namelen caddr len ) 2swap save ;

\ save a gotchitama: <file> <gotchitama> save-gotchi (zB gotchifile ertl save-gotchi)
\ load a gotchi: gotchitama new <file> load-gotchi (zB gotchitama new gotchifile load-gotchi)
